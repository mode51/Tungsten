ThreadMethod.Wait Method (CancellationToken)
============================================
   Waits for the thread method to complete

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
The CancellationToken to observe while waiting

#### Return Value
Type: [Boolean][3]  
True if the thread method completes before the CancellationToken is cancelled, otherwise False

See Also
--------

#### Reference
[ThreadMethod Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/dd384802
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: README.md