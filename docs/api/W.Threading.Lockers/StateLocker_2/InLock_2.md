StateLocker&lt;TLocker, TState>.InLock Method (Func&lt;TState, TState>)
=======================================================================
   Performs a function from within a lock, passing in the current state and assigning the state to the function result

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public TState InLock(
	Func<TState, TState> func
)
```

#### Parameters

##### *func*
Type: [System.Func][2]&lt;[TState][3], [TState][3]>  
The function to run

#### Return Value
Type: [TState][3]  

[Missing &lt;returns> documentation for "M:W.Threading.Lockers.StateLocker`2.InLock(System.Func{`1,`1})"]

#### Implements
[IStateLocker&lt;TLocker, TState>.InLock(Func&lt;TState, TState>)][4]  


See Also
--------

#### Reference
[StateLocker&lt;TLocker, TState> Class][3]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549151
[3]: README.md
[4]: ../IStateLocker_2/InLock_1.md