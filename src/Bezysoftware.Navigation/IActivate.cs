namespace Bezysoftware.Navigation
{
    /// <summary>
    /// Denotes a ViewModel class which can be activated when navigated to.
    /// </summary>
    public interface IActivate
    {
        /// <summary>
        /// This method is called when implementing class instance is navigated to.
        /// </summary>
        /// <param name="navigationType"> Type of navigation, forward or backward. </param>
        void Activate(NavigationType navigationType);
    }

    /// <summary>
    /// Denotes a ViewModel class which can be activated with data when navigated to.
    /// </summary>
    /// <typeparam name="TData"> Type of data. </typeparam>
    public interface IActivate<in TData>
    {
        /// <summary>
        /// This method is called when implementing class instance is navigated to with specified data type.
        /// </summary>
        /// <param name="navigationType"> Type of navigation, forward or backward. </param>
        /// <param name="data"> The data. </param>
        void Activate(NavigationType navigationType, TData data);
    }    
}
