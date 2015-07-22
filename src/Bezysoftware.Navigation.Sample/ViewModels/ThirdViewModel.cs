namespace Bezysoftware.Navigation.Sample.ViewModels
{
    using Bezysoftware.Navigation.Sample.Dto;
    using Bezysoftware.Navigation.StatePersistence;

    using GalaSoft.MvvmLight;
    using System;
    using System.Threading.Tasks;

    using Windows.UI.Popups;    

    [StatePersistenceBehavior(StatePersistenceBehaviorType.StateOnly)]
    public class ThirdViewModel : ViewModelBase, IActivate<Item>, IDeactivate, IDeactivateQuery, IStatefulViewModel<Item>
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

        public Item State
        {
            get
            {
                return this.Item;
            }
        }

        public async Task DeactivateAsync(NavigationType navigationType)
        {
            this.IsWorking = true;

            // simulate sync with cloud
            await Task.Delay(3000);

            this.IsWorking = false;

            this.Item = null;
        }

        public async void Activate(NavigationType navigationType, Item data)
        {
            this.IsWorking = true;

            // simulate long running operation. This could be a sync with the cloud and it is the reason, why this viewmodel has StatePersistenceBehavior set to StateOnly
            // this means that during state restore, after app had been tombstoned, this method will not be called and instead the state will be restored only using the RestoreState method
            await Task.Delay(3000);
            this.Item = data;

            this.IsWorking = false;
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

        public void RestoreState(Item state)
        {
            state.Content += " restored";
            this.Item = state;
        }
    }
}
