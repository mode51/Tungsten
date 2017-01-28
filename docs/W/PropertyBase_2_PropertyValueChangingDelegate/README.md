PropertyBase&lt;TOwner, TValue>.PropertyValueChangingDelegate Delegate
======================================================================
  
[Missing &lt;summary> documentation for "T:W.PropertyBase`2.PropertyValueChangingDelegate"]


  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate void PropertyValueChangingDelegate(
	TOwner owner,
	TValue oldValue,
	TValue newValue,
	ref bool cancel
)
```

#### Parameters

##### *owner*
Type: [TOwner][2]  


##### *oldValue*
Type: [TValue][2]  


##### *newValue*
Type: [TValue][2]  


##### *cancel*
Type: [System.Boolean][3]  



See Also
--------

#### Reference
[W Namespace][1]  

[1]: ../README.md
[2]: ../PropertyBase_2/README.md
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: ../../_icons/Help.png