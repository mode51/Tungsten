PropertyHost Class
==================
  
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class does not support INotifyPropertyChanged and is not intented to host owned properties (though nothing prevents you from doing so)



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.PropertyHost**  

  **Namespace:**  [W][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class PropertyHost
```

The **PropertyHost** type exposes the following members.


Constructors
------------

                 | Name              | Description                                                         
---------------- | ----------------- | ------------------------------------------------------------------- 
![Public method] | [PropertyHost][3] | Calls PropertyHostMethods.InitializeProperties so you don't have to 


Properties
----------

                   | Name         | Description                                        
------------------ | ------------ | -------------------------------------------------- 
![Public property] | [IsDirty][4] | Finds all Properties and checks their IsDirty flag 


Methods
-------

                 | Name             | Description                                                                                 
---------------- | ---------------- | ------------------------------------------------------------------------------------------- 
![Public method] | [MarkAsClean][5] | Uses reflection to find all Properties and mark them as clean (call Property.MarkAsClean()) 


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][6]          | Use Generic syntax for the as operator. (Defined by [AsExtensions][7].)                                                                                                                                                          
![Public Extension Method]                | [AsJson&lt;TType>][8]      | Serializes an object to a Json string (Defined by [AsExtensions][7].)                                                                                                                                                            
![Public Extension Method]                | [AsXml&lt;TType>][9]       | Serializes an object to an xml string (Defined by [AsExtensions][7].)                                                                                                                                                            
![Public Extension Method]                | [CreateThread&lt;T>][10]   | Starts a new thread (Defined by [ThreadExtensions][11].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][12] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][13].) 
![Public Extension Method]                | [IsDirty][14]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][13].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][15]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][13].)                                                                                                  


See Also
--------

#### Reference
[W Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: IsDirty.md
[5]: MarkAsClean.md
[6]: ../AsExtensions/As__1.md
[7]: ../AsExtensions/README.md
[8]: ../AsExtensions/AsJson__1.md
[9]: ../AsExtensions/AsXml__1.md
[10]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[11]: ../../W.Threading/ThreadExtensions/README.md
[12]: ../PropertyHostMethods/InitializeProperties.md
[13]: ../PropertyHostMethods/README.md
[14]: ../PropertyHostMethods/IsDirty.md
[15]: ../PropertyHostMethods/MarkAsClean.md
[16]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"