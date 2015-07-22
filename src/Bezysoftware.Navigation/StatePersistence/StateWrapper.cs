namespace Bezysoftware.Navigation.StatePersistence
{
    /// <summary>
    /// State wrapper used to persist current application state in isolated storage, deserialized to json.
    /// </summary>
    internal class StateWrapper
    {
        /// <summary>
        /// Activation data passed to the active viewmodel.
        /// </summary>
        public string ActivationData { get; set; }

        /// <summary>
        /// Type of <see cref="ActivationData"/>.
        /// </summary>
        public string ActivationDataType { get; set; }

        /// <summary>
        /// State of active viewmodel.
        /// </summary>
        public string ViewModelState { get; set; }

        /// <summary>
        /// Type of <see cref="ViewModelState"/>. 
        /// </summary>
        public string ViewModelStateType { get; set; }

        /// <summary>
        /// Type of associated ViewModel.
        /// </summary>
        public string ViewModelType { get; set; }
    }
}
