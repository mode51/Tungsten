PropertyChangedNotifier.SetValue Method
=======================================
   
Calls OnPropertyChanged. This method does not make assignments. Override this method to make assignments.


  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
protected virtual void SetValue(
	Object value,
	string propertyName = ""
)
```

#### Parameters

##### *value*
Type: [System.Object][2]  
The new value

##### *propertyName* (Optional)
Type: [System.String][3]  
The name of the caller (the property being set)


See Also
--------

#### Reference
[PropertyChangedNotifier Class][4]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[4]: README.md