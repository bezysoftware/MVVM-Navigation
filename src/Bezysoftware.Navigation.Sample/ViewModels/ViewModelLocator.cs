namespace Bezysoftware.Navigation.Sample.ViewModels
{
    using Bezysoftware.Navigation.Lookup;
    using Bezysoftware.Navigation.Platform;
    using Bezysoftware.Navigation.Sample.AssemblyResolver;
    using Bezysoftware.Navigation.Sample.Dto;
    using Bezysoftware.Navigation.Sample.Views;
    using Bezysoftware.Navigation.StatePersistence;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            var unity = new UnityContainer();

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(unity));

            unity
                .RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager())
                .RegisterType<IViewLocator, ViewLocator>(new ContainerControlledLifetimeManager())
                .RegisterType<IStore, Store>(new ContainerControlledLifetimeManager())
                .RegisterType<IViewModelLocator, ViewModelServiceLocator>(new ContainerControlledLifetimeManager())
                .RegisterType<IPlatformNavigator, PlatformNavigator>(new ContainerControlledLifetimeManager())
                .RegisterType<IApplicationFrameProvider, CurrentWindowFrameProvider>(new ContainerControlledLifetimeManager())
                .RegisterType<IStatePersistor, StatePersistor>(new ContainerControlledLifetimeManager())
                .RegisterType<IAssemblyResolver, ThisAssemblyResolver>(new ContainerControlledLifetimeManager())
                .RegisterInstance(typeof(TaskScheduler), TaskScheduler.Default)
                .RegisterType<INavigationInterceptor, AdaptiveNavigationInterceptor>("Adaptive", new ContainerControlledLifetimeManager())
                .RegisterType<IEnumerable<INavigationInterceptor>, INavigationInterceptor[]>(new ContainerControlledLifetimeManager())
                .RegisterType<MainViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<SecondViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<ThirdViewModel>(new ContainerControlledLifetimeManager());

            // manually register association for the main page to speed up application startup
            unity.Resolve<IViewLocator>().RegisterAssociation<MainViewModel, MainPage>();

            ServiceLocator.Current.GetInstance<INavigationService>().Navigated += ViewModelLocator_Navigated;
        }

        private static void ViewModelLocator_Navigated(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine(e.OriginalViewModelType);
        }

        public static void Init()
        { }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public SecondViewModel Second
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SecondViewModel>();
            }
        }

        public ThirdViewModel Third
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ThirdViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}