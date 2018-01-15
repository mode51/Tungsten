ExtensionMethods.Lock&lt;TItemType> Method
==========================================
   Performs the given action in a lock statement using the provided object for the lock

  **Namespace:**  [W.Extensions][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static void Lock<TItemType>(
	this TItemType this,
	Action<TItemType> action
)

```

#### Parameters

##### *this*
Type: **TItemType**  
The object to use in the lock

##### *action*
Type: [System.Action][2]&lt;**TItemType**>  
The action to perform in a lock statement

#### Type Parameters

##### *TItemType*
The type of object on which the lock is made

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type . When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][3] or [Extension Methods (C# Programming Guide)][4].

See Also
--------

#### Reference
[ExtensionMethods Class][5]  
[W.Extensions Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/018hxwa8
[3]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[4]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[5]: README.md