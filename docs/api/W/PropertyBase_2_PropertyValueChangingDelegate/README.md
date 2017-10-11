PropertyBase&lt;TOwner, TValue>.PropertyValueChangingDelegate Delegate
======================================================================
   Raised prior to the value of the property changing. Allows the programmer to cancel the change.

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
The owner of the property

##### *oldValue*
Type: [TValue][2]  
The old value

##### *newValue*
Type: [TValue][2]  
The expected new value

##### *cancel*
Type: [System.Boolean][3]  
Set to True to prevent the property value from changing


See Also
--------

#### Reference
[W Namespace][1]  

[1]: ../README.md
[2]: ../PropertyBase_2/README.md
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50