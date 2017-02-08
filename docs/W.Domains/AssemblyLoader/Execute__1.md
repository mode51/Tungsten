AssemblyLoader.Execute&lt;TResult> Method (String, String, Object[])
====================================================================
  Instantiates a class and calls a method exposed by it.

  **Namespace:**  [W.Domains][1]  
  **Assembly:**  Tungsten.Domains (in Tungsten.Domains.dll)

Syntax
------

```csharp
public TResult Execute<TResult>(
	string typeName,
	string methodName,
	params Object[] args
)

```

#### Parameters

##### *typeName*
Type: [System.String][2]  
The name of the type which exposes the static method

##### *methodName*
Type: [System.String][2]  
The name of the static method

##### *args*
Type: [System.Object][3][]  
Any arguments to be passed to the static method

#### Type Parameters

##### *TResult*
The result of the function call is cast to TResult

#### Return Value
Type: **TResult**  
The return value from the function, casted to TResult
#### Implements
[IAssemblyLoader.Execute&lt;TResult>(String, String, Object[])][4]  


See Also
--------

#### Reference
[AssemblyLoader Class][5]  
[W.Domains Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[4]: ../IAssemblyLoader/Execute__1.md
[5]: README.md
[6]: ../../_icons/Help.png