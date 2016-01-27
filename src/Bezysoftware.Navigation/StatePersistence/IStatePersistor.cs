namespace Bezysoftware.Navigation.StatePersistence
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The state persistor is responsible for saving the current state of viewmodels.
    /// </summary>
    public interface IStatePersistor
    {
        /// <summary>
        /// Gets or sets the navigation path.
        /// </summary>
        string NavigationPath { get; set; }

        /// <summary>
        /// Pushes the given state into storage. 
        /// </summary>
        /// <param name="viewModel"> The ViewModel that the state belongs to. </param>
        /// <param name="activationData"> Data the ViewModel was activated with. </param>
        /// <param name="navigationState"> The state of platform navigation. </param>
        Task PushStateAsync(object viewModel, object activationData, string navigationState);

        /// <summary>
        /// Pops the last available state from storage.
        /// </summary>
        /// <returns> The state. </returns>
        Task<State> PopStateAsync();

        /// <summary>
        /// Gets state for all ViewModels which are persisted.
        /// </summary>
        /// <returns>Collection of states, ordered like a stack. </returns>
        Task<List<State>> GetAllStatesAsync();
        
        /// <summary>
        /// Sets state for the given ViewModel, if it implements <see cref="IStatefulViewModel{TState}"/>.
        /// </summary>
        /// <param name="viewModel">The ViewModel.</param>
        /// <param name="state">The state.</param>
        void SetViewModelState(object viewModel, object state);

        /// <summary>
        /// Persists current stack of states into storage.
        /// </summary>
        Task SaveAsync();

        /// <summary>
        /// Clear all states.
        /// </summary>
        void ClearAllStates();

        /// <summary>
        /// Checks whether state is persisted.
        /// </summary>
        Task<bool> ContainsStateAsync();
    }
}
