AtomicMethod.Wait Method (Int32)
================================
   Block the calling thread until the method completes

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public bool Wait(
	int msTimeout
)
```

#### Parameters

##### *msTimeout*
Type: [System.Int32][2]  
The number of milliseconds to block before returning a False value.

#### Return Value
Type: [Boolean][3]  
True if the method completed within the specified timeout period, otherwise False

See Also
--------

#### Reference
[AtomicMethod Class][4]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/td2s409d
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: README.md