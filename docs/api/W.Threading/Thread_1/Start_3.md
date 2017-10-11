Thread&lt;TParameterType>.Start Method (TParameterType, Int32)
==============================================================
   Starts the thread if it's not already running

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public void Start(
	TParameterType arg,
	int msLifetime
)
```

#### Parameters

##### *arg*
Type: [TParameterType][2]  
The argument to pass into the threaded Action

##### *msLifetime*
Type: [System.Int32][3]  
The number of milliseconds before the thread is automatically cancelled. A negative value indicates an infinite lifetime.


See Also
--------

#### Reference
[Thread&lt;TParameterType> Class][2]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: README.md
[3]: http://msdn.microsoft.com/en-us/library/td2s409d