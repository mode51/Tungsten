ThreadSlim.Wait Method (CancellationToken)
==========================================
   Wait for the thread to complete

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public bool Wait(
	CancellationToken token
)
```

#### Parameters

##### *token*
Type: [System.Threading.CancellationToken][2]  
A CancellationToken to observe while waiting

#### Return Value
Type: [Boolean][3]  
True if the thread completed before the CancellationToken was canceled, otherwise FAlse

See Also
--------

#### Reference
[ThreadSlim Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/dd384802
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: README.md