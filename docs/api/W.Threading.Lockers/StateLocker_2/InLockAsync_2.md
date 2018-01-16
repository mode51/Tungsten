StateLocker&lt;TLocker, TState>.InLockAsync Method (Func&lt;TState, TState>)
============================================================================
   Asynchronously performs a function from within a lock, passing in the current state and assigning the state to the function result

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Task<TState> InLockAsync(
	Func<TState, TState> func
)
```

#### Parameters

##### *func*
Type: [System.Func][2]&lt;[TState][3], [TState][3]>  
The function to run

#### Return Value
Type: [Task][4]&lt;[TState][3]>  

[Missing &lt;returns> documentation for "M:W.Threading.Lockers.StateLocker`2.InLockAsync(System.Func{`1,`1})"]

#### Implements
[IStateLocker&lt;TLocker, TState>.InLockAsync(Func&lt;TState, TState>)][5]  


See Also
--------

#### Reference
[StateLocker&lt;TLocker, TState> Class][3]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549151
[3]: README.md
[4]: http://msdn.microsoft.com/en-us/library/dd321424
[5]: ../IStateLocker_2/InLockAsync_1.md