PropertyHost Class
==================
  
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class does not support INotifyPropertyChanged and is not intented to host owned properties (though nothing prevents you from doing so)



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.PropertyHost**  
    [W.Logging.Log.LogMessageHistory][2]  

  **Namespace:**  [W][3]  
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
![Public method] | [PropertyHost][4] | Calls PropertyHostMethods.InitializeProperties so you don't have to 


Properties
----------

                   | Name         | Description                                        
------------------ | ------------ | -------------------------------------------------- 
![Public property] | [IsDirty][5] | Finds all Properties and checks their IsDirty flag 


Methods
-------

                    | Name                  | Description                                                                                 
------------------- | --------------------- | ------------------------------------------------------------------------------------------- 
![Public method]    | [Equals][6]           | (Inherited from [Object][1].)                                                               
![Protected method] | [Finalize][7]         | (Inherited from [Object][1].)                                                               
![Public method]    | [GetHashCode][8]      | (Inherited from [Object][1].)                                                               
![Public method]    | [GetType][9]          | (Inherited from [Object][1].)                                                               
![Public method]    | [MarkAsClean][10]     | Uses reflection to find all Properties and mark them as clean (call Property.MarkAsClean()) 
![Protected method] | [MemberwiseClone][11] | (Inherited from [Object][1].)                                                               
![Public method]    | [ToString][12]        | (Inherited from [Object][1].)                                                               


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][13]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][14].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][15]     | Serializes an object to a Json string (Defined by [AsExtensions][14].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][16]      | Serializes an object to an xml string (Defined by [AsExtensions][14].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][17]   | Starts a new thread (Defined by [ThreadExtensions][18].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][19] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][20].) 
![Public Extension Method]                | [IsDirty][21]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][20].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][22]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][20].)                                                                                                  
![Public Extension Method]                | [WaitForValue][23]         | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][24].)                                                                                                         


See Also
--------

#### Reference
[W Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../../W.Logging/Log_LogMessageHistory/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: IsDirty.md
[6]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[7]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[8]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[9]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[10]: MarkAsClean.md
[11]: http://msdn.microsoft.com/en-us/library/57ctke0a
[12]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[13]: ../AsExtensions/As__1.md
[14]: ../AsExtensions/README.md
[15]: ../AsExtensions/AsJson__1.md
[16]: ../AsExtensions/AsXml__1.md
[17]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[18]: ../../W.Threading/ThreadExtensions/README.md
[19]: ../PropertyHostMethods/InitializeProperties.md
[20]: ../PropertyHostMethods/README.md
[21]: ../PropertyHostMethods/IsDirty.md
[22]: ../PropertyHostMethods/MarkAsClean.md
[23]: ../ExtensionMethods/WaitForValue.md
[24]: ../ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"