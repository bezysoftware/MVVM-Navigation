namespace Bezysoftware.Navigation.StatePersistence
{
    /// <summary>
    /// The ViewModel marked with this interface will automatically have its custom state persisted and restored upon app (de)activation.
    /// </summary>
    /// <remarks>
    /// The state should only consist of things that user manually entered in the UI. It should not contain any data recievied via Activate method (if it exists),
    /// because the NavigationManager will call the Activate method separately before restoring this state. The exception to this is when the Activate method performs a long running
    /// operation which would slow down state restoration. In this case, mark the viewmodel with <see cref="StatePersistenceBehaviorAttribute"/> set to StateOnly and implement this interface.
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
