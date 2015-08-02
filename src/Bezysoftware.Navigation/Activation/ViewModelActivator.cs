namespace Bezysoftware.Navigation.Activation
{
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Linq;

    internal static class ViewModelActivator
    {
        public static async Task<bool> CanDeactivateViewModelAsync(object target, NavigationType navigationType, DeactivationParameters parameters)
        {
            var query = target as IDeactivateQuery;
            if (query != null)
            {
                if (!await query.CanDeactivateAsync(navigationType, parameters))
                {
                    return false;
                }
            }

            return true;
        }

        public static async Task DeactivateViewModelAsync(object target, NavigationType navigationType, DeactivationParameters parameters)
        {
            var deactivate = target as IDeactivate;
            if (deactivate != null)
            {
                await deactivate.DeactivateAsync(navigationType, parameters);
            }
        }

        public static void ActivateViewModel(object target, NavigationType navigationType)
        {
            var instance = target as IActivate;
            if (instance != null)
            {
                instance.Activate(navigationType);
            }
        }

        public static void ActivateViewModel(object target, NavigationType navigationType, object data)
        {
            if (target.GetType().GetInterfaces().Where(i => i.GetTypeInfo().IsGenericType).Select(i => i.GetGenericTypeDefinition()).Any(i => i.Equals(typeof(IActivate<>))))
            {
                var method = target.GetType().GetRuntimeMethod("Activate", new[] { typeof(NavigationType), data.GetType() });

                if (method != null)
                {
                    method.Invoke(target, new[] { navigationType, data });
                }
            }
        }

    }
}
