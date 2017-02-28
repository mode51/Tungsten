Constants Class
===============
  
[Missing &lt;summary> documentation for "T:W.RPC.Constants"]



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.RPC.Constants**  

  **Namespace:**  [W.RPC][2]  
  **Assembly:**  Tungsten.RPC.Interfaces (in Tungsten.RPC.Interfaces.dll)

Syntax
------

```csharp
public class Constants
```

The **Constants** type exposes the following members.


Constructors
------------

                 | Name           | Description                                           
---------------- | -------------- | ----------------------------------------------------- 
![Public method] | [Constants][3] | Initializes a new instance of the **Constants** class 


Fields
------

                                | Name                           | Description 
------------------------------- | ------------------------------ | ----------- 
![Public field]![Static member] | [DefaultConnectTimeout][4]     |             
![Public field]![Static member] | [DefaultMakeRPCCallTimeout][5] |             


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
[W.RPC Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: DefaultConnectTimeout.md
[5]: DefaultMakeRPCCallTimeout.md
[6]: ../../W/AsExtensions/As__1.md
[7]: ../../W/AsExtensions/README.md
[8]: ../../W/AsExtensions/AsJson__1.md
[9]: ../../W/AsExtensions/AsXml__1.md
[10]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[11]: ../../W.Threading/ThreadExtensions/README.md
[12]: ../../W/PropertyHostMethods/InitializeProperties.md
[13]: ../../W/PropertyHostMethods/README.md
[14]: ../../W/PropertyHostMethods/IsDirty.md
[15]: ../../W/PropertyHostMethods/MarkAsClean.md
[16]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public field]: ../../_icons/pubfield.gif "Public field"
[Static member]: ../../_icons/static.gif "Static member"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"