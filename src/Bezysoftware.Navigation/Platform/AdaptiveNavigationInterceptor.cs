namespace Bezysoftware.Navigation.Platform
{
    using Bezysoftware.Navigation.Lookup;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Interceptor which looks for the <see cref="AdaptiveNavigationByWidthAttribute"/> attribute on target page
    /// </summary>
    public class AdaptiveNavigationInterceptor : INavigationInterceptor
    {
        private readonly Dictionary<Type, AdaptiveNavigationAttribute> subscribedTypes;

        public AdaptiveNavigationInterceptor()
        {
            this.subscribedTypes = new Dictionary<Type, AdaptiveNavigationAttribute>();
        }

        /// <summary>
        /// Raises events from <see cref="AdaptiveNavigationAttribute"/>
        /// </summary>
        public event EventHandler<TypeEventArgs> ConditionChanged;

        /// <summary>
        /// Intercept navigation to previous page
        /// </summary>
        /// <param name="targetViewType"> Type of page to navigate to. </param>
        /// <param name="targetViewModelType"> Type of associated viewModel</param>
        /// <returns> True if the navigation was intercepted and should be cancelled. False if normal navigation should continue. </returns>
        public bool InterceptNavigation(Type targetViewType)
        {
            return !this.ShouldNavigate(targetViewType);
        }

        /// <summary>
        /// Start watching for changes in the underlying interception condition and raise <seealso cref="ConditionChanged"/> event when it happens.
        /// </summary>
        /// <param name="viewType"> Type of View to watch. </param>
        public void HookType(Type viewType)
        {
            var attribute = this.GetNavigationAttribute(viewType);

            if (attribute != null && !this.subscribedTypes.ContainsKey(viewType))
            {
                attribute.Initilize(viewType);
                attribute.ConditionChanged += this.AttributeConditionChanged;
                this.subscribedTypes.Add(viewType, attribute);
            }
        }

        /// <summary>
        /// Stop watching for changes in the underlying interception condition and stop raising <seealso cref="ConditionChanged"/> event.
        /// </summary>
        /// <param name="viewType"> Type of view to stopp watching. </param>
        public void UnhookType(Type viewType)
        {
            if (this.subscribedTypes.ContainsKey(viewType))
            {
                var attribute = this.subscribedTypes[viewType];
                attribute.ConditionChanged -= this.AttributeConditionChanged;
                this.subscribedTypes.Remove(viewType);
            }
        }

        private bool ShouldNavigate(Type viewType)
        {
            return this.GetNavigationAttribute(viewType)?.ShouldNavigate() ?? true;
        }

        private void AttributeConditionChanged(object sender, TypeEventArgs e)
        {
            this.ConditionChanged?.Invoke(sender, e);
        }

        private AdaptiveNavigationAttribute GetNavigationAttribute(Type viewType)
        {
            if (!this.subscribedTypes.ContainsKey(viewType))
            {
                return viewType.GetTypeInfo().GetCustomAttribute<AdaptiveNavigationAttribute>();
            }

            return this.subscribedTypes[viewType];
        }
    }
}
