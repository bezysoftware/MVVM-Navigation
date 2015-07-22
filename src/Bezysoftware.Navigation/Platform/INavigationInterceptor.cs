namespace Bezysoftware.Navigation.Platform
{
    using System;

    /// <summary>
    /// Interface for interception of platform navigation. Implementing class can prevent navigation based on some condition as well as notify when this condition changes to reevaluate it.
    /// </summary>
    public interface INavigationInterceptor
    {
        /// <summary>
        /// Raised when condition which controls the interception of navigation changes.
        /// </summary>
        event EventHandler<TypeEventArgs> ConditionChanged;

        /// <summary>
        /// Intercept navigation to new page
        /// </summary>
        /// <param name="targetViewType"> Type of page to navigate to. </param>
        /// <returns> True if the navigation was intercepted and should be cancelled. False if normal navigation should continue. </returns>
        bool InterceptNavigation(Type targetViewType);

        /// <summary>
        /// Start watching for changes in the underlying interception condition and raise <seealso cref="ConditionChanged"/> event when it happens.
        /// </summary>
        /// <param name="viewType"> Type of View to watch. </param>
        void HookType(Type viewType);

        /// <summary>
        /// Stop watching for changes in the underlying interception condition and stop raising <seealso cref="ConditionChanged"/> event.
        /// </summary>
        /// <param name="viewType"> Type of view to stopp watching. </param>
        void UnhookType(Type viewType);
    }
}
