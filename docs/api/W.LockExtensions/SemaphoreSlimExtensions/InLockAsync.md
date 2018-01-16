SemaphoreSlimExtensions.InLockAsync Method (SemaphoreSlim, Action)
==================================================================
   Asynchronously performs the action in a lock

  **Namespace:**  [W.LockExtensions][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Task InLockAsync(
	this SemaphoreSlim this,
	Action action
)
```

#### Parameters

##### *this*
Type: [System.Threading.SemaphoreSlim][2]  
The SemaphoreSlim to provide resource locking

##### *action*
Type: [System.Action][3]  
The action to perform

#### Return Value
Type: [Task][4]  

[Missing &lt;returns> documentation for "M:W.LockExtensions.SemaphoreSlimExtensions.InLockAsync(System.Threading.SemaphoreSlim,System.Action)"]

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [SemaphoreSlim][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][5] or [Extension Methods (C# Programming Guide)][6].

See Also
--------

#### Reference
[SemaphoreSlimExtensions Class][7]  
[W.LockExtensions Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/dd271035
[3]: http://msdn.microsoft.com/en-us/library/bb534741
[4]: http://msdn.microsoft.com/en-us/library/dd235678
[5]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[6]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[7]: README.md