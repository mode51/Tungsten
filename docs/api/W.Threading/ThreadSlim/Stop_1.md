ThreadSlim.Stop Method (Int32)
==============================
   Signals the thread method to stop running and waits the specified number of milliseconds for it to complete before returning

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public bool Stop(
	int msTimeout
)
```

#### Parameters

##### *msTimeout*
Type: [System.Int32][2]  
The number of milliseconds to wait for the thread method to comlete

#### Return Value
Type: [Boolean][3]  
True if the thread method completes within the specified number of milliseconds, otherwise False

See Also
--------

#### Reference
[ThreadSlim Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/td2s409d
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: README.md