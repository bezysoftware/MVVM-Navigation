namespace Bezysoftware.Navigation.Platform
{
    using System;
    using Windows.UI.Xaml;

    /// <summary>
    /// Attribute for specifying what the minimum width of the app is to allow navigation to target page occur.
    /// </summary>
    /// <remarks>
    /// Use this attribute to decorate target pages.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class AdaptiveNavigationByWidthAttribute : AdaptiveNavigationAttribute
    {
        public AdaptiveNavigationByWidthAttribute()
        {
            this.MinWidth = double.NegativeInfinity;
            this.MaxWidth = double.PositiveInfinity;

            Window.Current.SizeChanged += this.WindowSizeChanged;
        }

        /// <summary>
        /// Gets or sets what the min width of the window should be to allow navigation
        /// </summary>
        public double MinWidth { get; set; }

        /// <summary>
        /// Gets or sets what the max width of the window should be to allow navigation
        /// </summary>
        public double MaxWidth { get; set; }

        public override bool ShouldNavigate()
        {
            var width = Window.Current.Bounds.Width;

            if (this.MaxWidth <= width)
            {
                return false;
            }

            if (this.MinWidth >= width)
            {
                return false;
            }

            return true;
        }

        public override void Dispose()
        {
            base.Dispose();
            Window.Current.SizeChanged -= this.WindowSizeChanged;
        }

        private void WindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.OnConditionChanged();
        }
    }
}
