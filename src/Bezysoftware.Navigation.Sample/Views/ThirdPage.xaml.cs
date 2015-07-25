namespace Bezysoftware.Navigation.Sample.Views
{
    using Bezysoftware.Navigation.Lookup;
    using Bezysoftware.Navigation.Platform;
    using Bezysoftware.Navigation.Sample.ViewModels;
    using Windows.UI.Xaml.Controls;


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [AdaptiveNavigationByWidth(MaxWidth = 1000)]
    [AssociatedViewModel(typeof(ThirdViewModel))]
    public sealed partial class ThirdPage : Page
    {
        public ThirdPage()
        {
            this.InitializeComponent();
        }
    }
}
