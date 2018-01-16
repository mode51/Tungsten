StateLocker&lt;TLocker, TState>.InLockAsync&lt;TResult> Method (Func&lt;TResult>)
=================================================================================
   Asynchronously performs a function from within a lock

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Task<TResult> InLockAsync<TResult>(
	Func<TResult> func
)

```

#### Parameters

##### *func*
Type: [System.Func][2]&lt;**TResult**>  
The function to run

#### Type Parameters

##### *TResult*

[Missing &lt;typeparam name="TResult"/> documentation for "M:W.Threading.Lockers.StateLocker`2.InLockAsync``1(System.Func{``0})"]


#### Return Value
Type: [Task][3]&lt;**TResult**>  

[Missing &lt;returns> documentation for "M:W.Threading.Lockers.StateLocker`2.InLockAsync``1(System.Func{``0})"]

#### Implements
[ILocker.InLockAsync&lt;TResult>(Func&lt;TResult>)][4]  


See Also
--------

#### Reference
[StateLocker&lt;TLocker, TState> Class][5]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb534960
[3]: http://msdn.microsoft.com/en-us/library/dd321424
[4]: ../ILocker/InLockAsync__1.md
[5]: README.md