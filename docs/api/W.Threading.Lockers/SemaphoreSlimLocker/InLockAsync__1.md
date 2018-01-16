SemaphoreSlimLocker.InLockAsync&lt;TValue> Method (Func&lt;TValue>)
===================================================================
   Executes a function from within a SemaphoreSlim

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Task<TValue> InLockAsync<TValue>(
	Func<TValue> func
)

```

#### Parameters

##### *func*
Type: [System.Func][2]&lt;**TValue**>  
The function to run

#### Type Parameters

##### *TValue*
The type of return value

#### Return Value
Type: [Task][3]&lt;**TValue**>  
The result of the function call (a value of type TValue)

See Also
--------

#### Reference
[SemaphoreSlimLocker Class][4]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb534960
[3]: http://msdn.microsoft.com/en-us/library/dd321424
[4]: README.md