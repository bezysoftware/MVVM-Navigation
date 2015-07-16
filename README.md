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

In you NewPage codebehind you then need to override OnNavigatedTo, get the navigation parameter and pass it to your ViewModel.

Not only is it a lot of repetitive work, but if you want to perform navigation in you ViewModel, this effectively breaks your separation of concerns and the MVVM pattern. 

With the rise of UAP you also have to manually deal with different screen factors - on a small screen, you want to navigate to new page, but on a larger one you just want to bring in new view on the existing Page.

You also have to manually take care of persisting and restoring your state to support tombstoning of your app. 

## Where?

Install-Package Bezysoftware.Universal.Navigation

## How?

With the MVMM Navigation you can just write something like this:

```
navigationService.Navigate<NewViewModel>();
```

optionally passing even complex parameters:

```
navigationService.Navigate<NewViewModel>(new User { Id = 42 });
```

The `NewViewModel` just needs to implement the corresponding interface:

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

and the `Activate` method gets invoked when you navigate to the given ViewModel.

## There is more?

There is a lot more present in the library. Following two features are noteble:

* Automatic state persistance for tombstoning - the data you send between viewmodels when navigating is persisted and automatically restored. Read more [here](doc/view-lookup.md).
* Adaptive platform navigation - when navigating to new viewmodel, you may not always want to navigate to new Page - e.g. on a larger screen you want to remain on the existing Page and only navigate to new Page when the size of windows shrinks. Read more [here](doc/adaptive-navigation.md).

## Plumbing

There is some plumbing needed to make all this work. [This](doc/plumbing.md) page describes all steps you need to undertake when setting up new project.

## Sample

There is a sample project you can check out. 
