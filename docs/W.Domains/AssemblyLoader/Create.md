AssemblyLoader.Create Method (String)
=====================================
  Instantiates a class and returns a handle to it. This handle must be cast to an interface in order to work across AppDomains.

  **Namespace:**  [W.Domains][1]  
  **Assembly:**  Tungsten.Domains (in Tungsten.Domains.dll)

Syntax
------

```csharp
public Object Create(
	string typeName
)
```

#### Parameters

##### *typeName*
Type: [System.String][2]  
The name of the type which is to be instantiated

#### Return Value
Type: [Object][3]  
A handle to the instantiated object. This value should be cast to an interface as only interfaces will work across AppDomains.
#### Implements
[IAssemblyLoader.Create(String)][4]  


See Also
--------

#### Reference
[AssemblyLoader Class][5]  
[W.Domains Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[4]: ../IAssemblyLoader/Create.md
[5]: README.md
[6]: ../../_icons/Help.png