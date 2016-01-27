namespace Bezysoftware.Navigation.UI
{
    using Lookup;
    using Microsoft.Practices.ServiceLocation;
    using System;
    using System.Windows.Input;
    using Windows.UI.Xaml;

    public class GoBackCommand : DependencyObject, ICommand
    {
        public static readonly DependencyProperty ActivationDataProperty = DependencyProperty.Register("ActivationData", typeof(object), typeof(NavigateToViewAction), new PropertyMetadata(null));
        public static readonly DependencyProperty NavigationServiceProperty = DependencyProperty.Register("NavigationService", typeof(INavigationService), typeof(NavigateToViewAction), new PropertyMetadata(null));

        public event EventHandler CanExecuteChanged;

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
        public object ActivationData
        {
            get { return (object)this.GetValue(ActivationDataProperty); }
            set { this.SetValue(ActivationDataProperty, value); }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var service = this.GetNavigationService();
        
            if (this.ActivationData == null)
            {
                service.GoBack();
            }
            else
            {
                service.GoBack(this.ActivationData);
            }
        }

        private INavigationService GetNavigationService()
        {
            return this.NavigationService ?? ServiceLocator.Current.GetInstance<INavigationService>();
        }
    }
}
