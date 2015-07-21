# View lookup

When you navigate to given ViewModel, the system uses a View Locator to find the corresponding View associated with the given ViewModel. To set these associations, you have two options:

 * Manually register the binding using the `RegisterAssociation` method
 * Use `AssociatedViewModel` attribute on the given Page, e.g. `[AssociatedViewModel(typeof(NextViewModel)]`

The manual solution is straigthforward, you just tell the View Locator what the associations are. This can get annoying because it needs to be maintained manually, however it is fast.

When you use the attribute, it adds a bit of overhead since the solution tries to find a matching type via reflection. But note this lookup is done just once and then cached.

The View Locator requires an instance of `IAssemblyResolver` to know out which assemblies to search in. There are two out-of-the-box implementations you can use:
 * `NullAssemblyResolver` - doesn't return any assemblies, you can use this when you know you will register your View-ViewModel associations manually.
 * `GlobalAssemblyResolver` - returns all assemblies which are deployed with your app. 
 
The third option is to write a custom implementation, which will return only the assemblies where your Views are. See the sample project to see how it's done, it's virtually a one-liner.

When using the attribute, to speed up the initial start of application, it might be a good idea to manually register the main page and use the attribute for everything else.
