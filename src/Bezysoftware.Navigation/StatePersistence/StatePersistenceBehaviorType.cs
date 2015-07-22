namespace Bezysoftware.Navigation.StatePersistence
{
    /// <summary>
    /// Specifies type of state persistence behavior.
    /// </summary>
    public enum StatePersistenceBehaviorType
    {
        /// <summary>
        /// Both activation and custom state (<see cref="IStatefulViewModel{TState}"/> needs to be implemented) are persisted. 
        /// </summary>
        ActivationAndState,

        /// <summary>
        /// Activation is ignored, only custom state (<see cref="IStatefulViewModel{TState}"/> needs to be implemented) is persisted. 
        /// </summary>
        StateOnly,

        /// <summary>
        /// Nothing is persisted. This basically means that the navigation framework should completely ignore the navigation action.
        /// </summary>
        None
    }
}
