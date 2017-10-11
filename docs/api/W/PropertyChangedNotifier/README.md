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

                    | Name                         | Description                                                                                               
------------------- | ---------------------------- | --------------------------------------------------------------------------------------------------------- 
![Public method]    | [Equals][6]                  | (Inherited from [Object][1].)                                                                             
![Protected method] | [Finalize][7]                | (Inherited from [Object][1].)                                                                             
![Public method]    | [GetHashCode][8]             | (Inherited from [Object][1].)                                                                             
![Public method]    | [GetType][9]                 | (Inherited from [Object][1].)                                                                             
![Protected method] | [GetValue][10]               | 
Override this method to provide Get functionality
                                                     
![Protected method] | [MemberwiseClone][11]        | (Inherited from [Object][1].)                                                                             
![Protected method] | [OnPropertyChanged][12]      | 
Calls RaisePropertyChanged to raise the PropertyChanged event
                                         
![Protected method] | [RaiseOnPropertyChanged][13] | 
Raises the PropertyChanged event
                                                                      
![Protected method] | [SetValue][14]               | 
Calls OnPropertyChanged. This method does not make assignments. Override this method to make assignments.
 
![Public method]    | [ToString][15]               | (Inherited from [Object][1].)                                                                             


Events
------

                | Name                  | Description                    
--------------- | --------------------- | ------------------------------ 
![Public event] | [PropertyChanged][16] | Raised when a property changes 


Extension Methods
-----------------

                                          | Name                                                                                         | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][17]                                                                           | Use Generic syntax for the as operator. (Defined by [AsExtensions][18].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][19]                                                                       | Serializes an object to a Json string (Defined by [AsExtensions][18].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][20]                                                                        | Serializes an object to an xml string (Defined by [AsExtensions][18].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>)][21]          | Overloaded. Creates and starts a new thread and (Defined by [ThreadExtensions][22].)                                                                                                                                             
![Public Extension Method]                | [CreateThread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>, Boolean)][23] | Overloaded. Creates a new thread (Defined by [ThreadExtensions][22].)                                                                                                                                                            
![Public Extension Method]                | [InitializeProperties][24]                                                                   | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][25].) 
![Public Extension Method]                | [IsDirty][26]                                                                                | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][25].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][27]                                                                            | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][25].)                                                                                                  
![Public Extension Method]                | [WaitForValueAsync][28]                                                                      | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][29].)                                                                                                         


See Also
--------

#### Reference
[W Namespace][4]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../PropertyBase_2/README.md
[3]: ../PropertyHostNotifier/README.md
[4]: ../README.md
[5]: _ctor.md
[6]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[7]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[8]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[9]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[10]: GetValue.md
[11]: http://msdn.microsoft.com/en-us/library/57ctke0a
[12]: OnPropertyChanged.md
[13]: RaiseOnPropertyChanged.md
[14]: SetValue.md
[15]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[16]: PropertyChanged.md
[17]: ../AsExtensions/As__1.md
[18]: ../AsExtensions/README.md
[19]: ../AsExtensions/AsJson__1.md
[20]: ../AsExtensions/AsXml__1.md
[21]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[22]: ../../W.Threading/ThreadExtensions/README.md
[23]: ../../W.Threading/ThreadExtensions/CreateThread__1_1.md
[24]: ../PropertyHostMethods/InitializeProperties.md
[25]: ../PropertyHostMethods/README.md
[26]: ../PropertyHostMethods/IsDirty.md
[27]: ../PropertyHostMethods/MarkAsClean.md
[28]: ../ExtensionMethods/WaitForValueAsync.md
[29]: ../ExtensionMethods/README.md
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"