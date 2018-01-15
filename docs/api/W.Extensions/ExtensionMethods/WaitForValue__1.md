ExtensionMethods.WaitForValue&lt;TItemType> Method
==================================================
   Initiates a Task which will wait for the specified condition to be met

  **Namespace:**  [W.Extensions][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static bool WaitForValue<TItemType>(
	this TItemType this,
	Predicate<TItemType> predicate,
	int msTimeout = -1
)

```

#### Parameters

##### *this*
Type: **TItemType**  
The value being inspected

##### *predicate*
Type: [System.Predicate][2]&lt;**TItemType**>  
The condition to be met

##### *msTimeout* (Optional)
Type: [System.Int32][3]  
The task will time out within the specified number of milliseconds. Use -1 to wait indefinitely.

#### Type Parameters

##### *TItemType*
The object Type of the item being extended

#### Return Value
Type: [Boolean][4]  
True if the condition was met within the specified timeout, otherwise False
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type . When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][5] or [Extension Methods (C# Programming Guide)][6].

See Also
--------

#### Reference
[ExtensionMethods Class][7]  
[W.Extensions Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bfcke1bz
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: http://msdn.microsoft.com/en-us/library/a28wyd50
[5]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[6]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[7]: README.md