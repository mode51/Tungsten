AtomicMethod.RunAsync Method (CancellationToken, Object[])
==========================================================
   Calls the delegate with the specified arguments. These arguments must be accurate in number and in type to the arguments expected by the delegate.

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Task RunAsync(
	CancellationToken token,
	params Object[] args
)
```

#### Parameters

##### *token*
Type: [System.Threading.CancellationToken][2]  
A CancellationToken which can be used to interrupt and stop execution of the delegate

##### *args*
Type: [System.Object][3][]  
An array of arguments to be passed into the delegate

#### Return Value
Type: [Task][4]  
The Task associated with this asynchronous operation

See Also
--------

#### Reference
[AtomicMethod Class][5]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/dd384802
[3]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[4]: http://msdn.microsoft.com/en-us/library/dd235678
[5]: README.md