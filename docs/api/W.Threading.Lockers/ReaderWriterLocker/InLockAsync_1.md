ReaderWriterLocker.InLockAsync Method (LockTypeEnum, Action)
============================================================
   Executes an action from within a ReaderWriterLockSlim

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Task InLockAsync(
	LockTypeEnum lockType,
	Action action
)
```

#### Parameters

##### *lockType*
Type: [W.Threading.Lockers.LockTypeEnum][2]  

[Missing &lt;param name="lockType"/> documentation for "M:W.Threading.Lockers.ReaderWriterLocker.InLockAsync(W.Threading.Lockers.LockTypeEnum,System.Action)"]


##### *action*
Type: [System.Action][3]  
The action to run

#### Return Value
Type: [Task][4]  

[Missing &lt;returns> documentation for "M:W.Threading.Lockers.ReaderWriterLocker.InLockAsync(W.Threading.Lockers.LockTypeEnum,System.Action)"]


See Also
--------

#### Reference
[ReaderWriterLocker Class][5]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: ../LockTypeEnum/README.md
[3]: http://msdn.microsoft.com/en-us/library/bb534741
[4]: http://msdn.microsoft.com/en-us/library/dd235678
[5]: README.md