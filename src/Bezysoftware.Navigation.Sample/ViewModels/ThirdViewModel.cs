namespace Bezysoftware.Navigation.Sample.ViewModels
{
    using System;
    using Bezysoftware.Navigation.Sample.Dto;
    using GalaSoft.MvvmLight;
    using System.Threading.Tasks;
    using Windows.UI.Popups;

    public class ThirdViewModel : ViewModelBase, IActivate<Item>, IDeactivate, IDeactivateQuery
    {
        private Item item;
        private bool isWorking;

        public Item Item
        {
            get { return this.item; }
            private set { this.Set(() => Item, ref this.item, value); }
        }

        public bool IsWorking
        {
            get { return this.isWorking; }
            private set { this.Set(() => IsWorking, ref this.isWorking, value); }
        }

        public async Task DeactivateAsync(NavigationType navigationType)
        {
            this.IsWorking = true;

            // simulate sync with cloud
            await Task.Delay(3000);

            this.IsWorking = false;

            this.Item = null;
        }

        public void Activate(NavigationType navigationType, Item data)
        {
            this.Item = data;
        }

        public async Task<bool> CanDeactivateAsync(NavigationType navigationType)
        {
            // Do NOT normally do this in you MMVM application, using dialogs and other View-related things breaks MVVM
            // This is here only for simplicity
            var dlg = new MessageDialog(Item.Content + " is trying to deactivate, allow?");
            dlg.Commands.Add(new UICommand("Yes"));
            dlg.Commands.Add(new UICommand("No"));

            return (await dlg.ShowAsync()).Label == "Yes";
        }


    }
}
