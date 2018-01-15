AtomicMethod.RunAsync Method (Action)
=====================================
   Creates a new AtomicMethod, immediately calls RunAsync and returns the AtomicMethod instance

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Task<AtomicMethod> RunAsync(
	Action action
)
```

#### Parameters

##### *action*
Type: [System.Action][2]  
The Action to be called

#### Return Value
Type: [Task][3]&lt;[AtomicMethod][4]>  
The AtomicMethod instanced created

See Also
--------

#### Reference
[AtomicMethod Class][4]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb534741
[3]: http://msdn.microsoft.com/en-us/library/dd321424
[4]: README.md