namespace Bezysoftware.Navigation.Activation
{
    using System;

    /// <summary>
    /// Container holding information about current deactivation
    /// </summary>
    public class DeactivationParameters
    {
        internal DeactivationParameters(Type targetViewModelType, object deactivationData)
        {
            this.TargetViewModelType = targetViewModelType;
            this.DeactivationData = deactivationData;
        }

        /// <summary>
        /// Target ViewModel which will be activated if current deactivation takes place.
        /// </summary>
        public Type TargetViewModelType
        {
            get;
            private set;
        }

        /// <summary>
        /// Data which will be passed to target ViewModel if current deactivation takes place. This data can be overriden.
        /// </summary>
        public object DeactivationData
        {
            get;
            set;
        }
    }
}
