CustomLogger Class
==================
  Allows the programmer to add a custom message logger


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Logging.CustomLogger**  

  **Namespace:**  [W.Logging][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class CustomLogger : IDisposable
```

The **CustomLogger** type exposes the following members.


Constructors
------------

                 | Name              | Description                   
---------------- | ----------------- | ----------------------------- 
![Public method] | [CustomLogger][3] | Constructs a new CustomLogger 


Properties
----------

                      | Name              | Description                                                                           
--------------------- | ----------------- | ------------------------------------------------------------------------------------- 
![Public property]    | [AddTimestamp][4] | If true, FormatLogMessage will, by default, add a timestamp prefix to the log message 
![Protected property] | [IsDisposed][5]   | True if OnDispose has been called                                                     
![Public property]    | [Name][6]         | The name of this custom logger                                                        


Methods
-------

                    | Name                   | Description                                                                                
------------------- | ---------------------- | ------------------------------------------------------------------------------------------ 
![Public method]    | [Dispose][7]           | Disposes the CustomLogger and releases resources                                           
![Public method]    | [Equals][8]            | (Inherited from [Object][1].)                                                              
![Protected method] | [Finalize][9]          | (Inherited from [Object][1].)                                                              
![Protected method] | [FormatLogMessage][10] | Formats the Log Messge (if AddTimestamp is true, the message is prefixed with a timestamp) 
![Public method]    | [GetHashCode][11]      | (Inherited from [Object][1].)                                                              
![Public method]    | [GetType][12]          | (Inherited from [Object][1].)                                                              
![Protected method] | [LogMessage][13]       | Log a message to the custom logger                                                         
![Protected method] | [MemberwiseClone][14]  | (Inherited from [Object][1].)                                                              
![Protected method] | [OnDispose][15]        | Disposes the CustomLogger, releases resources and supresses the finalizer                  
![Public method]    | [ToString][16]         | (Inherited from [Object][1].)                                                              


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][17]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][18].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][19]     | Serializes an object to a Json string (Defined by [AsExtensions][18].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][20]      | Serializes an object to an xml string (Defined by [AsExtensions][18].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][21]   | Starts a new thread (Defined by [ThreadExtensions][22].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][23] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][24].) 
![Public Extension Method]                | [IsDirty][25]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][24].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][26]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][24].)                                                                                                  


See Also
--------

#### Reference
[W.Logging Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: AddTimestamp.md
[5]: IsDisposed.md
[6]: Name.md
[7]: Dispose.md
[8]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[9]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[10]: FormatLogMessage.md
[11]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[12]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[13]: LogMessage.md
[14]: http://msdn.microsoft.com/en-us/library/57ctke0a
[15]: OnDispose.md
[16]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[17]: ../../W/AsExtensions/As__1.md
[18]: ../../W/AsExtensions/README.md
[19]: ../../W/AsExtensions/AsJson__1.md
[20]: ../../W/AsExtensions/AsXml__1.md
[21]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[22]: ../../W.Threading/ThreadExtensions/README.md
[23]: ../../W/PropertyHostMethods/InitializeProperties.md
[24]: ../../W/PropertyHostMethods/README.md
[25]: ../../W/PropertyHostMethods/IsDirty.md
[26]: ../../W/PropertyHostMethods/MarkAsClean.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected property]: ../../_icons/protproperty.gif "Protected property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"