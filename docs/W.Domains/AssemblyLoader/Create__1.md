AssemblyLoader.Create&lt;TInterfaceType> Method (String)
========================================================
  Instantiates a class and returns a handle to it. This handle must be cast to an interface in order to work across AppDomains.

  **Namespace:**  [W.Domains][1]  
  **Assembly:**  Tungsten.Domains (in Tungsten.Domains.dll)

Syntax
------

```csharp
public TInterfaceType Create<TInterfaceType>(
	string typeName
)

```

#### Parameters

##### *typeName*
Type: [System.String][2]  
The name of the type which is to be instantiated

#### Type Parameters

##### *TInterfaceType*
The handle to the class is automatically cast to the interfafce TInterfaceType

#### Return Value
Type: **TInterfaceType**  
A handle to the instantiated object. This value should be cast to an interface as only interfaces will work across AppDomains.

See Also
--------

#### Reference
[AssemblyLoader Class][3]  
[W.Domains Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: README.md
[4]: ../../_icons/Help.png