# State persistence

You have multiple options when it comes to application state persistence. By default, when you navigate to target ViewModel, this action is pushed onto a stack, along with the data you use to activate it (if any). When you go back, last item from the stack is popped (including when you navigate **back** with data - this data is **not persisted**)

When your app is being suspended, you need to call `PersistApplicationState()` and when it is being restored `RestoreApplicationState()`. When the state is restored, all the ViewModels in the navigation stack are activated in order, passing activation data (if any).

## Per ViewModel tuning

You may not always want this automatic behavior. For example, one of your ViewModels queries a remote service when activated, which can slow down application start up when restoring state. For these specific cases, you have several options:

### StatePersistenceBehaviorAttribute

Decorate a ViewModel with this attribute to specify what behavior should be used when persisting given ViewModel's state. There are three options:

 * `ActivationAndState` - Default. Persists the activation & custom state (see below)
 * `StateOnly` - Persists only custom state (see below)
 * `None` - Nothing is persisted
 
### Custom ViewModel state

Another option you can take advantage of is to have your ViewModel implement `IStateAwareViewModel<TState>` interface. This way you can specify custom object to represent your ViewModel's state.

For example, this can be useful when the user has some input option on given ViewModel (e.g. type his address on a delivery form etc.) and you want to persist what the user has typed so far. Or when you ViewModel gets activated it fetches some data from a service, you might want to persist this data and set the `PersistenceBehaviorType` to `StateOnly` to prevent activation when restoring.

## A note about nulls

Do not pass null values when navigating to ViewModels, e.g. `navService.Navigate<MainViewModel, SomeData>(null)` will break state persistence. Same applies to custom ViewModel state - if you implement `IStateAwareViewModel<TState>`, make sure you don't return null from the underlying method.
