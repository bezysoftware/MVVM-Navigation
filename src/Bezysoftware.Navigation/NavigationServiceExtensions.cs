namespace Bezysoftware.Navigation
{
    using Bezysoftware.Navigation.Activation;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Extensions for <see cref="INavigationService"/>
    /// </summary>
    public static class NavigationServiceExtensions
    {
        /// <summary>
        /// Navigate to a given ViewModel passing some data to it 
        /// </summary>
        /// <param name="data"> The data to be passed to target ViewModel. </param>
        /// <typeparam name="TViewModel"> Target ViewModel type </typeparam>
        /// <typeparam name="TData"> Type of data to be passed to target ViewModel </typeparam>        
        public static Task<bool> NavigateAsync<TViewModel, TData>(this INavigationService service, TData data) 
        {
            return service.NavigateAsync(typeof(TViewModel), data);
        }

        /// <summary>
        /// Navigate to a given ViewModel
        /// </summary>
        /// <typeparam name="TViewModel"> Target ViewModel type </typeparam>
        public static Task<bool> NavigateAsync<TViewModel>(this INavigationService service)
        {
            return service.NavigateAsync(typeof(TViewModel));
        }

        /// <summary>
        /// Navigate to a given ViewModel passing some data to it. This is a convenience method in case you don't care when the navigation finishes. If you do, use <see cref="NavigateAsync{TViewModel, TData}(INavigationService, TData)"/>
        /// </summary>
        /// <param name="data"> The data to be passed to target ViewModel. </param>
        /// <typeparam name="TViewModel"> Target ViewModel type </typeparam>
        /// <typeparam name="TData"> Type of data to be passed to target ViewModel </typeparam>      
        [Obsolete("Don't use this method as you lose stack trace in case of an exception.")]
        public static void Navigate<TViewModel, TData>(this INavigationService service, TData data) where TViewModel : IActivate<TData>
        {
            service.NavigateAsync(typeof(TViewModel), data);
        }

        /// <summary>
        /// Navigate to a given ViewModel. This is a convenience method in case you don't care when the navigation finishes. If you do, use <see cref="NavigateAsync{TViewModel}(INavigationService)"/>
        /// </summary>
        /// <typeparam name="TViewModel"> Target ViewModel type </typeparam>
        [Obsolete("Don't use this method as you lose stack trace in case of an exception.")]
        public static void Navigate<TViewModel>(this INavigationService service)
        {
            service.NavigateAsync(typeof(TViewModel));
        }

        /// <summary>
        /// Navigate to a given ViewModel passing some data to it. This is a convenience method in case you don't care when the navigation finishes. If you do, use <see cref="NavigateAsync{TViewModel, TData}(INavigationService, TData)"/>
        /// </summary>
        /// <param name="viewModelType"> Type of the ViewModel to navigate to. </param>
        /// <param name="data"> The data to be passed to target ViewModel. </param>
        [Obsolete("Don't use this method as you lose stack trace in case of an exception.")]
        public static void Navigate(this INavigationService service, Type viewModelType, object data)
        {
            service.NavigateAsync(viewModelType, data);
        }

        /// <summary>
        /// Navigate to a given ViewModel. This is a convenience method in case you don't care when the navigation finishes. If you do, use <see cref="NavigateAsync{TViewModel}(INavigationService)"/>
        /// </summary>
        [Obsolete("Don't use this method as you lose stack trace in case of an exception.")]
        public static void Navigate(this INavigationService service, Type viewModelType)
        {
            service.NavigateAsync(viewModelType);
        }

        /// <summary>
        /// Return control back to the previous ViewModel. This is a convenience method in case you don't care when the navigation finishes. If you do, use GoBackAsync(). />
        /// </summary>
        [Obsolete("Don't use this method as you lose stack trace in case of an exception.")]
        public static void GoBack(this INavigationService service)
        {
            service.GoBackAsync();
        }

        /// <summary>
        /// Return control back to the previous ViewModel with the specified result data. This is a convenience method in case you don't care when the navigation finishes. If you do, use GoBackAsync{TData}(TData). />
        /// </summary>
        /// <param name="result"> The result. </param>
        [Obsolete("Don't use this method as you lose stack trace in case of an exception.")]
        public static void GoBack(this INavigationService service, object result)
        {
            service.GoBackAsync(result);
        }
    }
}
