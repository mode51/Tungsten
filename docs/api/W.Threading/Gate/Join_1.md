Gate.Join Method (Int32)
========================
   Blocks the calling thread until the gated Action is complete, or until the specified number of milliseconds has elapsed

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public bool Join(
	int msTimeout
)
```

#### Parameters

##### *msTimeout*
Type: [System.Int32][2]  
The number of milliseconds to wait for the gate to complete before timing out and returning False

#### Return Value
Type: [Boolean][3]  
True if the gate completed within the specified timeout, otherwise False

See Also
--------

#### Reference
[Gate Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/td2s409d
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: README.md