Thread&lt;TParameterType> Constructor (Action&lt;TParameterType, CancellationToken>, TParameterType)
====================================================================================================
   Constructs a new Thread which can accept a parameter

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Thread(
	Action<TParameterType, CancellationToken> action,
	TParameterType defaultArg
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[TParameterType][3], [CancellationToken][4]>  
The CancellationToken which can be used to cancel the thread

##### *defaultArg*
Type: [TParameterType][3]  
The default argument to pass into the thread procedure


Remarks
-------
Calling this constructor will automatically start the thread

See Also
--------

#### Reference
[Thread&lt;TParameterType> Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: README.md
[4]: http://msdn.microsoft.com/en-us/library/dd384802