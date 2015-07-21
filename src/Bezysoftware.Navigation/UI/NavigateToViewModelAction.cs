namespace Bezysoftware.Navigation.UI
{
    using System;
    using Microsoft.Xaml.Interactivity;
    using Windows.UI.Xaml;
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Navigation action which can be used inside a <see cref="IBehavior"/> directly in xaml.
    /// </summary>
    public class NavigateToViewModelAction : DependencyObject, IAction
    {
        public static readonly DependencyProperty TargetViewModelKeyProperty = DependencyProperty.Register("TargetViewModelType", typeof(string), typeof(NavigateToViewModelAction), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty ActivationDataProperty = DependencyProperty.Register("ActivationData", typeof(object), typeof(NavigateToViewModelAction), new PropertyMetadata(null));
        public static readonly DependencyProperty NavigationServiceProperty = DependencyProperty.Register("NavigationService", typeof(INavigationService), typeof(NavigateToViewModelAction), new PropertyMetadata(null));

        /// <summary>
        /// The navigation service. If not provided, the instance is resolved using <see cref="ServiceLocator"/>.
        /// </summary>
        public INavigationService NavigationService
        {
            get { return (INavigationService)GetValue(NavigationServiceProperty); }
            set { SetValue(NavigationServiceProperty, value); }
        }

        /// <summary>
        /// Data to pass to target ViewModel.
        /// </summary>
        public object ActivationData
        {
            get { return (object)GetValue(ActivationDataProperty); }
            set { SetValue(ActivationDataProperty, value); }
        }

        /// <summary>
        /// Type of ViewModel to navigate to.
        /// </summary>
        public string TargetViewModelKey
        {
            get { return (string)GetValue(TargetViewModelKeyProperty); }
            set { SetValue(TargetViewModelKeyProperty, value); }
        }

        public object Execute(object sender, object parameter)
        {
            var service = this.GetNavigationService();
            var viewModelType = this.GetViewModelType();

            if (this.ActivationData == null)
            {
                service.Navigate(viewModelType);
            }
            else
            {
                service.Navigate(viewModelType, this.ActivationData);
            }

            return true;
        }

        private Type GetViewModelType()
        {
            return ServiceLocator.Current.GetInstance<object>(this.TargetViewModelKey).GetType();
        }

        private INavigationService GetNavigationService()
        {
            return this.NavigationService ?? ServiceLocator.Current.GetInstance<INavigationService>();
        }
    }
}
