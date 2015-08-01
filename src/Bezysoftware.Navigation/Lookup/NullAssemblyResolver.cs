namespace Bezysoftware.Navigation.Lookup
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Resolver which doesn't return any assemblies.
    /// </summary>
    public class NullAssemblyResolver : IAssemblyResolver
    {
        /// <summary>
        /// Returns empty collection.
        /// </summary>
        /// <remarks>
        /// This can be used when the <see cref="IViewLocator"/> is used only with explicit 
        /// </remarks>
        public Task<IEnumerable<Assembly>> GetAssembliesAsync()
        {
            return Task.FromResult(Enumerable.Empty<Assembly>());
        }
    }
}
