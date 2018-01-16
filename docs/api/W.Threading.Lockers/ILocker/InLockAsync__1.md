ILocker.InLockAsync&lt;TResult> Method (Func&lt;TResult>)
=========================================================
   Asyncrhonously perform some function in a lock

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
Task<TResult> InLockAsync<TResult>(
	Func<TResult> func
)

```

#### Parameters

##### *func*
Type: [System.Func][2]&lt;**TResult**>  
The function to perform

#### Type Parameters

##### *TResult*
The result Type

#### Return Value
Type: [Task][3]&lt;**TResult**>  
The result of the function

See Also
--------

#### Reference
[ILocker Interface][4]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb534960
[3]: http://msdn.microsoft.com/en-us/library/dd321424
[4]: README.md