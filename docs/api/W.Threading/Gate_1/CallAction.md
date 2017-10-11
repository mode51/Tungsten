Gate&lt;TParameterType>.CallAction Method
=========================================
   Invokes the gated Action

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
protected override void CallAction(
	CancellationToken token
)
```

#### Parameters

##### *token*
Type: [System.Threading.CancellationToken][2]  
The CancellationToken which is passed into the Action and should be used to monitor whether the Gate has been cancelled


See Also
--------

#### Reference
[Gate&lt;TParameterType> Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/dd384802
[3]: README.md