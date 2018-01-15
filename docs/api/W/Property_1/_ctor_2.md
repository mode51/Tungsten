Property&lt;TValue> Constructor (TValue, OnValueChangedDelegate&lt;Property&lt;TValue>, TValue>)
================================================================================================
   Constructs a new Property

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Property(
	TValue defaultValue,
	OnValueChangedDelegate<Property<TValue>, TValue> onValueChanged
)
```

#### Parameters

##### *defaultValue*
Type: [TValue][2]  
The default and initial value of the property value

##### *onValueChanged*
Type: [W.OnValueChangedDelegate][3]&lt;[Property][2]&lt;[TValue][2]>, [TValue][2]>  
A callback for when the property value changes


See Also
--------

#### Reference
[Property&lt;TValue> Class][2]  
[W Namespace][1]  

[1]: ../README.md
[2]: README.md
[3]: ../OnValueChangedDelegate_2/README.md