namespace Bezysoftware.Navigation.Lookup
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides functionality to list assemblies which can be searched. 
    /// </summary>
    public interface IAssemblyResolver
    {
        /// <summary>
        /// Returns a list of assemblies to use when looking up associations between View and ViewModel.
        /// </summary>
        /// <returns> A collection of assemblies. </returns>
        Task<IEnumerable<Assembly>> GetAssembliesAsync();
    }
}
