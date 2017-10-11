Thread&lt;TParameterType> Constructor (Action&lt;TParameterType, CancellationToken>, Boolean)
=============================================================================================
   Constructs a new Thread which can accept a parameter

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Thread(
	Action<TParameterType, CancellationToken> action,
	bool autoStart
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[TParameterType][3], [CancellationToken][4]>  
The CancellationToken which can be used to cancel the thread

##### *autoStart*
Type: [System.Boolean][5]  
If True, the Thread will immediately start, otherwise the Start method will have to be called manually


See Also
--------

#### Reference
[Thread&lt;TParameterType> Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: README.md
[4]: http://msdn.microsoft.com/en-us/library/dd384802
[5]: http://msdn.microsoft.com/en-us/library/a28wyd50