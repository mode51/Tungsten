IStateLocker&lt;TLocker, TState>.InLock Method (Action&lt;TState>)
==================================================================
   Perform some action in a lock

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
void InLock(
	Action<TState> action
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[TState][3]>  
The action to perform


See Also
--------

#### Reference
[IStateLocker&lt;TLocker, TState> Interface][3]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/018hxwa8
[3]: README.md