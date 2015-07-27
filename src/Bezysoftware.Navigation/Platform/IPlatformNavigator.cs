namespace Bezysoftware.Navigation.Platform
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    
    /// <summary>
    /// Interface for platform dependent page navigation.
    /// </summary>
    public interface IPlatformNavigator
    {
        /// <summary>
        /// Raised when the system requests back navigation (e.g. back button was pressed on a phone).
        /// </summary>
        event EventHandler<CancelEventArgs> BackNavigationRequested;

        /// <summary>
        /// Collection of page types in the back stack including the current one.
        /// </summary>
        IEnumerable<Type> BackStackPageTypes { get; }

        /// <summary>
        /// Checks whether navigator can go back.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// The go back.
        /// </summary>
        /// <param name="currentViewModelType">Type of current ViewModel</param>
        /// <param name="associatedViewType"> Type of View associated to current ViewModel type. /></param>
        void GoBack(Type currentViewModelType, Type associatedViewType);

        /// <summary> 
        /// The navigate.
        /// </summary>
        /// <param name="viewType"> Type of View to navigate to. </param>
        void Navigate(Type viewType);

        /// <summary>
        /// Get current state of navigation serialized as a string
        /// </summary>
        /// <returns> Serialized navigation state. </returns>
        string GetNavigationState();

        /// <summary>
        /// Unhook intercepted type
        /// </summary>
        /// <param name="viewType"> Type of view. </param>
        void UnhookType(Type viewType);

        /// <summary>
        /// Restores the navigation state.
        /// </summary>
        /// <param name="state"> Serialized navigation state. </param>
        /// <param name="allViewTypes"> Types of all views. Not all of them need to be in the seralized navigation state due to navigation interception. </param>
        void RestoreNavigationState(string state, IEnumerable<Type> allViewTypes);
    }
}
