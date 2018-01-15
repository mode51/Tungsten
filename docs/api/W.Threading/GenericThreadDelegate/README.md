GenericThreadDelegate Delegate
==============================
   The delegate, with variable arguments, which is called on a separate thread

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate void GenericThreadDelegate(
	CancellationToken token,
	params Object[] args
)
```

#### Parameters

##### *token*
Type: [System.Threading.CancellationToken][2]  
The CancellationToken used to cancel the thread

##### *args*
Type: [System.Object][3][]  
The arguments to pass into the thread procedure


See Also
--------

#### Reference
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/dd384802
[3]: http://msdn.microsoft.com/en-us/library/e5kfa45b