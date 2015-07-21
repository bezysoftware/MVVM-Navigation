namespace Bezysoftware.Navigation
{
    using Bezysoftware.Navigation.Lookup;
    using Bezysoftware.Navigation.Platform;
    using Bezysoftware.Navigation.StatePersistence;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// The navigation service.
    /// </summary>
    public class NavigationService : INavigationService
    {        
        #region Fields

        private readonly IViewModelLocator viewModelLocator;
        private readonly IViewLocator viewLocator;
        private readonly IStatePersistor statePersistor;
        private readonly TaskScheduler uiTaskScheduler; // calls to underlying IPlatformNavigator need to be thru the uiTaskScheduler
        private readonly IPlatformNavigator platformNavigator;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="statePersistor"> The state persistor. </param>
        /// <param name="viewModelLocator"> The ViewModel instance locator. </param>
        /// <param name="viewLocator"> The ViewLocator</param>
        /// <param name="uiTaskScheduler"> UI task scheduler. </param>
        public NavigationService(
            IViewLocator viewLocator, 
            IViewModelLocator viewModelLocator, 
            IStatePersistor statePersistor, 
            IPlatformNavigator platformNavigator,
            TaskScheduler uiTaskScheduler) 
        {
            this.viewModelLocator = viewModelLocator;
            this.viewLocator = viewLocator;
            this.uiTaskScheduler = uiTaskScheduler;
            this.statePersistor = statePersistor;
            this.platformNavigator = platformNavigator;

            this.platformNavigator.BackNavigationRequested += this.PlatformBackNavigationRequested;
        }

        public async Task PersistApplicationStateAsync()
        {
            this.statePersistor.NavigationPath = this.platformNavigator.GetNavigationState();
            await this.statePersistor.SaveAsync();
        }

        public async Task RestoreApplicationStateAsync()
        {
            if (!await this.statePersistor.ContainsStateAsync())
            {
                return;
            }

            var states = await this.statePersistor.GetAllStatesAsync();
            var lastState = states.Last();

            var allViewTypes = await Task.WhenAll(states.Select(s => this.viewLocator.GetViewTypeAsync(s.ViewModelType)));

            // this should also cause navigating to the last View, so no need to navigate there manually
            this.platformNavigator.RestoreNavigationState(statePersistor.NavigationPath, allViewTypes);

            foreach (var state in states.Where(s => s != null))
            {
                var viewType = await this.viewLocator.GetViewTypeAsync(state.ViewModelType);
                var attribute = viewType.GetTypeInfo().GetCustomAttribute<AssociatedViewModelAttribute>();
                var viewModel = await this.viewModelLocator.GetInstanceAsync(attribute.ViewModel);

                var activationData = state.ActivationData;
                var vmState = state.ViewModelState.Value;

                // try to activate the ViewModel
                if (activationData == null)
                {
                    // TODO: when VM has StatePersistence set to None || StateOnly, this always gets called because activationData is set to null
                    this.ActivateViewModel(viewModel, NavigationType.Forward);
                }
                else
                {
                    var method = this.GetType().GetRuntimeMethods().Where(m => m.Name == "Activate" && m.GetParameters().Count() == 2).First();
                    var genericMethod = method.MakeGenericMethod(state.ActivationData.GetType());
                    genericMethod.Invoke(this, new[] { viewModel, activationData });
                }

                this.statePersistor.SetViewModelState(viewModel, vmState);
            }
        }

        #endregion

        #region Navigate

        public async Task NavigateAsync<TData>(Type viewModelType, TData data)
        {
            await this.NavigateAndActivateAsync(viewModelType, vm => this.ActivateViewModel(vm, NavigationType.Forward, data), data);
        }

        public async Task NavigateAsync(Type viewModelType)
        {
            await this.NavigateAndActivateAsync(viewModelType, vm => this.ActivateViewModel(vm, NavigationType.Forward), null);
        }

        #endregion

        #region GoBack

        /// <summary>
        /// Return control back to the original ViewModel
        /// </summary>
        public async Task GoBackAsync()
        {
            await this.GoBackAndActivateAsync(vm => this.ActivateViewModel(vm, NavigationType.Backward));
        }

        /// <summary>
        /// Go back to the previous ViewModel and activate it with specified result data.
        /// </summary>
        /// <param name="data"> The result. </param>
        /// <typeparam name="TData"> Type of the data. </typeparam> 
        public async Task GoBackAsync<TData>(TData data) 
        {
            await this.GoBackAndActivateAsync(vm => this.ActivateViewModel(vm, NavigationType.Backward, data));
        }

        #endregion

        #region Private methods

        private async Task NavigateAndActivateAsync(Type viewModelType, Action<object> activationAction, object activationData)
        {
            if (!await this.DeactivatePreviousViewModelsAsync(NavigationType.Forward, viewModelType))
            {
                return;
            }

            var viewType = await this.viewLocator.GetViewTypeAsync(viewModelType);
            var viewModel = await this.viewModelLocator.GetInstanceAsync(viewModelType);

            // activate the ViewModel instance
            activationAction(viewModel);

            // navigate to target View
            this.platformNavigator.Navigate(viewType);

            // push state
            await this.statePersistor.PushStateAsync(viewModel, activationData, this.platformNavigator.GetNavigationState());
        }
        
        private async Task GoBackAndActivateAsync(Action<object> activationAction)
        {
            if (! await this.DeactivatePreviousViewModelsAsync(NavigationType.Backward, null))
            {
                return;
            }
            
            var lastState = await this.statePersistor.PopStateAsync();
            var lastViewType = await this.viewLocator.GetViewTypeAsync(lastState.ViewModelType);
            var nextState = (await this.statePersistor.GetAllStatesAsync()).Last();
            var viewModel = await this.viewModelLocator.GetInstanceAsync(nextState.ViewModelType);
            var viewType = await this.viewLocator.GetViewTypeAsync(nextState.ViewModelType);

            // activate the ViewModelInstance
            activationAction(viewModel);

            // go back to previous View
            this.platformNavigator.GoBack(lastState.ViewModelType, lastViewType);
        }

        private async Task<bool> DeactivatePreviousViewModelsAsync(NavigationType navigationType, Type newViewModelType)
        {
            IEnumerable<State> stack = await this.statePersistor.GetAllStatesAsync();
            if (stack.Count() > 0)
            {
                var toDeactivate = stack
                    .SkipWhile(i => i.ViewModelType != newViewModelType)
                    .Select(i => i.ViewModelType)
                    .Reverse()
                    .ToList();

                if (toDeactivate.Count == 0)
                {
                    toDeactivate.Add(stack.Last().ViewModelType);
                }
                else
                {
                    // target ViewModel already exists on the stack, meaning from the perspective of the existing ViewModels, the navigation is going back
                    navigationType = NavigationType.Backward;

                    // pop the extra ViewModels from the stack
                    foreach (var _ in toDeactivate)
                    {
                        await this.statePersistor.PopStateAsync();
                    }
                }

                // first check if all can deactivate
                foreach (var viewModelType in toDeactivate)
                {
                    var viewModel = await this.viewModelLocator.GetInstanceAsync(viewModelType);
                    if (!await this.CanDeactivateViewModelAsync(viewModel, navigationType))
                    {
                        return false;
                    }
                }

                // if all can deactivate, do so
                foreach (var viewModelType in toDeactivate)
                {
                    var viewModel = await this.viewModelLocator.GetInstanceAsync(viewModelType);
                    await this.DeactivateViewModelAsync(viewModel, navigationType);
                }
            }

            return true;
        }

        private async Task<bool> CanDeactivateViewModelAsync(object target, NavigationType navigationType)
        {
            var query = target as IDeactivateQuery;
            if (query != null)
            {
                if (!await query.CanDeactivateAsync(navigationType))
                {
                    return false;
                }
            }

            return true;
        }

        private async Task DeactivateViewModelAsync(object target, NavigationType navigationType)
        {
            var deactivate = target as IDeactivate;
            if (deactivate != null)
            {
                await deactivate.DeactivateAsync(navigationType);
            }
        }

        private void ActivateViewModel(object target, NavigationType navigationType)
        {
            var instance = target as IActivate;
            if (instance != null)
            {
                instance.Activate(navigationType);
            }
        }

        private void ActivateViewModel<TData>(object target, NavigationType navigationType, TData data)
        {
            var instance = target as IActivate<TData>;
            if (instance != null)
            {
                instance.Activate(navigationType, data);
            }
        }

        private async void PlatformBackNavigationRequested(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.platformNavigator.CanGoBack)
            {
                e.Cancel = true;
                await this.GoBackAsync();
            }
            else
            {
                // platform navigator cannot go back, which means there is just one page in the stack, allowing back navigation will deactivate the app
                // TODO: DeactivateQuery
            }
        }

        #endregion
    }
}
