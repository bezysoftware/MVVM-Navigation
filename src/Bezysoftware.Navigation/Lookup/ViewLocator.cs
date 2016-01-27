namespace Bezysoftware.Navigation.Lookup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The default view locator which inspects all loaded assemblies.
    /// </summary>
    public class ViewLocator : IViewLocator
    {
        private readonly Dictionary<Type, Type> lookupCache;
        private readonly IAssemblyResolver assemblyResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLocator"/> class.
        /// </summary>
        public ViewLocator(IAssemblyResolver assemblyResolver)
        {
            this.assemblyResolver = assemblyResolver;

            this.lookupCache = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// The get view uri.
        /// </summary>
        /// <param name="viewModelType"> Type of ViewModel </param>
        /// <returns> The <see cref="Uri"/> pointing to specific View </returns>
        public async virtual Task<Type> GetViewTypeAsync(Type viewModelType)
        {
            if (!this.lookupCache.ContainsKey(viewModelType))
            {
                var assemblies = await this.assemblyResolver.GetAssembliesAsync();
                var pairs = ReflectionUtils.GetTypesWithAttribute<AssociatedViewModelAttribute>(assemblies);
                var t = pairs.FirstOrDefault(p => p.Value.ViewModel == viewModelType);
                this.lookupCache.Add(viewModelType, t.Key);
            }

            return this.lookupCache[viewModelType];
        }

        /// <summary>
        /// Manually registers an association between a View and ViewModel
        /// </summary>        
        /// <param name="viewModelType"> Type of ViewModel </param>
        /// <param name="viewType"> Type of View </param>
        /// <returns> The <see cref="IViewLocator"/>. </returns>
        public IViewLocator RegisterAssociation(Type viewModelType, Type viewType)
        {
            this.lookupCache.Add(viewModelType, viewType);
            return this;
        }
    }
}
