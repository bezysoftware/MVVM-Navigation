namespace Bezysoftware.Navigation.Platform
{
    using Bezysoftware.Navigation.Lookup;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;
    using System.ComponentModel;
    using Windows.UI.Core;

    public class PlatformNavigator : IPlatformNavigator
    {
        private readonly IApplicationFrameProvider frameProvider;
        private readonly IEnumerable<INavigationInterceptor> navigationInterceptors;
        private readonly IViewModelLocator viewModelLocator;
        private readonly List<Type> interceptedViewTypes;

        private bool stateShouldBeEmpty;

        private Frame frame;

        public PlatformNavigator(IApplicationFrameProvider frameProvider, IViewModelLocator viewModelLocator, IEnumerable<INavigationInterceptor> navigationInterceptors)
        {
            this.frameProvider = frameProvider;
            this.viewModelLocator = viewModelLocator;
            this.navigationInterceptors = navigationInterceptors;
            this.interceptedViewTypes = new List<Type>();

            this.InterceptBackNavigation = true;

            foreach (var interceptor in this.navigationInterceptors)
            {
                interceptor.ConditionChanged += this.InterceptorConditionChanged;
            }
        }

        /// <summary>
        /// Raised when user hits the back button.
        /// </summary>
        public event EventHandler<CancelEventArgs> BackNavigationRequested;

        /// <summary>
        /// Checks whether navigator can go back.
        /// </summary>
        public bool CanGoBack
        {
            get
            {
                return this.GetFrame().CanGoBack;
            }
        }

        /// <summary>
        /// Indicates whether back button navigation should be handled. It set to false, it is the caller's responsibility to handle back button. Default value is true.
        /// </summary>
        public bool InterceptBackNavigation
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of page types in the back stack including the current one
        /// </summary>
        public IEnumerable<Type> BackStackPageTypes
        {
            get
            {
                return this.GetFrame().BackStack.Select(b => b.SourcePageType).Concat(new[] { this.GetFrame().CurrentSourcePageType });
            }
        }


        /// <summary>
        /// The go back.
        /// </summary>
        /// <param name="currentViewModelType">Type of current ViewModel</param>
        /// <param name="associatedViewType"> Type of View associated to current ViewModel type. /></param>
        public void GoBack(Type currentViewModelType, Type associatedViewType)
        {
            this.stateShouldBeEmpty = false;

            var currentViewType = this.GetFrame().CurrentSourcePageType;
            var shouldBeViewModelType = this.viewModelLocator.GetAssociatedViewModelType(currentViewType);

            foreach (var interceptor in this.navigationInterceptors)
            {
                interceptor.UnhookType(associatedViewType);
            }

            this.interceptedViewTypes.RemoveAll(i => i == associatedViewType);

            if (shouldBeViewModelType == currentViewModelType)
            {
                if (!this.CanGoBack)
                {
                    this.stateShouldBeEmpty = true;
                    return;
                }

                this.GetFrame().GoBack();
            }
        }

        /// <summary> 
        /// Navigates to specified view. It should be a <see cref="Page"/>.
        /// </summary>
        /// <param name="viewType"> Type of View to navigate to. </param>
        public void Navigate(Type viewType)
        {
            var vmType = this.viewModelLocator.GetAssociatedViewModelType(viewType);

            this.interceptedViewTypes.Add(viewType);

            foreach (var interceptor in this.navigationInterceptors)
            {
                interceptor.HookType(viewType);
            }

            if (this.InterceptNavigationForward(viewType, vmType))
            {
                return;
            }

            var frame = this.GetFrame();
            frame.Navigate(viewType);

            // in previous back navigation it was requested to completely wipe the backstack - but that cannot be done, so remove the last (zero index) page here.
            if (this.stateShouldBeEmpty)
            {
                // this is a hack to trigger the OnNavigated event *after* the zero item on stack is removed.
                frame.Navigate(viewType);
                frame.BackStack.RemoveAt(0);
                frame.GoBack();
                this.stateShouldBeEmpty = false;
            }
        }

        /// <summary>
        /// Get current state of navigation serialized as a string.
        /// </summary>
        /// <returns> Serialized navigation state. </returns>
        public string GetNavigationState()
        {
            return this.GetFrame().GetNavigationState();
        }

        /// <summary>
        /// Restores the navigation state.
        /// </summary>
        /// <param name="state"> Serialized view navigation state. </param>
        /// <param name="allViewTypes"> Types of all views. Not all of them need to be in the seralized navigation state due to navigation interception. </param>
        public void RestoreNavigationState(string state, IEnumerable<Type> allViewTypes)
        {
            var viewTypesList = allViewTypes.ToList();

            this.GetFrame().SetNavigationState(state);

            // unhook any existing interceptors
            foreach (var viewType in this.interceptedViewTypes)
            {
                foreach (var interceptor in this.navigationInterceptors)
                {
                    interceptor.UnhookType(viewType);
                }
            }

            this.interceptedViewTypes.Clear();
            this.interceptedViewTypes.AddRange(viewTypesList);

            // now hook up the interceptors with all views
            foreach (var viewType in viewTypesList)
            {
                foreach (var interceptor in this.navigationInterceptors)
                {
                    interceptor.HookType(viewType);
                }
            }
        }

        public void UnhookType(Type viewType)
        {
            this.interceptedViewTypes.Remove(viewType);
            foreach (var interceptor in this.navigationInterceptors)
            {
                interceptor.UnhookType(viewType);
            }
        }

        private Frame GetFrame()
        {
            if (this.frame == null)
            {
                this.frame = this.frameProvider.GetCurrentFrame();
                SystemNavigationManager.GetForCurrentView().BackRequested += this.BackRequested;
            }

            return this.frame;
        }

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (this.InterceptBackNavigation && !e.Handled)
            {
                var cancelArgs = new CancelEventArgs();
                this.BackNavigationRequested?.Invoke(sender, cancelArgs);
                e.Handled = cancelArgs.Cancel;
            }
        } 

        private bool InterceptNavigationForward(Type viewType, Type viewModelType)
        {
            return this.navigationInterceptors.FirstOrDefault(i => i.InterceptNavigation(viewType)) != null;
        }

        private void InterceptorConditionChanged(object sender, TypeEventArgs e)
        {
            var indexOfChangedType = this.interceptedViewTypes.IndexOf(e.Type);
            if (indexOfChangedType >= 0)
            {
                var frame = this.GetFrame();
                var vmType = this.viewModelLocator.GetAssociatedViewModelType(e.Type);
                var intercepted = this.InterceptNavigationForward(e.Type, vmType);

                if (intercepted)
                {
                    // the navigation should now be intercepted, if the navigation did occur, remove it from stack / go back if it is last page
                    var inStack = frame.BackStack.FirstOrDefault(p => p.SourcePageType == e.Type);
                    if (inStack != null)
                    {
                        frame.BackStack.Remove(inStack);
                    }
                    else if (frame.CurrentSourcePageType == e.Type)
                    {
                        frame.GoBack();
                    }
                }
                else if (this.BackStackPageTypes.FirstOrDefault(t => t == e.Type) == null)
                {
                    // the navigation should not be intercepted, if navigation didn't occur, insert it into backstack / navigate to it if it's last page
                    var backTypes = this.BackStackPageTypes.ToList();
                    for (int i = 0; i < backTypes.Count; i++)
                    {
                        if (indexOfChangedType < this.interceptedViewTypes.IndexOf(backTypes[i]))
                        {
                            frame.BackStack.Insert(i, new PageStackEntry(e.Type, null, null));
                            return;
                        }
                    }

                    frame.Navigate(e.Type);
                }
            }
        }
    };
}
