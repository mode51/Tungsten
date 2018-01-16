ThreadSlim.Stop Method (CancellationToken)
==========================================
   Signals the thread method to stop running and waits for the method to complete, while observing the specified CancellationToken

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public void Stop(
	CancellationToken token
)
```

#### Parameters

##### *token*
Type: [System.Threading.CancellationToken][2]  
The CancellationToken to observe while waiting

#### Return Value
Type:   
True if the thread method completes before the CancellationToken cancels, otherwise False

See Also
--------

#### Reference
[ThreadSlim Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/dd384802
[3]: README.md