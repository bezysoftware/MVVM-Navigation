namespace Bezysoftware.Navigation
{
    using System;

    public class NavigationEventArgs : EventArgs
    {
        private readonly object ActivationData;
        private readonly NavigationType NavigationType;
        private readonly Type ViewModelType;
        private readonly Type ViewType;

        public NavigationEventArgs(NavigationType navigationType, Type viewModelType, Type viewType, object activationData)
        {
            this.NavigationType = navigationType;
            this.ViewModelType = viewModelType;
            this.ViewType = viewType;
            this.ActivationData = activationData;
        }
    }
}
