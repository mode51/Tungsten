Lockable&lt;TValue>.ExecuteInLockAsync Method (Func&lt;TValue, TValue>)
=======================================================================
   Executes a task within a lock of the LockObject

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public void ExecuteInLockAsync(
	Func<TValue, TValue> function
)
```

#### Parameters

##### *function*
Type: [System.Func][2]&lt;[TValue][3], [TValue][3]>  
The function to call within a lock


See Also
--------

#### Reference
[Lockable&lt;TValue> Class][3]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549151
[3]: README.md