﻿namespace Bezysoftware.Navigation.Sample.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using GalaSoft.MvvmLight;
    using Bezysoftware.Navigation.Sample.Dto;
    using System.Collections.ObjectModel;
    using GalaSoft.MvvmLight.Command;

    public class SecondViewModel : ViewModelBase, IActivate<Group>, IDeactivate
    {
        private readonly INavigationService nav;
        private Group data;

        public SecondViewModel(INavigationService nav)
        {
            this.nav = nav;

            this.Items = new ObservableCollection<Item>();
            this.NavigateCommand = new RelayCommand<Item>(g => this.nav.Navigate<ThirdViewModel, Item>(g));
        }

        public RelayCommand<Item> NavigateCommand
        {
            get;
            private set;
        }

        public ObservableCollection<Item> Items
        {
            get;
            private set;
        }

        public void Activate(NavigationType navigationType, Group data)
        {
            foreach (var subGroup in data.Items)
            {
                this.Items.Add(subGroup);
            }
        }

        public async Task DeactivateAsync(NavigationType navigationType)
        {
            if (navigationType == NavigationType.Backward)
            {
                this.Items.Clear();
            }
        }
    }
}