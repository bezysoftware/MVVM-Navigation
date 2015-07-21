namespace Bezysoftware.Navigation.StatePersistence
{
    /// <summary>
    /// The ViewModel marked with this interface will automatically have its state persisted and restored upon app deactivation.
    /// </summary>
    /// <remarks>
    /// The state should only consist of things that user manually entered in the UI. It should not contain any data recievied via Activate method (if it exists),
    /// because the NavigationManager will call the Activate method separately before restoring this state. 
    /// </remarks>
    /// <typeparam name="TState"> Type of state. </typeparam>
    public interface IStatefulViewModel<TState>
    {
        /// <summary>
        /// Gets the current state of the ViewModel
        /// </summary>
        TState State { get; }

        /// <summary>
        /// Restores the state. 
        /// </summary>
        /// <param name="state"> The state. </param>
        void RestoreState(TState state);
    }
}
