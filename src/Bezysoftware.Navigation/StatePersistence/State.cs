namespace Bezysoftware.Navigation.StatePersistence
{
    using System;

    /// <summary>
    /// The state of active view and viewmodel.
    /// </summary>
    public class State
    {
        /// <summary>
        /// Activation data passed to the associated viewmodel.
        /// </summary>
        public object ActivationData { get; set; }

        /// <summary>
        /// State of associated viewmodel. It is <see cref="Lazy{T}"/> to allow the <see cref="StatePersistor"/> to get it only when it needs to persist the world. It cannot get it at time of navigation because it can change while user is interacting with it.
        /// </summary>
        public Lazy<object> ViewModelState { get; set; }

        /// <summary>
        /// Type of ViewModel this state belongs to.
        /// </summary>
        public Type ViewModelType { get; set; }
    }
}