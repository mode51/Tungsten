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

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][7]          | Use Generic syntax for the as operator. (Defined by [AsExtensions][8].)                                                                                                                                                          
![Public Extension Method]                | [AsJson&lt;TType>][9]      | Serializes an object to a Json string (Defined by [AsExtensions][8].)                                                                                                                                                            
![Public Extension Method]                | [AsXml&lt;TType>][10]      | Serializes an object to an xml string (Defined by [AsExtensions][8].)                                                                                                                                                            
![Public Extension Method]                | [CreateThread&lt;T>][11]   | Starts a new thread (Defined by [ThreadExtensions][12].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][13] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][14].) 
![Public Extension Method]                | [IsDirty][15]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][14].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][16]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][14].)                                                                                                  


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
[7]: ../AsExtensions/As__1.md
[8]: ../AsExtensions/README.md
[9]: ../AsExtensions/AsJson__1.md
[10]: ../AsExtensions/AsXml__1.md
[11]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[12]: ../../W.Threading/ThreadExtensions/README.md
[13]: ../PropertyHostMethods/InitializeProperties.md
[14]: ../PropertyHostMethods/README.md
[15]: ../PropertyHostMethods/IsDirty.md
[16]: ../PropertyHostMethods/MarkAsClean.md
[17]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"