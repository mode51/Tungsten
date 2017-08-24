PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate Delegate
===============================================================
  Used by the constructor to handle the property change via a callback rather than the events

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate void OnValueChangedDelegate(
	TOwner owner,
	TValue oldValue,
	TValue newValue
)
```

#### Parameters

##### *owner*
Type: [TOwner][2]  
The property owner

##### *oldValue*
Type: [TValue][2]  
The previous value

##### *newValue*
Type: [TValue][2]  
The new value


See Also
--------

#### Reference
[W Namespace][1]  

[1]: ../README.md
[2]: ../PropertyBase_2/README.md