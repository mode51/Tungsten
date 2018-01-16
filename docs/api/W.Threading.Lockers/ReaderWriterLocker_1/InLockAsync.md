ReaderWriterLocker&lt;TState>.InLockAsync Method (LockTypeEnum, Action&lt;TState>)
==================================================================================
   Asynchronously performs the action in a lock, passing in the current state

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Task InLockAsync(
	LockTypeEnum lockType,
	Action<TState> action
)
```

#### Parameters

##### *lockType*
Type: [W.Threading.Lockers.LockTypeEnum][2]  

[Missing &lt;param name="lockType"/> documentation for "M:W.Threading.Lockers.ReaderWriterLocker`1.InLockAsync(W.Threading.Lockers.LockTypeEnum,System.Action{`0})"]


##### *action*
Type: [System.Action][3]&lt;[TState][4]>  
The action to perform

#### Return Value
Type: [Task][5]  

[Missing &lt;returns> documentation for "M:W.Threading.Lockers.ReaderWriterLocker`1.InLockAsync(W.Threading.Lockers.LockTypeEnum,System.Action{`0})"]


See Also
--------

#### Reference
[ReaderWriterLocker&lt;TState> Class][4]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: ../LockTypeEnum/README.md
[3]: http://msdn.microsoft.com/en-us/library/018hxwa8
[4]: README.md
[5]: http://msdn.microsoft.com/en-us/library/dd235678