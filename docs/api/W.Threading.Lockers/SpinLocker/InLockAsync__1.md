SpinLocker.InLockAsync&lt;TResult> Method (Func&lt;TResult>)
============================================================
   Performs a function from within a SpinLock

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
The type of return value

#### Return Value
Type: [Task][3]&lt;**TResult**>  
The result of the function call (a value of type TValue)
#### Implements
[ILocker.InLockAsync&lt;TResult>(Func&lt;TResult>)][4]  


See Also
--------

#### Reference
[SpinLocker Class][5]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb534960
[3]: http://msdn.microsoft.com/en-us/library/dd321424
[4]: ../ILocker/InLockAsync__1.md
[5]: README.md