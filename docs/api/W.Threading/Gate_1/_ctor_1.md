Gate&lt;TParameterType> Constructor (Action&lt;TParameterType, CancellationToken>, TParameterType)
==================================================================================================
   Constructs a new Gate

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Gate(
	Action<TParameterType, CancellationToken> action,
	TParameterType defaultArg
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[TParameterType][3], [CancellationToken][4]>  
The Action to call when the gate is opened

##### *defaultArg*
Type: [TParameterType][3]  
A default value which will be passed to the gated Action, if not otherwise specified


See Also
--------

#### Reference
[Gate&lt;TParameterType> Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: README.md
[4]: http://msdn.microsoft.com/en-us/library/dd384802