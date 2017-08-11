ThreadBase.Cancel Method (Int32)
================================
  
Cancels the thread by calling Cancel on the CancellationTokenSource. The value should be checked in the code in the specified Action parameter.


  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public virtual void Cancel(
	int msForceAbortDelay
)
```

#### Parameters

##### *msForceAbortDelay*
Type: [System.Int32][2]  
Abort the thread if it doesn't terminate before the specified number of milliseconds elapse


See Also
--------

#### Reference
[ThreadBase Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/td2s409d
[3]: README.md