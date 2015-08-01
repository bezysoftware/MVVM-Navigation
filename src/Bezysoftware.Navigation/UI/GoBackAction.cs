namespace Bezysoftware.Navigation.UI
{
    using Microsoft.Xaml.Interactivity;
    using Microsoft.Practices.ServiceLocation;
    using Windows.UI.Xaml;

    public class GoBackAction : DependencyObject, IAction
    {
        public static readonly DependencyProperty DeactivationDataProperty = DependencyProperty.Register("DeactivationData", typeof(object), typeof(GoBackAction), new PropertyMetadata(null));
        public static readonly DependencyProperty NavigationServiceProperty = DependencyProperty.Register("NavigationService", typeof(INavigationService), typeof(GoBackAction), new PropertyMetadata(null));

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
        public object DeactivationData
        {
            get { return (object)GetValue(DeactivationDataProperty); }
            set { SetValue(DeactivationDataProperty, value); }
        }
        
        public object Execute(object sender, object parameter)
        {
            var service = this.GetNavigationService();
            
            if (this.DeactivationData == null)
            {
                service.GoBack();
            }
            else
            {
                service.GoBack(this.DeactivationData);
            }

            return true;
        }
        
        private INavigationService GetNavigationService()
        {
            return this.NavigationService ?? ServiceLocator.Current.GetInstance<INavigationService>();
        }
    }
}
