ReaderWriterLocker&lt;TState>.InLockAsync Method (LockTypeEnum, Func&lt;TState, TState>)
========================================================================================
   Asynchronously performs the function in a lock, passing in the current state and assigning the state to the result

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Task<TState> InLockAsync(
	LockTypeEnum lockType,
	Func<TState, TState> func
)
```

#### Parameters

##### *lockType*
Type: [W.Threading.Lockers.LockTypeEnum][2]  

[Missing &lt;param name="lockType"/> documentation for "M:W.Threading.Lockers.ReaderWriterLocker`1.InLockAsync(W.Threading.Lockers.LockTypeEnum,System.Func{`0,`0})"]


##### *func*
Type: [System.Func][3]&lt;[TState][4], [TState][4]>  
The action to perform

#### Return Value
Type: [Task][5]&lt;[TState][4]>  

[Missing &lt;returns> documentation for "M:W.Threading.Lockers.ReaderWriterLocker`1.InLockAsync(W.Threading.Lockers.LockTypeEnum,System.Func{`0,`0})"]


See Also
--------

#### Reference
[ReaderWriterLocker&lt;TState> Class][4]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: ../LockTypeEnum/README.md
[3]: http://msdn.microsoft.com/en-us/library/bb549151
[4]: README.md
[5]: http://msdn.microsoft.com/en-us/library/dd321424