# Adaptive navigation

When navigating to new ViewModel, you may not always want to navigate to new Page - e.g. on a larger screen you want to remain on the existing Page and only navigate to new one when the size of windows shrinks. And vice-versa, when you are on a second Page and the window gets larger, you want to jump to the previous page and have both Views side-by-side. Much like in the Outlook app in Windows 10.

For this reason, we are need to talk about two different navigations - View navigation (View navigation stack) and ViewModel navigation (ViewModel navigation stack)

This can be done fairly easily using this library. 

## Adaptive triggers

Let's start with the MainPage. Let's say you want to have two panes - left is the main one, right secondary and only visible when the size of the Window is 700 or more. 

To do this, you need to do 2 things:

 * Put the second View into a separate `UserControl` and use it as right pane on the `MainPage.` The `DataContext` of this `UserControl` should be a `SecondViewModel`
 * Use `AdaptiveTrigger` - to show / hide this control as the size of the Window changes

What it basically comes down to is this:

```
...
  <VisualStateManager.VisualStateGroups>
    <VisualStateGroup>
      <VisualStateGroup>
        <VisualState.StateTriggers>
          <AdaptiveTrigger MinWindowWidth="0"/>  
        </VisualState.StateTriggers> 
        <VisualState.Setters> 
          <Setter Target="userControl.Visibility" Value="Collapsed"/> 
        </VisualState.Setters> 
      </VisualStateGroup>
      <VisualState.StateTriggers>
        <AdaptiveTrigger MinWindowWidth="700"/>  
      </VisualState.StateTriggers> 
      <VisualState.Setters> 
        <Setter Target="userControl.Visibility" Value="Visible"/> 
      </VisualState.Setters> 
    </VisualStateGroup>
  </VisualStateManager.VisualStateGroups>
...
```

More on adaptive triggers here: http://www.c-sharpcorner.com/UploadFile/1ff178/adaptive-trigger-for-xaml-uap-dev/

## Navigation interceptors

The `PlatformNavigator` requires an array of `INavigationInterceptor`s to be provided. A `INavigationInterceptor` basically allows to control when a View navigation should actually happen, based on some condition. Currently there is one implementation in the library which can be used - `AdaptiveNavigationInterceptor`. When navigating to target Page, it looks for a `AdaptiveNavigationAttribute` and based on that determines whether View navigation should happen or not (but ViewModel navigation happens in either case).

## Second page

Last thing you need to do is create `SecondPage` - inside of it you again use the `UserControl`. Then decorate this Page with `AdaptiveNavigationAttributeByWidth` attribute e.g.

```
    [AdaptiveNavigationByWidthAttribute(MinWidth = 700)]
    [AssociatedViewModel(typeof(SecondViewModel)]
    public partial class SecondPage : Page
    {
        public SecondPage()
        {
            InitializeComponent();
        }
    }
```

`AdaptiveNavigationByWidthAttribute` inherits from `AdaptiveNavigationAttribute` and allows navigation based on the width of the Window. 

## Realtime changes

Much like the UAP `AdaptiveTrigger`, the current infrastructure allows the `PlatformNavigator` to listen to changes in the underlying condition modify the current View navigation stack on the fly. 

For example, when you have a small window under 700px, navigate to the `NextViewModel`, `NextPage` is shown. However when you expand the width of the Window, the `NextPage` is removed from the View navigation stack and `MainPage` is displayed.
