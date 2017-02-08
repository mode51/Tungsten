DomainLoader.GetData&lt;TData> Method
=====================================
  Gets the value stored in the current application domain for the specified name

  **Namespace:**  [W.Domains][1]  
  **Assembly:**  Tungsten.Domains (in Tungsten.Domains.dll)

Syntax
------

```csharp
public TData GetData<TData>(
	string name
)

```

#### Parameters

##### *name*
Type: [System.String][2]  
The name of a predefined or custom domain property

#### Type Parameters

##### *TData*
The type of data to be returned

#### Return Value
Type: **TData**  
The data stored in the domain property as cast to T

See Also
--------

#### Reference
[DomainLoader Class][3]  
[W.Domains Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: README.md
[4]: ../../_icons/Help.png