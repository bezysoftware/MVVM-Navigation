namespace Bezysoftware.Navigation.Lookup
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Assembly resolver which returns all assemblies distributed in current package.
    /// </summary>
    public class GlobalAssemblyResolver : IAssemblyResolver
    {
        private IEnumerable<Assembly> assemblies = null;

        /// <summary>
        /// Returns all assemblies in current package.
        /// </summary>
        public async Task<IEnumerable<Assembly>> GetAssembliesAsync()
        {
            return this.assemblies ?? (this.assemblies = (await this.GetAssemblyListAsync()).ToList());
        }

        private async Task<IEnumerable<Assembly>> GetAssemblyListAsync()
        {
            var result = new List<Assembly>();

            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var files = (await folder.GetFilesAsync())
                .Where(file => file.FileType == ".dll" || file.FileType == ".exe")
                .Select(file => new AssemblyName() { Name = file.Name.Substring(0, file.Name.Length - file.FileType.Length) });

            foreach (var file in files)
            {
                try
                {
                    result.Add(Assembly.Load(file));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return result;
        }
    }
}
