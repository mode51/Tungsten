DomainLoader Constructor (String, Boolean)
==========================================
  Creates an AppDomain under the current AppDomain

  **Namespace:**  [W.Domains][1]  
  **Assembly:**  Tungsten.Domains (in Tungsten.Domains.dll)

Syntax
------

```csharp
public DomainLoader(
	string relativeSubFolderForDomain,
	bool useShadowCopy = false
)
```

#### Parameters

##### *relativeSubFolderForDomain*
Type: [System.String][2]  
The relative path to the subfolder which will be the root folder for the new AppDomain

##### *useShadowCopy* (Optional)
Type: [System.Boolean][3]  
True to shadow copy files. This allows dlls to be added, removed or modified while the AppDomain is still loaded.


See Also
--------

#### Reference
[DomainLoader Class][4]  
[W.Domains Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: README.md
[5]: ../../_icons/Help.png