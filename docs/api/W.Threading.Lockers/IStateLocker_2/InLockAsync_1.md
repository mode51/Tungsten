IStateLocker&lt;TLocker, TState>.InLockAsync Method (Func&lt;TState, TState>)
=============================================================================
   Asyncrhonously perform some function in a lock

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
Task<TState> InLockAsync(
	Func<TState, TState> func
)
```

#### Parameters

##### *func*
Type: [System.Func][2]&lt;[TState][3], [TState][3]>  
The function to perform

#### Return Value
Type: [Task][4]&lt;[TState][3]>  
The result of the function

See Also
--------

#### Reference
[IStateLocker&lt;TLocker, TState> Interface][3]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549151
[3]: README.md
[4]: http://msdn.microsoft.com/en-us/library/dd321424