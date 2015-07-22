![Logo](/art/logo.128x128.png)

# MVVM-Navigation

## What?
Navigation framework to allow true MVVM-like navigation in UAP (Universal App Platform).

## Why?

Platform API supports only navigating to target Page type:
```
frame.Navigate(typeof(NewPage));
```

optionally passing in simple parameters:

```
frame.Navigate(typeof(NewPage), userId);
```

In your `NewPage` codebehind you then need to override `OnNavigatedTo`, get the navigation parameter and pass it to your viewmodel.

Not only is it a lot of repetitive work, but if you want to perform navigation in you viewmodel, this effectively breaks your separation of concerns and the MVVM pattern. 

With the rise of UAP you also have to manually deal with different screen factors - on a small screen, you want to navigate to new page, but on a larger one you just want to bring in new view on the existing page.

You also have to manually take care of persisting and restoring your state to support tombstoning of your app. 

## How?

With the MVMM Navigation you can just write something like this:

```
navigationService.Navigate<NewViewModel>();
```

optionally passing even complex parameters:

```
navigationService.Navigate<NewViewModel, User>(new User { Id = 42 });
```

The `NewViewModel` just needs to implement the corresponding interface:

```
public interface IActivate
{
  void Activate();
}

public interface IActivate<TData>
{
  void Activate(TData data);
}
```

and the `Activate` method gets invoked when you navigate to the given viewmodel. To allow this navigation to viewmodels, the framework needs to know which view corresponds to which viewmodel. They way to set it up is described [here](doc/view-lookup.md).

## There is more?

There are other cool features in the library. Following are noteble:

* Viewmodel activation and deactivation - pass data to your viewmodels and let them gracefully deactivate or prevent deactivation (and hence navigation) altogether. Read more [here](doc/activation-deactivation.md). 
* Automatic state persistance for tombstoning - the data you send between viewmodels when navigating is persisted and automatically restored. Read more [here](doc/state-persistence.md).
* Adaptive platform navigation - when navigating to new viewmodel, you may not always want to navigate to new Page - e.g. on a larger screen you want to remain on the existing Page and only navigate to new Page when the size of the Window shrinks. Read more [here](doc/adaptive-navigation.md).

## Where?

Install-Package Bezysoftware.Universal.Navigation

## How do I start?

There is some plumbing needed to make all this work. [This](doc/plumbing.md) page describes all steps you need to undertake when setting up new project.

You can also take a look at the sample project located [here](src/) (part of the main solution). 
