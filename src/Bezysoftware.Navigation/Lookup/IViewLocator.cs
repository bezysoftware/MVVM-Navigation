namespace Bezysoftware.Navigation.Lookup
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Locator capable of looking up the corresponding View to given ViewModel
    /// </summary>
    public interface IViewLocator
    {
        /// <summary>
        /// Finds a corresponding View for given ViewModel
        /// </summary>
        /// <param name="viewModelType"> Type of ViewModel </param>
        /// <returns> Type of corresponding View </returns>
        Task<Type> GetViewTypeAsync(Type viewModelType);

        /// <summary>
        /// Manually registers an association between a View and ViewModel
        /// </summary>        
        /// <param name="viewModelType"> Type of ViewModel </param>
        /// <param name="viewType"> Type of View </param>
        /// <returns> The <see cref="IViewLocator"/>. </returns>
        IViewLocator RegisterAssociation(Type viewModelType, Type viewType);
    }
}
