PropertyBase&lt;TOwner, TValue>.OnValueChanged Method
=====================================================
   Calls RaiseValueChanged to raise the ValueChanged event

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
protected override void OnValueChanged(
	Object sender,
	TValue oldValue,
	TValue newValue
)
```

#### Parameters

##### *sender*
Type: [System.Object][2]  
The property owner

##### *oldValue*
Type: [TValue][3]  
The previous value

##### *newValue*
Type: [TValue][3]  
The current value


See Also
--------

#### Reference
[PropertyBase&lt;TOwner, TValue> Class][3]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: README.md