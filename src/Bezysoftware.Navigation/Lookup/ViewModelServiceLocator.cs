namespace Bezysoftware.Navigation.Lookup
{
    using Microsoft.Practices.ServiceLocation;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="IViewModelLocator"/> which uses ServiceLocator
    /// </summary>
    public class ViewModelServiceLocator : IViewModelLocator
    {
        /// <summary>
        /// Uses service locator to find a ViewModel instance
        /// </summary>
        /// <param name="viewModelType"> Type of ViewModel </param>
        /// <returns> The ViewModel instance. </returns>
        public Task<object> GetInstanceAsync(Type viewModelType)
        {
            return Task.FromResult(ServiceLocator.Current.GetInstance(viewModelType));
        }

        public Type GetAssociatedViewModelType(Type viewType)
        {
            return viewType.GetTypeInfo().GetCustomAttribute<AssociatedViewModelAttribute>().ViewModel;
        }
    }
}
