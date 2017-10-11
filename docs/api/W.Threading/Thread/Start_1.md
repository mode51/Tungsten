Thread.Start Method (Int32)
===========================
   Start the thread with a CancellationToken which will timeout in the specified milliseconds

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public virtual void Start(
	int msLifetime
)
```

#### Parameters

##### *msLifetime*
Type: [System.Int32][2]  
The number of milliseconds before the thread is automatically cancelled. A negative value indicates an infinite lifetime.


See Also
--------

#### Reference
[Thread Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/td2s409d
[3]: README.md