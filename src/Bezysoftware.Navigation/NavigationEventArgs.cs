namespace Bezysoftware.Navigation
{
    using System;

    public class NavigationEventArgs : EventArgs
    {
        public readonly object ActivationData;
        public readonly NavigationType NavigationType;
        public readonly Type ViewModelType;
        public readonly Type ViewType;

        public NavigationEventArgs(NavigationType navigationType, Type viewModelType, Type viewType, object activationData)
        {
            this.NavigationType = navigationType;
            this.ViewModelType = viewModelType;
            this.ViewType = viewType;
            this.ActivationData = activationData;
        }
    }
}
