ThreadDelegate Delegate
=======================
   Thread delegate used by ThreadSlim

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate void ThreadDelegate(
	CancellationToken token,
	params Object[] args
)
```

#### Parameters

##### *token*
Type: [System.Threading.CancellationToken][2]  
A CancellationToken which can be used to signal the threaded method to stop

##### *args*
Type: [System.Object][3][]  
Arguments to pass into the thread method


See Also
--------

#### Reference
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/dd384802
[3]: http://msdn.microsoft.com/en-us/library/e5kfa45b