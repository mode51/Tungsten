ParameterizedThread.Start Method (Int32, Object[])
==================================================
   Start the thread with a CancellationToken which will timeout in the specified milliseconds

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Task Start(
	int msLifetime,
	params Object[] args
)
```

#### Parameters

##### *msLifetime*
Type: [System.Int32][2]  
The number of milliseconds before the thread is automatically cancelled. A negative value indicates an infinite lifetime.

##### *args*
Type: [System.Object][3][]  
The arguments to pass into the thread procedure

#### Return Value
Type: [Task][4]  

[Missing &lt;returns> documentation for "M:W.Threading.ParameterizedThread.Start(System.Int32,System.Object[])"]


See Also
--------

#### Reference
[ParameterizedThread Class][5]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/td2s409d
[3]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[4]: http://msdn.microsoft.com/en-us/library/dd235678
[5]: README.md