namespace Bezysoftware.Navigation.Activation
{
    /// <summary>
    /// Denotes a ViewModel class which can be activated when navigated to.
    /// </summary>
    public interface IActivating
    {
        /// <summary>
        /// This method is called before implementing class instance is navigated to. It is not called during state restoration.
        /// </summary>
        /// <param name="navigationType"> Type of navigation, forward or backward. </param>
        void Activating(NavigationType navigationType);
    }

    /// <summary>
    /// Denotes a ViewModel class which can be activated with data when navigated to.
    /// </summary>
    /// <typeparam name="TData"> Type of data. </typeparam>
    public interface IActivating<in TData>
    {
        /// <summary>
        /// This method is called before implementing class instance is navigated to with specified data type. It is not called during state restoration.
        /// </summary>
        /// <param name="navigationType"> Type of navigation, forward or backward. </param>
        /// <param name="data"> The data. </param>
        void Activating(NavigationType navigationType, TData data);
    }    
}
