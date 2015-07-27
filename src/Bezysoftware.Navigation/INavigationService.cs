namespace Bezysoftware.Navigation
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Implemented by services that provide <see cref="Uri" /> based navigation.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Event raised when navigation happens.
        /// </summary>
        event EventHandler<NavigationEventArgs> Navigated;

        /// <summary>
        /// Navigate to a given ViewModel passing some data to it.
        /// </summary>
        /// <param name="data"> The data to be passed to target ViewModel. </param>
        /// <param name="viewModelType"> Target ViewModel type. </typeparam>
        /// <typeparam name="TData"> Type of data to be passed to target ViewModel. </typeparam>        
        Task<bool> NavigateAsync<TData>(Type viewModelType, TData data);

        /// <summary>
        /// Navigate to a given ViewModel.
        /// </summary>
        /// <param name="viewModelType"> Target ViewModel type. </typeparam>        
        Task<bool> NavigateAsync(Type viewModelType);

        /// <summary>
        /// Return control back to the previous ViewModel.
        /// </summary>
        Task<bool> GoBackAsync();

        /// <summary>
        /// Return control back to the previous ViewModel with the specified result data.
        /// </summary>
        /// <param name="result"> The result. </param>
        /// <typeparam name="TData"> Type of the data to pass to previous ViewModel. </typeparam>
        Task<bool> GoBackAsync<TData>(TData result);

        /// <summary>
        /// Persist current application state.
        /// </summary>
        Task PersistApplicationStateAsync();

        /// <summary>
        /// Restore persisted application state.
        /// </summary>
        Task RestoreApplicationStateAsync();
    }
}
