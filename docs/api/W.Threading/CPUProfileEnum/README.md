CPUProfileEnum Enumeration
==========================
   The preferred level of CPU usage

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public enum CPUProfileEnum
```


Members
-------

Member name   | Value | Description                                                                                                                                   
------------- | ----- | --------------------------------------------------------------------------------------------------------------------------------------------- 
**SpinWait0** | 0     | High CPU usage, but fastest execution. May be faster on single-core/cpu machines. May be slower on multi-core/cpu machines.                   
**Sleep**     | 1     | Medium CPU usage. Uses Thread.Sleep or Task.Delay to block the current thread.                                                                
**SpinWait1** | 2     | Low CPU usage. Should be faster on multi-core/cpu machines as the load will be divided among cores/cpus. Slowest on single-core/cpu machines. 
**Yield**     | 3     | Only available for .Net Framework; uses Thread.Yield instead of Thread.Sleep.                                                                 
**SpinUntil** | 4     |                                                                                                                                               


See Also
--------

#### Reference
[W.Threading Namespace][1]  

[1]: ../README.md