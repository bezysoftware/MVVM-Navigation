namespace Bezysoftware.Navigation.Sample.ViewModels
{
    using Bezysoftware.Navigation.Activation;
    using Bezysoftware.Navigation.Sample.Dto;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.ObjectModel;

    public class MainViewModel : ViewModelBase, IActivate
    {
        private readonly IStore store;
        private readonly INavigationService nav;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IStore store, INavigationService nav)
        {
            this.store = store;
            this.nav = nav;

            this.Groups = new ObservableCollection<Group>();
        }

        public ObservableCollection<Group> Groups
        {
            get;
            private set;
        }

        public void Activate(NavigationType navigationType)
        {
            this.Groups.Clear();
            foreach (var group in this.store.GetGroups())
            {
                this.Groups.Add(group);
            }
        }

    }
}