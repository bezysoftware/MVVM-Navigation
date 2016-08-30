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
        /// Gets the type of the active ViewModel.
        /// </summary>
        Type ActiveViewModelType { get; }

        /// <summary>
        /// Navigate to a given ViewModel passing some data to it.
        /// </summary>
        /// <param name="viewModelType"> Type of the ViewModel to navigate to. </param>
        /// <param name="data"> The data to be passed to target ViewModel. </param>
        Task<bool> NavigateAsync(Type viewModelType, object data, bool isRoot = false);

        /// <summary>
        /// Navigate to a given ViewModel.
        /// </summary>
        /// <param name="viewModelType"> Type of the ViewModel to navigate to. </param>
        Task<bool> NavigateAsync(Type viewModelType, bool isRoot = false);

        /// <summary>
        /// Return control back to the previous ViewModel.
        /// </summary>
        Task<bool> GoBackAsync();

        /// <summary>
        /// Return control back to the previous ViewModel with the specified result data.
        /// </summary>
        /// <param name="result"> The result. </param>
        Task<bool> GoBackAsync(object result);

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
