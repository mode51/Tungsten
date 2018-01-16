StateLocker&lt;TLocker, TState>.InLockAsync Method (Action&lt;TState>)
======================================================================
   Asynchronously performs an action from within a lock, passing in the current state

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Task InLockAsync(
	Action<TState> action
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[TState][3]>  

[Missing &lt;param name="action"/> documentation for "M:W.Threading.Lockers.StateLocker`2.InLockAsync(System.Action{`1})"]


#### Return Value
Type: [Task][4]  

[Missing &lt;returns> documentation for "M:W.Threading.Lockers.StateLocker`2.InLockAsync(System.Action{`1})"]

#### Implements
[IStateLocker&lt;TLocker, TState>.InLockAsync(Action&lt;TState>)][5]  


See Also
--------

#### Reference
[StateLocker&lt;TLocker, TState> Class][3]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/018hxwa8
[3]: README.md
[4]: http://msdn.microsoft.com/en-us/library/dd235678
[5]: ../IStateLocker_2/InLockAsync.md