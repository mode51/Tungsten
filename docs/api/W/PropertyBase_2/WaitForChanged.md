PropertyBase&lt;TOwner, TValue>.WaitForChanged Method
=====================================================
   Allows the caller to suspend it's thread until Value changes

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public bool WaitForChanged(
	int msTimeout = 0
)
```

#### Parameters

##### *msTimeout* (Optional)
Type: [System.Int32][2]  
The number of milliseconds to wait for the value to change

#### Return Value
Type: [Boolean][3]  
True if the value changed within the specified timeout period, otherwise False

See Also
--------

#### Reference
[PropertyBase&lt;TOwner, TValue> Class][4]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/td2s409d
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: README.md