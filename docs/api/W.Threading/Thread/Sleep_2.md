Thread.Sleep Method (CPUProfileEnum)
====================================
   Attempts to free the CPU for other processes, based on the desired level. Consequences will vary depending on your hardware architecture. The more processors/cores you have, the better performance you will have by selecting LowCPU. Likewise, on a single-core processor, you may wish to select HighCPU.

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static void Sleep(
	CPUProfileEnum level
)
```

#### Parameters

##### *level*
Type: [W.Threading.CPUProfileEnum][2]  
The desired level of CPU usage


Remarks
-------
Note results may vary. LowCPU will spread the load onto multiple cores and can actually yield faster results depending on your hardware architecture. This may not always be the case.

See Also
--------

#### Reference
[Thread Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: ../CPUProfileEnum/README.md
[3]: README.md