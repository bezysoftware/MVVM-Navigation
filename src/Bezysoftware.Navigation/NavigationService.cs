namespace Bezysoftware.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Bezysoftware.Navigation.Lookup;
    using Bezysoftware.Navigation.Platform;
    using Bezysoftware.Navigation.StatePersistence;
    using Bezysoftware.Navigation.Activation;


    /// <summary>
    /// The navigation service.
    /// </summary>
    public class NavigationService : INavigationService
    {        
        #region Fields

        private readonly IViewModelLocator viewModelLocator;
        private readonly IViewLocator viewLocator;
        private readonly IStatePersistor statePersistor;
        private readonly IPlatformNavigator platformNavigator;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="statePersistor"> The state persistor. </param>
        /// <param name="viewModelLocator"> The ViewModel instance locator. </param>
        /// <param name="viewLocator"> The ViewLocator</param>
        /// <param name="platformNavigator"> The platform navigator. </param>
        public NavigationService(
            IViewLocator viewLocator, 
            IViewModelLocator viewModelLocator, 
            IStatePersistor statePersistor, 
            IPlatformNavigator platformNavigator) 
        {
            this.viewModelLocator = viewModelLocator;
            this.viewLocator = viewLocator;
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
            this.platformNavigator.RestoreNavigationState(this.statePersistor.NavigationPath, allViewTypes);

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
                    ViewModelActivator.ActivateViewModel(viewModel, NavigationType.Forward);
                }
                else
                {
                    ViewModelActivator.ActivateViewModel(viewModel, NavigationType.Forward, activationData);
                }

                this.statePersistor.SetViewModelState(viewModel, vmState);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the active ViewModel.
        /// </summary>
        public Type ActiveViewModelType
        {
            get;
            private set;
        } 

        #endregion

        #region Events

        /// <summary>
        /// Event raised when navigation happens.
        /// </summary>
        public event EventHandler<NavigationEventArgs> Navigated; 

        #endregion

        #region Navigate

        public async Task<bool> NavigateAsync(Type viewModelType, object data)
        {
            return await this.NavigateAndActivateAsync(viewModelType, data);
        }

        public async Task<bool> NavigateAsync(Type viewModelType)
        {
            return await this.NavigateAndActivateAsync(viewModelType, null);
        }

        #endregion

        #region GoBack

        /// <summary>
        /// Return control back to the original ViewModel
        /// </summary>
        public async Task<bool> GoBackAsync()
        {
            return await this.GoBackAndActivateAsync(null);
        }

        /// <summary>
        /// Go back to the previous ViewModel and activate it with specified result data.
        /// </summary>
        /// <param name="data"> The result. </param>
        public async Task<bool> GoBackAsync(object data) 
        {
            return await this.GoBackAndActivateAsync(data);
        }

        #endregion

        #region Private methods

        private async Task<bool> NavigateAndActivateAsync(Type viewModelType, object activationData)
        {
            var parameters = new DeactivationParameters(viewModelType, activationData);

            if (!await this.DeactivatePreviousViewModelsAsync(NavigationType.Forward, viewModelType, parameters))
            {
                return false;
            }

            var viewType = await this.viewLocator.GetViewTypeAsync(viewModelType);
            var viewModel = await this.viewModelLocator.GetInstanceAsync(viewModelType);

            var currentViewModel = this.ActiveViewModelType;
            var currentView = currentViewModel != null ? await this.viewLocator.GetViewTypeAsync(currentViewModel) : null;

            // activating the ViewModel instance
            if (parameters.DeactivationData == null)
            {
                ViewModelActivator.ActivatingViewModel(viewModel, NavigationType.Forward);
            }
            else
            {
                ViewModelActivator.ActivatingViewModel(viewModel, NavigationType.Forward, parameters.DeactivationData);
            }

            // navigate to target View
            this.platformNavigator.Navigate(viewType);

            // push state
            await this.statePersistor.PushStateAsync(viewModel, parameters.DeactivationData, this.platformNavigator.GetNavigationState());

            this.ActiveViewModelType = viewModelType;

            // raise navigated event
            this.Navigated?.Invoke(this, new NavigationEventArgs(NavigationType.Forward, currentViewModel, currentView, viewModelType, viewType, parameters.DeactivationData));

            // activate the ViewModel instance
            if (parameters.DeactivationData == null)
            {
                ViewModelActivator.ActivateViewModel(viewModel, NavigationType.Forward);
            }
            else
            {
                ViewModelActivator.ActivateViewModel(viewModel, NavigationType.Forward, parameters.DeactivationData);
            }

            return true;
        }
        
        private async Task<bool> GoBackAndActivateAsync(object deactivationData)
        {
            var nextState = (await this.statePersistor.GetAllStatesAsync()).Select(s => s).Reverse().Skip(1).First();
            var parameters = new DeactivationParameters(nextState.ViewModelType, deactivationData);

            if (! await this.DeactivatePreviousViewModelsAsync(NavigationType.Backward, null, parameters))
            {
                return false;
            }

            var viewModel = await this.viewModelLocator.GetInstanceAsync(nextState.ViewModelType);
            var viewType = await this.viewLocator.GetViewTypeAsync(nextState.ViewModelType);
            var lastState = await this.statePersistor.PopStateAsync();
            var lastViewType = await this.viewLocator.GetViewTypeAsync(lastState.ViewModelType);

            // activating the ViewModelInstance
            if (parameters.DeactivationData == null)
            {
                ViewModelActivator.ActivatingViewModel(viewModel, NavigationType.Backward);
            }
            else
            {
                ViewModelActivator.ActivatingViewModel(viewModel, NavigationType.Backward, parameters.DeactivationData);
            }

            // go back to previous View
            this.platformNavigator.GoBack(lastState.ViewModelType, lastViewType);

            this.ActiveViewModelType = nextState.ViewModelType;

            // raise navigated event
            this.Navigated?.Invoke(this, new NavigationEventArgs(NavigationType.Backward, lastState.ViewModelType, lastViewType, nextState.ViewModelType, viewType, parameters.DeactivationData));

            // activate the ViewModel instance
            if (parameters.DeactivationData == null)
            {
                ViewModelActivator.ActivateViewModel(viewModel, NavigationType.Backward);
            }
            else
            {
                ViewModelActivator.ActivateViewModel(viewModel, NavigationType.Backward, parameters.DeactivationData);
            }

            return true;
        }

        private async Task<bool> DeactivatePreviousViewModelsAsync(NavigationType navigationType, Type newViewModelTypeOverride, DeactivationParameters parameters)
        {
            IEnumerable<State> stack = await this.statePersistor.GetAllStatesAsync();
            bool navigationTypeOverriden = false;

            if (stack.Any())
            {
                var viewModelsToDeactivate = stack
                    .SkipWhile(i => i.ViewModelType != newViewModelTypeOverride)
                    .Select(i => i.ViewModelType)
                    .Reverse()
                    .ToList();

                if (viewModelsToDeactivate.Count == 0)
                {
                    viewModelsToDeactivate.Add(stack.Last().ViewModelType);
                }
                else
                {
                    // target ViewModel already exists on the stack, meaning from the perspective of the existing ViewModels, the navigation is going back
                    navigationType = NavigationType.Backward;
                    navigationTypeOverriden = true;
                }

                // first check if all can deactivate
                foreach (var viewModelType in viewModelsToDeactivate)
                {
                    var viewModel = await this.viewModelLocator.GetInstanceAsync(viewModelType);
                    if (!await ViewModelActivator.CanDeactivateViewModelAsync(viewModel, navigationType, parameters))
                    {
                        return false;
                    }
                }

                // if all can deactivate, do so
                foreach (var viewModelType in viewModelsToDeactivate)
                {
                    var viewModel = await this.viewModelLocator.GetInstanceAsync(viewModelType);
                    await ViewModelActivator.DeactivateViewModelAsync(viewModel, navigationType, parameters);
                }

                if (navigationTypeOverriden)
                {
                    foreach (var viewModelType in viewModelsToDeactivate)
                    {
                        // pop the extra ViewModels from the persistence stack
                        await this.statePersistor.PopStateAsync();

                        // when navigating forward to existing ViewModel (large screen with the first View visible) we must manually unhook existing ViewTypes, since they are no longer active
                        var viewType = await this.viewLocator.GetViewTypeAsync(viewModelType);
                        //this.platformNavigator.UnhookType(viewType);
                        this.platformNavigator.GoBack(viewModelType, viewType);
                    }
                }
            }

            return true;
        }

        private async void PlatformBackNavigationRequested(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // there are still some ViewModels in the stack which can be deactivated
            if ((await this.statePersistor.GetAllStatesAsync()).Count > 1)
            {
                e.Cancel = true;
                await this.GoBackAsync();
            }
            else
            {
                // TODO: DeactivateQuery
            }
        }

        #endregion
    }
}
