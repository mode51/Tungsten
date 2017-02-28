PropertyChangedNotifier Class
=============================
  
This is a base class for supporting INotifyPropertyChanged



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.PropertyChangedNotifier**  
    [W.PropertyBase&lt;TOwner, TValue>][2]  
    [W.PropertyHostNotifier][3]  

  **Namespace:**  [W][4]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public abstract class PropertyChangedNotifier : INotifyPropertyChanged
```

The **PropertyChangedNotifier** type exposes the following members.


Constructors
------------

                    | Name                         | Description                                                         
------------------- | ---------------------------- | ------------------------------------------------------------------- 
![Protected method] | [PropertyChangedNotifier][5] | Initializes a new instance of the **PropertyChangedNotifier** class 


Methods
-------

                    | Name                        | Description                                                                                               
------------------- | --------------------------- | --------------------------------------------------------------------------------------------------------- 
![Protected method] | [GetValue][6]               | 
Override this method to provide Get functionality
                                                     
![Protected method] | [OnPropertyChanged][7]      | 
Calls RaisePropertyChanged to raise the PropertyChanged event
                                         
![Protected method] | [RaiseOnPropertyChanged][8] | 
Raises the PropertyChanged event
                                                                      
![Protected method] | [SetValue][9]               | 
Calls OnPropertyChanged. This method does not make assignments. Override this method to make assignments.
 


Events
------

                | Name                  | Description 
--------------- | --------------------- | ----------- 
![Public event] | [PropertyChanged][10] |             


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][11]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][12].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][13]     | Serializes an object to a Json string (Defined by [AsExtensions][12].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][14]      | Serializes an object to an xml string (Defined by [AsExtensions][12].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][15]   | Starts a new thread (Defined by [ThreadExtensions][16].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][17] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][18].) 
![Public Extension Method]                | [IsDirty][19]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][18].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][20]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][18].)                                                                                                  


See Also
--------

#### Reference
[W Namespace][4]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../PropertyBase_2/README.md
[3]: ../PropertyHostNotifier/README.md
[4]: ../README.md
[5]: _ctor.md
[6]: GetValue.md
[7]: OnPropertyChanged.md
[8]: RaiseOnPropertyChanged.md
[9]: SetValue.md
[10]: PropertyChanged.md
[11]: ../AsExtensions/As__1.md
[12]: ../AsExtensions/README.md
[13]: ../AsExtensions/AsJson__1.md
[14]: ../AsExtensions/AsXml__1.md
[15]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[16]: ../../W.Threading/ThreadExtensions/README.md
[17]: ../PropertyHostMethods/InitializeProperties.md
[18]: ../PropertyHostMethods/README.md
[19]: ../PropertyHostMethods/IsDirty.md
[20]: ../PropertyHostMethods/MarkAsClean.md
[21]: ../../_icons/Help.png
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"