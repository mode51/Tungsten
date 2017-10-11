Thread.Sleep Method (Int32, Boolean)
====================================
   Blocks the calling thread for the specified time

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static void Sleep(
	int msDelay,
	bool useSpinWait
)
```

#### Parameters

##### *msDelay*
Type: [System.Int32][2]  
The number of milliseconds to block the thread

##### *useSpinWait*
Type: [System.Boolean][3]  
If True, a SpinWait.SpinUntil will be used instead of a call to Thread.Sleep (or Task.Delay). Note that SpinWait should only be used on multi-core/cpu machines.


See Also
--------

#### Reference
[Thread Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/td2s409d
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: README.md