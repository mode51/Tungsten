PropertyHostExtensions Class
============================
   Extension methods related to W.PropertyHost or any class which has multiple W.Property members


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.PropertyHostExtensions.PropertyHostExtensions**  

  **Namespace:**  [W.PropertyHostExtensions][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static class PropertyHostExtensions
```

The **PropertyHostExtensions** type exposes the following members.


Methods
-------

                                 | Name                      | Description                                                                                                                                                                              
-------------------------------- | ------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]![Static member] | [InitializeProperties][3] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 
![Public method]![Static member] | [IsDirty][4]              | 
Scans the IsDirty value of each field and property of type IProperty
                                                                                                                 
![Public method]![Static member] | [MarkAsClean][5]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
                                                                                                  


See Also
--------

#### Reference
[W.PropertyHostExtensions Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: InitializeProperties.md
[4]: IsDirty.md
[5]: MarkAsClean.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"