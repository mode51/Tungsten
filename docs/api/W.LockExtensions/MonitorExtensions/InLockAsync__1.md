MonitorExtensions.InLockAsync&lt;TType> Method (Object, Func&lt;TType>)
=======================================================================
   Asynchronously performs the action in a Monitor lock

  **Namespace:**  [W.LockExtensions][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Task<TType> InLockAsync<TType>(
	this Object this,
	Func<TType> func
)

```

#### Parameters

##### *this*
Type: [System.Object][2]  
The object to provide resource locking

##### *func*
Type: [System.Func][3]&lt;**TType**>  
The function to perform

#### Type Parameters

##### *TType*

[Missing &lt;typeparam name="TType"/> documentation for "M:W.LockExtensions.MonitorExtensions.InLockAsync``1(System.Object,System.Func{``0})"]


#### Return Value
Type: [Task][4]&lt;**TType**>  

[Missing &lt;returns> documentation for "M:W.LockExtensions.MonitorExtensions.InLockAsync``1(System.Object,System.Func{``0})"]

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [Object][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][5] or [Extension Methods (C# Programming Guide)][6].

See Also
--------

#### Reference
[MonitorExtensions Class][7]  
[W.LockExtensions Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/bb534960
[4]: http://msdn.microsoft.com/en-us/library/dd321424
[5]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[6]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[7]: README.md