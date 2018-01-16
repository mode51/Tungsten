SpinLocker.InLock&lt;TResult> Method (Func&lt;TResult>)
=======================================================
   Performs a function from within a SpinLock

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public TResult InLock<TResult>(
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
Type: **TResult**  
The result of the function call (a value of type TValue)
#### Implements
[ILocker.InLock&lt;TResult>(Func&lt;TResult>)][3]  


See Also
--------

#### Reference
[SpinLocker Class][4]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb534960
[3]: ../ILocker/InLock__1.md
[4]: README.md