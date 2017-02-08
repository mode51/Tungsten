PropertyBase&lt;TOwner, TValue>.RaisePropertyValueChanging Method
=================================================================
  Raises the ValueChanging event

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
protected virtual void RaisePropertyValueChanging(
	TValue oldValue,
	TValue newValue,
	ref bool cancel
)
```

#### Parameters

##### *oldValue*
Type: [TValue][2]  
The old property value

##### *newValue*
Type: [TValue][2]  
The expected new property value

##### *cancel*
Type: [System.Boolean][3]  
Set to True to cancel the property change


See Also
--------

#### Reference
[PropertyBase&lt;TOwner, TValue> Class][2]  
[W Namespace][1]  

[1]: ../README.md
[2]: README.md
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: ../../_icons/Help.png