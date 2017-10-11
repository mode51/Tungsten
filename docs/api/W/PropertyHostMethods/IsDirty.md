PropertyHostMethods.IsDirty Method
==================================
   
Scans the IsDirty value of each field and property of type IProperty


  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static bool IsDirty(
	this Object this
)
```

#### Parameters

##### *this*
Type: [System.Object][2]  
The object on which to inspect for dirty properties

#### Return Value
Type: [Boolean][3]  
True if any IProperty member's IsDirty value is true, otherwise false
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [Object][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][4] or [Extension Methods (C# Programming Guide)][5].

See Also
--------

#### Reference
[PropertyHostMethods Class][6]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[5]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[6]: README.md