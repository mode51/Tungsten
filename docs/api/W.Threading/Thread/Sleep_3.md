Thread.Sleep Method (CPUProfileEnum, Int32)
===========================================
   Attempts to free the CPU for other processes, based on the desired level. Consequences will vary depending on your hardware architecture. The more processors/cores you have, the better performance you will have by selecting SpinWait1. Likewise, on a single-core processor, you may wish to select SpinWait0.

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static void Sleep(
	CPUProfileEnum level,
	int msTimeout = 1
)
```

#### Parameters

##### *level*
Type: [W.Threading.CPUProfileEnum][2]  
The desired level of CPU usage

##### *msTimeout* (Optional)
Type: [System.Int32][3]  
Optional value for CPUProfileEnum.Sleep and CPUProfileEnum.SpinUntil. Ignored by other profiles.


Remarks
-------
Note results may vary. SpinWait1 will spread the load onto multiple cores and can actually yield faster results depending on your hardware architecture. This may not always be the case.

See Also
--------

#### Reference
[Thread Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: ../CPUProfileEnum/README.md
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: README.md