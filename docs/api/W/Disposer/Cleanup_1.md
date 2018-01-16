Disposer.Cleanup Method (Object, Action)
========================================
   Calls the action (should contain cleanup code)

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public void Cleanup(
	Object objToSupressFinalize,
	Action cleanupAction
)
```

#### Parameters

##### *objToSupressFinalize*
Type: [System.Object][2]  
The object on which to suppress the finalizer call (usually the one currently being disposed)

##### *cleanupAction*
Type: [System.Action][3]  
The action to call


See Also
--------

#### Reference
[Disposer Class][4]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/bb534741
[4]: README.md