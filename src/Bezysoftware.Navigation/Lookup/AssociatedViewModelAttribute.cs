namespace Bezysoftware.Navigation.Lookup
{
    using System;

    /// <summary>
    /// Links the View to a specific ViewModel. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AssociatedViewModelAttribute : Attribute
    {
        public readonly Type ViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssociatedViewModelAttribute"/> class.
        /// </summary>
        /// <param name="viewModel">
        /// The view model.
        /// </param>
        public AssociatedViewModelAttribute(Type viewModel)
        {
            this.ViewModel = viewModel;
        }
    }
}
