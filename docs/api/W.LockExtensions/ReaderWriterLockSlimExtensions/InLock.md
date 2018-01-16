ReaderWriterLockSlimExtensions.InLock Method (ReaderWriterLockSlim, LockTypeEnum, Action)
=========================================================================================
   Performs the action in a lock

  **Namespace:**  [W.LockExtensions][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static void InLock(
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

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [ReaderWriterLockSlim][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][5] or [Extension Methods (C# Programming Guide)][6].

See Also
--------

#### Reference
[ReaderWriterLockSlimExtensions Class][7]  
[W.LockExtensions Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb300132
[3]: ../../W.Threading.Lockers/LockTypeEnum/README.md
[4]: http://msdn.microsoft.com/en-us/library/bb534741
[5]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[6]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[7]: README.md