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
            get { return (INavigationService)this.GetValue(NavigationServiceProperty); }
            set { this.SetValue(NavigationServiceProperty, value); }
        }

        /// <summary>
        /// Data to pass to target ViewModel.
        /// </summary>
        public object DeactivationData
        {
            get { return (object)this.GetValue(DeactivationDataProperty); }
            set { this.SetValue(DeactivationDataProperty, value); }
        }
        
        public object Execute(object sender, object parameter)
        {
            var service = this.GetNavigationService();
            
            if (this.DeactivationData == null)
            {
                service.GoBackAsync();
            }
            else
            {
                service.GoBackAsync(this.DeactivationData);
            }

            return true;
        }
        
        private INavigationService GetNavigationService()
        {
            return this.NavigationService ?? ServiceLocator.Current.GetInstance<INavigationService>();
        }
    }
}
