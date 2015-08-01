namespace Bezysoftware.Navigation.UI
{
    using System;
    using Microsoft.Xaml.Interactivity;
    using Windows.UI.Xaml;
    using Microsoft.Practices.ServiceLocation;
    using Bezysoftware.Navigation.Lookup;



    /// <summary>
    /// Navigation action which can be used inside a <see cref="IBehavior"/> directly in xaml.
    /// </summary>
    public class NavigateToViewAction : DependencyObject, IAction
    {
        public static readonly DependencyProperty TargetViewTypeProperty = DependencyProperty.Register("TargetViewType", typeof(Type), typeof(NavigateToViewAction), new PropertyMetadata(null));
        public static readonly DependencyProperty ActivationDataProperty = DependencyProperty.Register("ActivationData", typeof(object), typeof(NavigateToViewAction), new PropertyMetadata(null));
        public static readonly DependencyProperty NavigationServiceProperty = DependencyProperty.Register("NavigationService", typeof(INavigationService), typeof(NavigateToViewAction), new PropertyMetadata(null));

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
        /// Type of View to navigate to.
        /// </summary>
        public Type TargetViewType
        {
            get { return (Type)GetValue(TargetViewTypeProperty); }
            set { SetValue(TargetViewTypeProperty, value); }
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
            return ServiceLocator.Current.GetInstance<IViewModelLocator>().GetAssociatedViewModelType(this.TargetViewType);
        }

        private INavigationService GetNavigationService()
        {
            return this.NavigationService ?? ServiceLocator.Current.GetInstance<INavigationService>();
        }
    }
}
