# Viewmodel activation and deactivation

Your viewmodels can be activated (when being navigated to) or deactivated (when being navigated from). There are 4 interfaces that your viewmodels can implement to achieve this:

```
public interface IActivate
{
  void Activate(NavigationType navigationType);
}

public interface IActivate<TData>
{
  void Activate(NavigationType navigationType, TData data);
}

public interface IDeactivate
{
  Task DeactivateAsync(NavigationType navigationType);
}

public interface IDeactivateQuery
{
  Task<bool> CanDeactivateAsync(NavigationType navigationType);
}
```

Notice that all interfaces have a parameter of type `NavigationType` which can be `Forward` or `Backward`.

The methods `IActivate` interfaces are invoked when the given viewmodel is being navigated to - the second one allows the caller to pass data to it. 

The `IDeactivate` interface allows the viewmodel to deactivate when being navigated from. Notice its method returns a `Task` - the navigation service waits for completion of this method before proceeding with navigation. This allows you to perform some long running async tasks (such as back up data to cloud etc.)

And lastly, the `IDeactivateQuery` allows you to prevent navigation from happening. Its method returns a `Task<bool>` and the navigation service awaits its result. This interface can be useful for example on the phone, when user clicks the back button to go back, but has some unsaved data on given page. You can show a dialog asking him if he wants to save or discard it.

Note that the `IActivate` methods do not return `Task`. That does not prevent you from using `async` & `await` in them, but if you do, the navigation service will not wait for their completion and will complete navigation right away.
