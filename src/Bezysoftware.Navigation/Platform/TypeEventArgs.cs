namespace Bezysoftware.Navigation.Platform
{
    using System;

    /// <summary>
    /// EventArgs holding a type
    /// </summary>
    public class TypeEventArgs : EventArgs
    {
        public readonly Type Type;

        public TypeEventArgs(Type type)
        {
            this.Type = type;
        }
    }
}
