AtomicMethod.Wait Method (CancellationToken)
============================================
   Block the calling thread until the method completes

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public void Wait(
	CancellationToken token
)
```

#### Parameters

##### *token*
Type: [System.Threading.CancellationToken][2]  
A CancellationToken which can be used to stop waiting for the method to complete

#### Return Value
Type:   
True if the method completed before the CancellatioToken was cancelled, otherwise False

See Also
--------

#### Reference
[AtomicMethod Class][3]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/dd384802
[3]: README.md