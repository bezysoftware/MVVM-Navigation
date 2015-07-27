namespace Bezysoftware.Navigation.Activation
{
    using System.Threading.Tasks;

    /// <summary>
    /// Marks a ViewModel which can prevent being deactivate and therefor prevent navigation.
    /// </summary>
    public interface IDeactivateQuery
    {
        /// <summary>
        /// Checks if deactivation can happen.
        /// </summary>
        /// <param name="navigationType"> Type of navigation, forward or backward. </param>
        /// <returns> Whether deactivation can happen. </returns>
        Task<bool> CanDeactivateAsync(NavigationType navigationType, DeactivationParameters parameters);
    }
}
