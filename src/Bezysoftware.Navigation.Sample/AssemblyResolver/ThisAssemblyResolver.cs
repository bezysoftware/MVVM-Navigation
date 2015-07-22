namespace Bezysoftware.Navigation.Sample.AssemblyResolver
{
    using Bezysoftware.Navigation.Lookup;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    public class ThisAssemblyResolver : IAssemblyResolver
    {
        public async Task<IEnumerable<Assembly>> GetAssembliesAsync()
        {
            return new[] { typeof(ThisAssemblyResolver).GetTypeInfo().Assembly };
        }
    }
}
