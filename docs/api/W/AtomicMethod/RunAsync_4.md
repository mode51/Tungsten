AtomicMethod.RunAsync Method (AtomicMethodDelegate, Object[])
=============================================================
   Creates a new AtomicMethod, immediately calls RunAsync and returns the AtomicMethod instance

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Task<AtomicMethod> RunAsync(
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
Type: [Task][4]&lt;[AtomicMethod][5]>  
The AtomicMethod instanced created

See Also
--------

#### Reference
[AtomicMethod Class][5]  
[W Namespace][1]  

[1]: ../README.md
[2]: ../AtomicMethodDelegate/README.md
[3]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[4]: http://msdn.microsoft.com/en-us/library/dd321424
[5]: README.md