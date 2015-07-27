namespace Bezysoftware.Navigation
{
    using System.Threading.Tasks;

    /// <summary>
    /// Denotes a ViewModel class which can be deactivated when navigated from.
    /// </summary>
    public interface IDeactivate
    {
        /// <summary>
        /// This method is called when implementing class instance is navigated from.
        /// </summary>
        /// <param name="navigationType"> Type of navigation occurring. </param>
        Task DeactivateAsync(NavigationType navigationType, DeactivationParameters parameters);
    }
}
