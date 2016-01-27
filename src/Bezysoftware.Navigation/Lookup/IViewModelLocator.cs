namespace Bezysoftware.Navigation.Lookup
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for looking up ViewModels.
    /// </summary>
    public interface IViewModelLocator
    {
        /// <summary>
        /// Finds and instance of the given ViewModel
        /// </summary>
        /// <param name="viewModelType"> Type of ViewModel </param>
        /// <returns> The ViewModel instance </returns>
        Task<object> GetInstanceAsync(Type viewModelType);

        /// <summary>
        /// Gets the type of ViewModel associated with the given viewType
        /// </summary>
        /// <param name="viewType"></param>
        /// <returns> Associated ViewModel type to given View type. </returns>
        Type GetAssociatedViewModelType(Type viewType);
    }
}
