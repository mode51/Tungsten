ILocker.InLock&lt;TResult> Method (Func&lt;TResult>)
====================================================
   Perform some function in a lock

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
TResult InLock<TResult>(
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
Type: **TResult**  
The result of the function

See Also
--------

#### Reference
[ILocker Interface][3]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb534960
[3]: README.md