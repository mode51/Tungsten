Gate.Join Method (Int32)
========================
  Blocks the calling thread until either the thread terminates or the specified milliseconds elapse

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public override bool Join(
	int msTimeout
)
```

#### Parameters

##### *msTimeout*
Type: [System.Int32][2]  
The number of milliseconds to wait for the thread to terminate

#### Return Value
Type: [Boolean][3]  
True if the thread terminates within the timeout specified, otherwise false

See Also
--------

#### Reference
[Gate Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/td2s409d
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: README.md