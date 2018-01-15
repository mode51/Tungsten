OnValueChangedDelegate&lt;TOwner, TValue> Delegate
==================================================
   Used by the constructor to handle the property change via a callback rather than the events

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate void OnValueChangedDelegate<TOwner, TValue>(
	TOwner owner,
	TValue oldValue,
	TValue newValue
)

```

#### Parameters

##### *owner*
Type: **TOwner**  
The property owner

##### *oldValue*
Type: **TValue**  
The previous value

##### *newValue*
Type: **TValue**  
The new value

#### Type Parameters

##### *TOwner*

[Missing &lt;typeparam name="TOwner"/> documentation for "T:W.OnValueChangedDelegate`2"]


##### *TValue*

[Missing &lt;typeparam name="TValue"/> documentation for "T:W.OnValueChangedDelegate`2"]



See Also
--------

#### Reference
[W Namespace][1]  

[1]: ../README.md