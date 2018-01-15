AtomicMethod.Run Method (AtomicMethodDelegate, Object[])
========================================================
   Creates a new AtomicMethod, immediately calls Run and returns the AtomicMethod instance

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static AtomicMethod Run(
	AtomicMethodDelegate delegate,
	params Object[] args
)
```

#### Parameters

##### *delegate*
Type: [W.AtomicMethodDelegate][2]  
The ActionMethodDelegate to be called

##### *args*
Type: [System.Object][3][]  
Arguments to be passed into the AtomicMethodDelegate when called

#### Return Value
Type: [AtomicMethod][4]  
The AtomicMethod instanced created

See Also
--------

#### Reference
[AtomicMethod Class][4]  
[W Namespace][1]  

[1]: ../README.md
[2]: ../AtomicMethodDelegate/README.md
[3]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[4]: README.md