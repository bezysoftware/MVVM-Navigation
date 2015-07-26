namespace Bezysoftware.Navigation.UI
{
    using System;
    using Microsoft.Xaml.Interactivity;
    using Windows.UI.Xaml;
    using Microsoft.Practices.ServiceLocation;
    using Bezysoftware.Navigation.Lookup;
    using System.Reflection;
    using System.Linq;

    public class GoBackAction : DependencyObject, IAction
    {
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
        public object DectivationData
        {
            get { return (object)GetValue(ActivationDataProperty); }
            set { SetValue(ActivationDataProperty, value); }
        }
        
        public object Execute(object sender, object parameter)
        {
            var service = this.GetNavigationService();
            
            if (this.DectivationData == null)
            {
                service.GoBack();
            }
            else
            {
                var method = service.GetType().GetRuntimeMethods().First(m => m.Name == "GoBackAsync" && m.GetParameters().Count() == 1);
                var generic = method.MakeGenericMethod(this.DectivationData.GetType());
                generic.Invoke(service, new[] { this.DectivationData });
            }

            return true;
        }
        
        private INavigationService GetNavigationService()
        {
            return this.NavigationService ?? ServiceLocator.Current.GetInstance<INavigationService>();
        }
    }
}
