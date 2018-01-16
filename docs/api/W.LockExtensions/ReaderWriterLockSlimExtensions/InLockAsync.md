ReaderWriterLockSlimExtensions.InLockAsync Method (ReaderWriterLockSlim, LockTypeEnum, Action)
==============================================================================================
   Asynchronously performs the action in a lock

  **Namespace:**  [W.LockExtensions][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Task InLockAsync(
	this ReaderWriterLockSlim this,
	LockTypeEnum lockType,
	Action action
)
```

#### Parameters

##### *this*
Type: [System.Threading.ReaderWriterLockSlim][2]  
The ReaderWriterLockSlim to provide resource locking

##### *lockType*
Type: [W.Threading.Lockers.LockTypeEnum][3]  
The type of lock to obtain

##### *action*
Type: [System.Action][4]  
The action to perform

#### Return Value
Type: [Task][5]  

[Missing &lt;returns> documentation for "M:W.LockExtensions.ReaderWriterLockSlimExtensions.InLockAsync(System.Threading.ReaderWriterLockSlim,W.Threading.Lockers.LockTypeEnum,System.Action)"]

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [ReaderWriterLockSlim][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][6] or [Extension Methods (C# Programming Guide)][7].

See Also
--------

#### Reference
[ReaderWriterLockSlimExtensions Class][8]  
[W.LockExtensions Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb300132
[3]: ../../W.Threading.Lockers/LockTypeEnum/README.md
[4]: http://msdn.microsoft.com/en-us/library/bb534741
[5]: http://msdn.microsoft.com/en-us/library/dd235678
[6]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[7]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[8]: README.md