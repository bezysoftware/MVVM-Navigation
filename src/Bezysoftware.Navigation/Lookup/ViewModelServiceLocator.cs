namespace Bezysoftware.Navigation.Lookup
{
    using Microsoft.Practices.ServiceLocation;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// ViewModelInstanceLocator which uses ServiceLocator
    /// </summary>
    public class ViewModelServiceLocator : IViewModelLocator
    {
        /// <summary>
        /// Uses service locator to find a ViewModel instance
        /// </summary>
        /// <typeparam name="TViewModel"> Type of ViewModel </typeparam>
        /// <returns> The <see cref="TViewModel"/>. </returns>
        public Task<object> GetInstanceAsync(Type viewModelType)
        {
            return Task.FromResult(ServiceLocator.Current.GetInstance(viewModelType));
        }

        public Type GetType(Type viewType)
        {
            return viewType.GetTypeInfo().GetCustomAttribute<AssociatedViewModelAttribute>().ViewModel;
        }
    }
}
