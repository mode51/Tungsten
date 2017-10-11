EventTemplate Class
===================
   Wraps the functionality of delegate, event and RaiseXXX into a single class


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.EventTemplate**  

  **Namespace:**  [W][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class EventTemplate
```

The **EventTemplate** type exposes the following members.


Constructors
------------

                 | Name               | Description                                               
---------------- | ------------------ | --------------------------------------------------------- 
![Public method] | [EventTemplate][3] | Initializes a new instance of the **EventTemplate** class 


Methods
-------

                    | Name                 | Description                   
------------------- | -------------------- | ----------------------------- 
![Public method]    | [Equals][4]          | (Inherited from [Object][1].) 
![Protected method] | [Finalize][5]        | (Inherited from [Object][1].) 
![Public method]    | [GetHashCode][6]     | (Inherited from [Object][1].) 
![Public method]    | [GetType][7]         | (Inherited from [Object][1].) 
![Protected method] | [MemberwiseClone][8] | (Inherited from [Object][1].) 
![Public method]    | [Raise][9]           | Raises the template event     
![Public method]    | [ToString][10]       | (Inherited from [Object][1].) 


Events
------

                | Name           | Description        
--------------- | -------------- | ------------------ 
![Public event] | [OnRaised][11] | The template event 


Extension Methods
-----------------

                                          | Name                                                                                         | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][12]                                                                           | Use Generic syntax for the as operator. (Defined by [AsExtensions][13].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][14]                                                                       | Serializes an object to a Json string (Defined by [AsExtensions][13].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][15]                                                                        | Serializes an object to an xml string (Defined by [AsExtensions][13].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>)][16]          | Overloaded. Creates and starts a new thread and (Defined by [ThreadExtensions][17].)                                                                                                                                             
![Public Extension Method]                | [CreateThread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>, Boolean)][18] | Overloaded. Creates a new thread (Defined by [ThreadExtensions][17].)                                                                                                                                                            
![Public Extension Method]                | [InitializeProperties][19]                                                                   | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][20].) 
![Public Extension Method]                | [IsDirty][21]                                                                                | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][20].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][22]                                                                            | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][20].)                                                                                                  
![Public Extension Method]                | [WaitForValueAsync][23]                                                                      | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][24].)                                                                                                         


See Also
--------

#### Reference
[W Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[5]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[6]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[7]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[8]: http://msdn.microsoft.com/en-us/library/57ctke0a
[9]: Raise.md
[10]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[11]: OnRaised.md
[12]: ../AsExtensions/As__1.md
[13]: ../AsExtensions/README.md
[14]: ../AsExtensions/AsJson__1.md
[15]: ../AsExtensions/AsXml__1.md
[16]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[17]: ../../W.Threading/ThreadExtensions/README.md
[18]: ../../W.Threading/ThreadExtensions/CreateThread__1_1.md
[19]: ../PropertyHostMethods/InitializeProperties.md
[20]: ../PropertyHostMethods/README.md
[21]: ../PropertyHostMethods/IsDirty.md
[22]: ../PropertyHostMethods/MarkAsClean.md
[23]: ../ExtensionMethods/WaitForValueAsync.md
[24]: ../ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"