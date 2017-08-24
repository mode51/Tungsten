Log.LogMessageHistory Class
===========================
  
[Missing &lt;summary> documentation for "T:W.Logging.Log.LogMessageHistory"]



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.PropertyHost][2]  
    **W.Logging.Log.LogMessageHistory**  

  **Namespace:**  [W.Logging][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class LogMessageHistory : PropertyHost
```

The **Log.LogMessageHistory** type exposes the following members.


Constructors
------------

                 | Name                       | Description                                                       
---------------- | -------------------------- | ----------------------------------------------------------------- 
![Public method] | [Log.LogMessageHistory][4] | Initializes a new instance of the **Log.LogMessageHistory** class 


Properties
----------

                   | Name                         | Description                                                                            
------------------ | ---------------------------- | -------------------------------------------------------------------------------------- 
![Public property] | [Enabled][5]                 |                                                                                        
![Public property] | [IsDirty][6]                 | Finds all Properties and checks their IsDirty flag (Inherited from [PropertyHost][2].) 
![Public property] | [MaximumNumberOfMessages][7] |                                                                                        
![Public property] | [Messages][8]                |                                                                                        


Methods
-------

                    | Name                  | Description                                                                                                                     
------------------- | --------------------- | ------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Equals][9]           | (Inherited from [Object][1].)                                                                                                   
![Protected method] | [Finalize][10]        | (Inherited from [Object][1].)                                                                                                   
![Public method]    | [GetHashCode][11]     | (Inherited from [Object][1].)                                                                                                   
![Public method]    | [GetType][12]         | (Inherited from [Object][1].)                                                                                                   
![Public method]    | [MarkAsClean][13]     | Uses reflection to find all Properties and mark them as clean (call Property.MarkAsClean()) (Inherited from [PropertyHost][2].) 
![Protected method] | [MemberwiseClone][14] | (Inherited from [Object][1].)                                                                                                   
![Public method]    | [ToString][15]        | (Inherited from [Object][1].)                                                                                                   


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][16]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][17].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][18]     | Serializes an object to a Json string (Defined by [AsExtensions][17].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][19]      | Serializes an object to an xml string (Defined by [AsExtensions][17].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][20]   | Starts a new thread (Defined by [ThreadExtensions][21].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][22] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][23].) 
![Public Extension Method]                | [IsDirty][24]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][23].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][25]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][23].)                                                                                                  
![Public Extension Method]                | [WaitForValue][26]         | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][27].)                                                                                                         


See Also
--------

#### Reference
[W.Logging Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../../W/PropertyHost/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: Enabled.md
[6]: ../../W/PropertyHost/IsDirty.md
[7]: MaximumNumberOfMessages.md
[8]: Messages.md
[9]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[10]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[11]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[12]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[13]: ../../W/PropertyHost/MarkAsClean.md
[14]: http://msdn.microsoft.com/en-us/library/57ctke0a
[15]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[16]: ../../W/AsExtensions/As__1.md
[17]: ../../W/AsExtensions/README.md
[18]: ../../W/AsExtensions/AsJson__1.md
[19]: ../../W/AsExtensions/AsXml__1.md
[20]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[21]: ../../W.Threading/ThreadExtensions/README.md
[22]: ../../W/PropertyHostMethods/InitializeProperties.md
[23]: ../../W/PropertyHostMethods/README.md
[24]: ../../W/PropertyHostMethods/IsDirty.md
[25]: ../../W/PropertyHostMethods/MarkAsClean.md
[26]: ../../W/ExtensionMethods/WaitForValue.md
[27]: ../../W/ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"