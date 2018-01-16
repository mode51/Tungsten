ReaderWriterLocker.InLock Method (LockTypeEnum, Action)
=======================================================
   Executes an action from within a ReaderWriterLockSlim

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public void InLock(
	LockTypeEnum lockType,
	Action action
)
```

#### Parameters

##### *lockType*
Type: [W.Threading.Lockers.LockTypeEnum][2]  

[Missing &lt;param name="lockType"/> documentation for "M:W.Threading.Lockers.ReaderWriterLocker.InLock(W.Threading.Lockers.LockTypeEnum,System.Action)"]


##### *action*
Type: [System.Action][3]  
The action to run


See Also
--------

#### Reference
[ReaderWriterLocker Class][4]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: ../LockTypeEnum/README.md
[3]: http://msdn.microsoft.com/en-us/library/bb534741
[4]: README.md