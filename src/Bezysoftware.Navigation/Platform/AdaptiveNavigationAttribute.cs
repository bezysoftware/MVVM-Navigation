namespace Bezysoftware.Navigation.Platform
{
    using System;

    /// <summary>
    /// Base class attribute for specifying whether navigation to target page should occur.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use this to implement adaptive behaviors for navigation - typical example would be having an implementation which 
    /// checks available app width / height and depending on that do or don't perform navigation to target page.
    /// </para>
    /// <para>
    /// Use this attribute's descendants to decorate target pages.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public abstract class AdaptiveNavigationAttribute : Attribute, IDisposable
    {
        protected Type ViewType;

        public event EventHandler<TypeEventArgs> ConditionChanged;

        public virtual void Dispose()
        {
        }

        public virtual void Initilize(Type viewType)
        {
            this.ViewType = viewType;
        }


        /// <summary>
        /// Determines whether platform navigation should happen to target view
        /// </summary>
        /// <returns> Whether navigation should occur. </returns>
        public abstract bool ShouldNavigate();

        /// <summary>
        /// Raises <see cref="ConditionChanged"/> event.
        /// </summary>
        protected void OnConditionChanged()
        {
            this.ConditionChanged?.Invoke(this, new TypeEventArgs(this.ViewType));
        }
    }
}
