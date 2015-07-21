namespace Bezysoftware.Navigation.Lookup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The default view locator which inspects all loaded assemblies.
    /// </summary>
    public class DefaultViewLocator : IViewLocator
    {
        private readonly Dictionary<Type, Type> LookupCache;
        private readonly IAssemblyResolver assemblyResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultViewLocator"/> class.
        /// </summary>
        public DefaultViewLocator(IAssemblyResolver assemblyResolver)
        {
            this.assemblyResolver = assemblyResolver;

            this.LookupCache = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// The get view uri.
        /// </summary>
        /// <param name="viewModelType"> Type of ViewModel </param>
        /// <returns> The <see cref="Uri"/> pointing to specific View </returns>
        public async virtual Task<Type> GetViewTypeAsync(Type viewModelType)
        {
            if (!this.LookupCache.ContainsKey(viewModelType))
            {
                var assemblies = await this.assemblyResolver.GetAssembliesAsync();
                var pairs = ReflectionUtils.GetTypesWithAttribute<AssociatedViewModelAttribute>(assemblies);
                var t = pairs.FirstOrDefault(p => p.Value.ViewModel == viewModelType);
                this.LookupCache.Add(viewModelType, t.Key);
            }

            return this.LookupCache[viewModelType];
        }

        /// <summary>
        /// Manually registers an association between a View and ViewModel
        /// </summary>        
        /// <param name="vieModelType"> Type of ViewModel </typeparam>
        /// <param name="viewType"> Type of View </typeparam>
        /// <returns> The <see cref="IViewLocator"/>. </returns>
        public IViewLocator RegisterAssociation(Type viewModelType, Type viewType)
        {
            this.LookupCache.Add(viewModelType, viewType);
            return this;
        }
    }
}
