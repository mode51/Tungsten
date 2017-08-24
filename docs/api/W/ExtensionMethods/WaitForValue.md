ExtensionMethods.WaitForValue Method (Object, Object, Int32)
============================================================
  Initiates a Task which will wait for the given variable to have the specified value

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Task<bool> WaitForValue(
	this Object this,
	Object desiredValue,
	int msTimeout = -1
)
```

#### Parameters

##### *this*
Type: [System.Object][2]  
The value being inspected

##### *desiredValue*
Type: [System.Object][2]  
The value to wait for

##### *msTimeout* (Optional)
Type: [System.Int32][3]  
The task will time out within the specified number of milliseconds. Use -1 to wait indefinitely.

#### Return Value
Type: [Task][4]&lt;[Boolean][5]>  
True if the value was acquired within the specified timeout, otherwise False
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [Object][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][6] or [Extension Methods (C# Programming Guide)][7].

See Also
--------

#### Reference
[ExtensionMethods Class][8]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: http://msdn.microsoft.com/en-us/library/dd321424
[5]: http://msdn.microsoft.com/en-us/library/a28wyd50
[6]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[7]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[8]: README.md