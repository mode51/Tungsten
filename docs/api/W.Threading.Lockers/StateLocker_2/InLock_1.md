StateLocker&lt;TLocker, TState>.InLock Method (Action&lt;TState>)
=================================================================
   Performs an action from within a lock, passing in the current state

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public void InLock(
	Action<TState> action
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[TState][3]>  
The action to run

#### Implements
[IStateLocker&lt;TLocker, TState>.InLock(Action&lt;TState>)][4]  


See Also
--------

#### Reference
[StateLocker&lt;TLocker, TState> Class][3]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/018hxwa8
[3]: README.md
[4]: ../IStateLocker_2/InLock.md