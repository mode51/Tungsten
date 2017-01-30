PropertyHostNotifier Class
==========================
  
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class inherits PropertyChangedNotifier for INotifyPropertyChanged support



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.PropertyChangedNotifier][2]  
    **W.PropertyHostNotifier**  

  **Namespace:**  [W][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class PropertyHostNotifier : PropertyChangedNotifier
```

The **PropertyHostNotifier** type exposes the following members.


Constructors
------------

                 | Name                      | Description                                                         
---------------- | ------------------------- | ------------------------------------------------------------------- 
![Public method] | [PropertyHostNotifier][4] | Calls PropertyHostMethods.InitializeProperties so you don't have to 


Properties
----------

                   | Name         | Description                                        
------------------ | ------------ | -------------------------------------------------- 
![Public property] | [IsDirty][5] | Finds all Properties and checks their IsDirty flag 


Methods
-------

                 | Name             | Description                                                                                 
---------------- | ---------------- | ------------------------------------------------------------------------------------------- 
![Public method] | [MarkAsClean][6] | Uses reflection to find all Properties and mark them as clean (call Property.MarkAsClean()) 


Extension Methods
-----------------

                           | Name                      | Description                                                                                                                                                                                                                      
-------------------------- | ------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][7]   | Starts a new thread (Defined by [ThreadExtensions][8].)                                                                                                                                                                          
![Public Extension Method] | [InitializeProperties][9] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][10].) 
![Public Extension Method] | [IsDirty][11]             | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][10].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][12]         | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][10].)                                                                                                  


See Also
--------

#### Reference
[W Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../PropertyChangedNotifier/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: IsDirty.md
[6]: MarkAsClean.md
[7]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[8]: ../../W.Threading/ThreadExtensions/README.md
[9]: ../PropertyHostMethods/InitializeProperties.md
[10]: ../PropertyHostMethods/README.md
[11]: ../PropertyHostMethods/IsDirty.md
[12]: ../PropertyHostMethods/MarkAsClean.md
[13]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"