namespace Bezysoftware.Navigation.StatePersistence
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Windows.Storage;

    /// <summary>
    /// The state persistor is responsible for saving the current state of viewmodels.
    /// </summary>
    public class StatePersistor : IStatePersistor
    {
        private const string PersistedStateKey = "Bezysoftware_Navigation_State";
        private const string NavigationPathKey = "Bezysoftware_Navigation_Path";

        private List<State> stateCache;

        public StatePersistor()
        {
            this.stateCache = new List<State>();
            this.NavigationPath = ApplicationData.Current.LocalSettings.Values.ContainsKey(NavigationPathKey)
                ? ApplicationData.Current.LocalSettings.Values[NavigationPathKey].ToString()
                : string.Empty;
        }

        /// <summary>
        /// Gets or sets the navigation path.
        /// </summary>
        public string NavigationPath
        {
            get;
            set;
        }

        /// <summary>
        /// Checks whether state is persisted.
        /// </summary>
        public Task<bool> ContainsStateAsync()
        {
            return Task.FromResult(ApplicationData.Current.LocalSettings.Values.ContainsKey(PersistedStateKey));
        }

        /// <summary>
        /// Gets state for all ViewModels which are persisted.
        /// </summary>
        /// <returns>Collection of states, ordered like a stack. </returns>
        public async Task<List<State>> GetAllStatesAsync()
        {
            if (!this.stateCache.Any())
            {
                this.stateCache = await this.GetStatesAsync();
            }

            return this.stateCache;
        }

        /// <summary>
        /// Persists current stack of states into storage.
        /// </summary>
        public async Task SaveAsync()
        {
            await this.SetStatesAsync(await this.GetAllStatesAsync());
            ApplicationData.Current.LocalSettings.Values[NavigationPathKey] = this.NavigationPath;
        }

        /// <summary>
        /// Pops the last available state from storage.
        /// </summary>
        /// <returns> The state. </returns>
        public async Task<State> PopStateAsync()
        {
            // get top
            var states = await this.GetAllStatesAsync();
            var top = states.Last();

            // remove top 
            states.Remove(top);

            return top;
        }

        /// <summary>
        /// Pushes the given state into storage. 
        /// </summary>
        /// <param name="viewModel"> The ViewModel that the state belongs to. </param>
        /// <param name="activationData"> Data the ViewModel was activated with. </param>
        /// <param name="navigationState"> The state of platform navigation. </param>
        public async Task PushStateAsync(object viewModel, object activationData, string navigationState)
        {
            var states = await this.GetAllStatesAsync();
            var attribute = viewModel.GetType().GetTypeInfo().GetCustomAttribute<StatePersistenceBehaviorAttribute>();
            var vmType = viewModel.GetType();

            if (attribute != null)
            {
                switch (attribute.StatePersistenceBehaviorType)
                {
                    case StatePersistenceBehaviorType.ActivationAndState:
                        break;
                    case StatePersistenceBehaviorType.StateOnly:
                        activationData = null;
                        break;
                    case StatePersistenceBehaviorType.None:
                        activationData = null;
                        viewModel = null;
                        break;
                }
            }

            var state = new State
            {
                ActivationData = activationData,
                ViewModelState = this.GetViewModelStateLazy(viewModel),
                ViewModelType = vmType
            };

            states.Add(state);
        }

        public void SetViewModelState(object viewModel, object state)
        {
            if (this.IsViewModelStateful(viewModel) && state != null)
            {
                var method = viewModel.GetType().GetRuntimeMethod("RestoreState", new[] { state.GetType() });
                method.Invoke(viewModel, new[] { state });
            }
        }

        public void ClearAllStates()
        {
            var values = ApplicationData.Current.LocalSettings.Values;
            if (values.ContainsKey(PersistedStateKey))
            {
                values.Remove(PersistedStateKey);
            }
        }

        private Lazy<object> GetViewModelStateLazy(object viewModel)
        {
            return this.IsViewModelStateful(viewModel)
                ? new Lazy<object>(() => viewModel.GetType().GetRuntimeProperty("State").GetValue(viewModel))
                : new Lazy<object>(() => null);
        }

        private StateWrapper SerializeState(State state)
        {
            var vmState = state.ViewModelState.Value;

            return new StateWrapper
            {
                ActivationData = JsonConvert.SerializeObject(state.ActivationData),
                ActivationDataType = state.ActivationData?.GetType().AssemblyQualifiedName ?? string.Empty,
                ViewModelState = JsonConvert.SerializeObject(vmState),
                ViewModelStateType = vmState?.GetType().AssemblyQualifiedName ?? string.Empty,
                ViewModelType = state.ViewModelType?.AssemblyQualifiedName ?? string.Empty
            };
        }

        private State DeserializeState(StateWrapper wrapper)
        {
            var vmState = string.IsNullOrWhiteSpace(wrapper.ViewModelStateType) ? null : JsonConvert.DeserializeObject(wrapper.ViewModelState, Type.GetType(wrapper.ViewModelStateType));

            return new State
            {
                ActivationData = string.IsNullOrWhiteSpace(wrapper.ActivationDataType) ? null : JsonConvert.DeserializeObject(wrapper.ActivationData, Type.GetType(wrapper.ActivationDataType)),
                ViewModelState = new Lazy<object>(() => vmState),
                ViewModelType = string.IsNullOrWhiteSpace(wrapper.ViewModelType) ? null : Type.GetType(wrapper.ViewModelType),
            };
        }

        private async Task<List<State>> GetStatesAsync()
        {
            var values = ApplicationData.Current.LocalSettings.Values;

            if (!values.ContainsKey(PersistedStateKey))
            {
                return new List<State>();
            }

            var collection = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<StateWrapper>>(values[PersistedStateKey].ToString()).Select(this.DeserializeState));

            return collection.ToList();
        }

        private async Task SetStatesAsync(IEnumerable<State> states)
        {
            var values = ApplicationData.Current.LocalSettings.Values;
            values[PersistedStateKey] = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(states.Select(this.SerializeState).ToList()));
        }

        private bool IsViewModelStateful(object viewModel)
        {
            var hasState = viewModel?.GetType().GetTypeInfo().ImplementedInterfaces.Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IStatefulViewModel<>));
            return hasState.HasValue && hasState.Value;
        }
    }
}
