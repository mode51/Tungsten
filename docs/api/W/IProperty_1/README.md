IProperty&lt;TValue> Interface
==============================
  The base interface which Property must support

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public interface IProperty<TValue> : IProperty

```

#### Type Parameters

##### *TValue*
The type of value for the property

The **IProperty<TValue>** type exposes the following members.


Properties
----------

                   | Name         | Description                                                                                                                          
------------------ | ------------ | ------------------------------------------------------------------------------------------------------------------------------------ 
![Public property] | [IsDirty][2] | True if the property's value has changed since initialization or since the last call to MarkAsClean (Inherited from [IProperty][3].) 
![Public property] | [Value][4]   | The value of the property                                                                                                            


See Also
--------

#### Reference
[W Namespace][1]  

[1]: ../README.md
[2]: ../IProperty/IsDirty.md
[3]: ../IProperty/README.md
[4]: Value.md
[Public property]: ../../_icons/pubproperty.gif "Public property"