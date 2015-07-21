namespace Bezysoftware.Navigation.StatePersistence
{
    using System;

    /// <summary>
    /// Attribute which can be used to specify how the given ViewModel should be persisted in case of tombstoning event.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class StatePersistenceBehaviorAttribute : Attribute
    {
        public StatePersistenceBehaviorAttribute(StatePersistenceBehaviorType statePersistenceBehaviorType)
        {
            this.StatePersistenceBehaviorType = statePersistenceBehaviorType;
        }

        public StatePersistenceBehaviorType StatePersistenceBehaviorType { get; private set; }
    }
}
