DomainLoader.ExecuteStaticMethod Method (String, String, Object[])
==================================================================
  Executes a static method on the specified type across the AppDomain

  **Namespace:**  [W.Domains][1]  
  **Assembly:**  Tungsten.Domains (in Tungsten.Domains.dll)

Syntax
------

```csharp
public void ExecuteStaticMethod(
	string typeName,
	string staticMethodName,
	params Object[] args
)
```

#### Parameters

##### *typeName*
Type: [System.String][2]  
The name of the type which exposes the static method

##### *staticMethodName*
Type: [System.String][2]  
The name of the static method

##### *args*
Type: [System.Object][3][]  
Any arguments to be passedd to the static method

#### Implements
[IDomainLoader.ExecuteStaticMethod(String, String, Object[])][4]  


See Also
--------

#### Reference
[DomainLoader Class][5]  
[W.Domains Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[4]: ../IDomainLoader/ExecuteStaticMethod.md
[5]: README.md
[6]: ../../_icons/Help.png