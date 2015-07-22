namespace Bezysoftware.Navigation.Lookup
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Extensions for <see cref="IViewLocator"/>
    /// </summary>
    public static class ViewLocatorExtensions
    {
        /// <summary>
        /// Finds a corresponding View for given ViewModel
        /// </summary>
        /// <typeparam name="TViewModel"> Type of ViewModel </typeparam>
        /// <returns> Type of corresponding View </returns>
        public static Task<Type> GetViewTypeAsync<TViewModel>(this IViewLocator locator)
        {
            return locator.GetViewTypeAsync(typeof(TViewModel));
        }

        /// <summary>
        /// Manually registers an association between a View and ViewModel
        /// </summary>        
        /// <typeparam name="TViewModel"> Type of ViewModel </typeparam>
        /// <typeparam name="TView"> Type of View </typeparam>
        /// <returns> The <see cref="IViewLocator"/>. </returns>
        public static IViewLocator RegisterAssociation<TViewModel, TView>(this IViewLocator locator)
        {
            return locator.RegisterAssociation(typeof(TViewModel), typeof(TView));
        }
    }
}
