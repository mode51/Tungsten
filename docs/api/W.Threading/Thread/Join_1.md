Thread.Join Method (Int32)
==========================
   Block the calling thread until this thread object has completed or until the timeout has occurred

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public bool Join(
	int msTimeout
)
```

#### Parameters

##### *msTimeout*
Type: [System.Int32][2]  
The number of milliseconds to wait, for the thread Action to complete, before timing out

#### Return Value
Type: [Boolean][3]  
True if the thread Action completed within the specified timeout, otherwise False

See Also
--------

#### Reference
[Thread Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/td2s409d
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: README.md