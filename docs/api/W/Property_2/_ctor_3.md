Property&lt;TOwner, TValue> Constructor (TOwner, TValue, PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate)
================================================================================================================
   Constructs a new Property

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Property(
	TOwner owner,
	TValue defaultValue,
	PropertyBase<TOwner, TValue>.OnValueChangedDelegate onValueChanged
)
```

#### Parameters

##### *owner*
Type: [TOwner][2]  
The owner of the property

##### *defaultValue*
Type: [TValue][2]  
The default and initial value of the property

##### *onValueChanged*
Type: [W.PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate][3]  
A callback for when the property value changes


See Also
--------

#### Reference
[Property&lt;TOwner, TValue> Class][2]  
[W Namespace][1]  

[1]: ../README.md
[2]: README.md
[3]: ../PropertyBase_2_OnValueChangedDelegate/README.md