MonitorLocker.InLock&lt;TValue> Method (Func&lt;TValue>)
========================================================
   Executes a function from within a Monitor

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public TValue InLock<TValue>(
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
Type: **TValue**  
The result of the function call (a value of type TValue)

See Also
--------

#### Reference
[MonitorLocker Class][3]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb534960
[3]: README.md