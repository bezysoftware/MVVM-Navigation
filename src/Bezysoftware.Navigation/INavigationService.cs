using System;
using System.Threading.Tasks;

namespace Bezysoftware.Navigation
{
    /// <summary>
    /// Implemented by services that provide <see cref="Uri" /> based navigation.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Navigate to a given ViewModel passing some data to it.
        /// </summary>
        /// <param name="data"> The data to be passed to target ViewModel. </param>
        /// <param name="viewModelType"> Target ViewModel type. </typeparam>
        /// <typeparam name="TData"> Type of data to be passed to target ViewModel. </typeparam>        
        Task NavigateAsync<TData>(Type viewModelType, TData data);

        /// <summary>
        /// Navigate to a given ViewModel.
        /// </summary>
        /// <param name="viewModelType"> Target ViewModel type. </typeparam>        
        Task NavigateAsync(Type viewModelType);

        /// <summary>
        /// Return control back to the previous ViewModel.
        /// </summary>
        Task GoBackAsync();

        /// <summary>
        /// Return control back to the previous ViewModel with the specified result data.
        /// </summary>
        /// <param name="result"> The result. </param>
        /// <typeparam name="TData"> Type of the data to pass to previous ViewModel. </typeparam>
        Task GoBackAsync<TData>(TData result);

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
