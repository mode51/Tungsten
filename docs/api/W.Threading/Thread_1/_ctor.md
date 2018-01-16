Thread&lt;TType> Constructor
============================
   Constructs a new Thread which can accept a single, typed, paramter

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Thread(
	Action<CancellationToken, TType> action
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[CancellationToken][3], [TType][4]>  
The action to run on a separate thread


See Also
--------

#### Reference
[Thread&lt;TType> Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: http://msdn.microsoft.com/en-us/library/dd384802
[4]: README.md