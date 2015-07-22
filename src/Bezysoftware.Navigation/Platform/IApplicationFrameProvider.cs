using Windows.UI.Xaml.Controls;

namespace Bezysoftware.Navigation.Platform
{
    /// <summary>
    /// Interface for providing current application <see cref="Frame"/>.
    /// </summary>
    public interface IApplicationFrameProvider
    {
        /// <summary>
        /// Returns current application frame.
        /// </summary>
        /// <returns> Application frame. </returns>
        Frame GetCurrentFrame();
    }
}
