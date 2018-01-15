AtomicMethod.RunAsync Method (Object[])
=======================================
   Asynchronously calls the delegate with the specified arguments. These arguments must be accurate in number and in type to the arguments expected by the delegate.

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Task RunAsync(
	params Object[] args
)
```

#### Parameters

##### *args*
Type: [System.Object][2][]  
An array of arguments to be passed into the delegate

#### Return Value
Type: [Task][3]  
The Task associated with this asynchronous operation

See Also
--------

#### Reference
[AtomicMethod Class][4]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/dd235678
[4]: README.md