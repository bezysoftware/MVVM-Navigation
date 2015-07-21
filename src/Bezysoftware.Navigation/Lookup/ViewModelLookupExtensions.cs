namespace Bezysoftware.Navigation.Lookup
{
    using System.Threading.Tasks;

    /// <summary>
    /// Extensions for ViewModel lookup
    /// </summary>
    public static class ViewModelLookupExtensions
    {
        /// <summary>
        /// Finds and instance of the given ViewModel
        /// </summary>
        /// <typeparam name="TViewModel"> Type of ViewModel</typeparam>
        /// <param name="locator"> The locator </param>
        /// <returns> ViewModel instance </returns>
        public static async Task<TViewModel> GetInstanceAsync<TViewModel>(this IViewModelLocator locator)
        {
            return (TViewModel)(await locator.GetInstanceAsync(typeof(TViewModel)));
        }
    }
}
