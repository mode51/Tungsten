AtomicMethod.RunAsync Method (AtomicMethodDelegate)
===================================================
   Creates a new AtomicMethod, immediately calls RunAsync and returns the AtomicMethod instance

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Task<AtomicMethod> RunAsync(
	AtomicMethodDelegate delegate
)
```

#### Parameters

##### *delegate*
Type: [W.AtomicMethodDelegate][2]  
The ActionMethodDelegate to be called

#### Return Value
Type: [Task][3]&lt;[AtomicMethod][4]>  
The AtomicMethod instanced created

See Also
--------

#### Reference
[AtomicMethod Class][4]  
[W Namespace][1]  

[1]: ../README.md
[2]: ../AtomicMethodDelegate/README.md
[3]: http://msdn.microsoft.com/en-us/library/dd321424
[4]: README.md