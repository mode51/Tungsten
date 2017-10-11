Thread.CallAction Method
========================
   Calls the action encapsulated by this thread. This method can be overridden to provide more specific functionality.

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
protected virtual void CallAction(
	CancellationToken token
)
```

#### Parameters

##### *token*
Type: [System.Threading.CancellationToken][2]  
The CancellationToken, passed into the action, which can be used to cancel the thread Action


See Also
--------

#### Reference
[Thread Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/dd384802
[3]: README.md