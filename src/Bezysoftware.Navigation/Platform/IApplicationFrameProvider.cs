using Windows.UI.Xaml.Controls;

namespace Bezysoftware.Navigation.Platform
{
    public interface IApplicationFrameProvider
    {
        /// <summary>
        /// Returns current application frame.
        /// </summary>
        /// <returns> Application frame. </returns>
        Frame GetCurrentFrame();
    }
}
