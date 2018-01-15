PropertyValueChangingDelegate&lt;TOwner, TValue> Delegate
=========================================================
   Raised prior to the value of the property changing. Allows the programmer to cancel the change.

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate void PropertyValueChangingDelegate<TOwner, TValue>(
	TOwner owner,
	TValue oldValue,
	TValue newValue,
	ref bool cancel
)

```

#### Parameters

##### *owner*
Type: **TOwner**  
The owner of the property

##### *oldValue*
Type: **TValue**  
The old value

##### *newValue*
Type: **TValue**  
The expected new value

##### *cancel*
Type: [System.Boolean][2]  
Set to True to prevent the property value from changing

#### Type Parameters

##### *TOwner*

[Missing &lt;typeparam name="TOwner"/> documentation for "T:W.PropertyValueChangingDelegate`2"]


##### *TValue*

[Missing &lt;typeparam name="TValue"/> documentation for "T:W.PropertyValueChangingDelegate`2"]



See Also
--------

#### Reference
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/a28wyd50