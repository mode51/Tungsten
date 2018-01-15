Property&lt;TOwner, TValue> Constructor (TOwner, OnValueChangedDelegate&lt;TOwner, TValue>)
===========================================================================================
   Constructs a new Property

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Property(
	TOwner owner,
	OnValueChangedDelegate<TOwner, TValue> onValueChanged = null
)
```

#### Parameters

##### *owner*
Type: [TOwner][2]  
The owner of the property

##### *onValueChanged* (Optional)
Type: [W.OnValueChangedDelegate][3]&lt;[TOwner][2], [TValue][2]>  
A callback for when the property value changes


See Also
--------

#### Reference
[Property&lt;TOwner, TValue> Class][2]  
[W Namespace][1]  

[1]: ../README.md
[2]: README.md
[3]: ../OnValueChangedDelegate_2/README.md