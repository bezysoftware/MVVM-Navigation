# Universal-Navigation
Navigation framework to allow true MVVM-like navigation in Universal Windows Apps.

## UWP API
The existing API allows to perform navigation in the following way:

```
frame.Navigate(typeof(NewPage));
```

optionally passing in simple parameters:

```
frame.Navigate(typeof(NewPage), userId);
```

In you NewPage codebehind you then need to override OnNavigatedTo, get the navigation parameter and pass it to your ViewModel.

Not only is it a lot of repetitve work, but if you want to perform navigation in you ViewModel, this effectively breaks your separation of concerns and the MVVM pattern. 
You also have to manually take care of persisting and restoring your state to support tombstoning of your app. 

## MVVM API

Wouldn't it be cool if you could write something like this:

```
navigationService.Navigate<NewViewModel>();
```

optionally passing even complex parameters:

```
navigationService.Navigate<NewViewModel>(new User { Id = 42 });
```

and having your latest state automatically restored?

### INavigationService

The NavigationService is an implementation of INavigationService, which has 4 methods for navigation and 2 for state persistance. Those are discussed later in this document.

```
public interface INavigationService 
{
  void Navigate<TViewModel>();
  void Navigate<TViewModel, TData>(TData data) where TViewModel: IIn<TData>;
  void GoBack();
  void GoBack<TData>(TData data);
  
  Task RestoreApplicationState();
  Task PersistApplicationState();
}
```

As you can see, you get the standard `Navigate()` and `GoBack()` methods, but they allow navigating to specific ViewModel and passing data to. To make this work, there is some plumbing required.

### Hookup Views to your ViewModels

First you need to somehow connect your Views with corresponding ViewModels. Then when you navigate to a ViewModel, this gets translated to navigating to the corresponding View.

You can use two appraches:
* Use AssociatedViewModel attribute on the given Page, e.g. `[AssociatedViewModel(typeof(NextViewModel)]`
* Or manually register the binding in the `IViewLocator` instance

When you use the attribute, it add a bit of overhead since the solution tries to find a matching type via reflection in all loaded assemblies. To speed up the initial start of application, it might be a good idea to manually register the main page and use the attribute for everything else.

### Have your ViewModels implement `IIn` and `IIn<TData>`

If you want to have some entrypoint to a ViewModel once it gets activated (when the app navigates to corresponing page),have it implement `IIn` or `IIn<TData>` interface. They look like this:

```
public interface IIn
{
  void Activate();
}

public interface IIn<TData>
{
  void Activate(TData data);
}
```

Then when you navigate to the given ViewModel, the NavigationService checks whether it implements one of the above interfaces (given by which overload of `Navigate()` method you use of course) and call to the corresponding `Activate()` method is invoked.

## State persistance

