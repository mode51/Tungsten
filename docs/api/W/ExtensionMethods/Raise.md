ExtensionMethods.Raise Method
=============================
   Calls the delegate, passing in any arguments. Provides error handling to allow all subscribers to handle the delegate.

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static List<Exception> Raise(
	this Delegate del,
	params Object[] args
)
```

#### Parameters

##### *del*
Type: [System.Delegate][2]  
The delegate to call

##### *args*
Type: [System.Object][3][]  
Parameters to pass into the delegate

#### Return Value
Type: [List][4]&lt;[Exception][5]>  
Exceptions if any are thrown
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [Delegate][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][6] or [Extension Methods (C# Programming Guide)][7].

See Also
--------

#### Reference
[ExtensionMethods Class][8]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/y22acf51
[3]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[4]: http://msdn.microsoft.com/en-us/library/6sh2ey19
[5]: http://msdn.microsoft.com/en-us/library/c18k6c59
[6]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[7]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[8]: README.md