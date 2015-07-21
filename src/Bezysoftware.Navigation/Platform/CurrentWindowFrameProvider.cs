namespace Bezysoftware.Navigation.Platform
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Gets a <see cref="Frame"/> from Window.Current.Content. It also supports <see cref="SplitView"/> which wraps the frame one level deeper.
    /// </summary>
    public class CurrentWindowFrameProvider : IApplicationFrameProvider
    {
        /// <summary>
        /// Inspects Window.Curent.Content to get current <see cref="Frame"/>
        /// </summary>
        /// <returns> <see cref="Frame"/> or null. </returns>
        public Frame GetCurrentFrame()
        {
            return (Window.Current.Content as Frame) ?? (Window.Current.Content as SplitView)?.Content as Frame;
        }
    }
}
