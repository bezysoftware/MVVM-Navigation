namespace Bezysoftware.Navigation.Sample.Views
{
    using Lookup;
    using ViewModels;
    using Windows.UI.Xaml.Controls;

    [AssociatedViewModel(typeof(DifferentViewModel))]
    public sealed partial class DifferentPage : Page
    {
        public DifferentPage()
        {
            this.InitializeComponent();
        }
    }
}
