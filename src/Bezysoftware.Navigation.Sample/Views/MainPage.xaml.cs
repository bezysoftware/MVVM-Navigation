namespace Bezysoftware.Navigation.Sample.Views
{
    using Bezysoftware.Navigation.Lookup;
    using Bezysoftware.Navigation.Sample.ViewModels;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [AssociatedViewModel(typeof(MainViewModel))]
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
