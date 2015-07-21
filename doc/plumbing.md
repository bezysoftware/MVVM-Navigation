# Plumbing

This is a list of things you need to do to get framework to work. There might be a better way in the future.

## ApplicationFrameProvider

You need to have an implementation of `IApplicationFrameProvider` - it should come down to a one liner, which will just return current instance of `Frame`.

## Register types in ServiceLocator

The built-in implementation of `IViewModelLocator`, which is reponsible for ViewModel lookup uses a ServiceLocator under the hood. I recommend using some kind of IoC / DI framework (such as [Unity](https://unity.codeplex.com/)) and hook it up to `ServiceLocator`. See the sample project for details.

Or you can of course have a custom implementation of `IViewModelLocator`.

## Modify `App.xaml.cs`

There are four things needed in `App.xaml.cs` - persist state, restore state, clear state in case app started normally, perform initial navigation (typically to `MainPage`).

Persist state when app is being suspended, restore in the `OnActivated` method (but only when it was terminated), clear state in `OnLaunched` method and perform initial navigation again in the `OnLaunched` event (no need to do navigation on `OnActivated`, the View stack is restored automatically). See the sample project for more code.

In the future some wrapping of all this might be added. 

## Construct it all

Last thing is to construct it all - again I recommend using some IoC / DI container for that. Then just pass `INavigationService` around and navigate :)
