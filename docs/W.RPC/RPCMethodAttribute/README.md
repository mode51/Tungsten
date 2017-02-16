RPCMethodAttribute Class
========================
  
[Missing &lt;summary> documentation for "T:W.RPC.RPCMethodAttribute"]



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [System.Attribute][2]  
    **W.RPC.RPCMethodAttribute**  

  **Namespace:**  [W.RPC][3]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public class RPCMethodAttribute : Attribute
```

The **RPCMethodAttribute** type exposes the following members.


Constructors
------------

                 | Name                    | Description                                                    
---------------- | ----------------------- | -------------------------------------------------------------- 
![Public method] | [RPCMethodAttribute][4] | Initializes a new instance of the **RPCMethodAttribute** class 


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][5]          | Use Generic syntax for the as operator. (Defined by [AsExtensions][6].)                                                                                                                                                          
![Public Extension Method]                | [AsJson&lt;TType>][7]      | Serializes an object to a Json string (Defined by [AsExtensions][6].)                                                                                                                                                            
![Public Extension Method]                | [CreateThread&lt;T>][8]    | Starts a new thread (Defined by [ThreadExtensions][9].)                                                                                                                                                                          
![Public Extension Method]                | [InitializeProperties][10] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][11].) 
![Public Extension Method]                | [IsDirty][12]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][11].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][13]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][11].)                                                                                                  


See Also
--------

#### Reference
[W.RPC Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: http://msdn.microsoft.com/en-us/library/e8kc3626
[3]: ../README.md
[4]: _ctor.md
[5]: ../../W/AsExtensions/As__1.md
[6]: ../../W/AsExtensions/README.md
[7]: ../../W/AsExtensions/AsJson__1.md
[8]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[9]: ../../W.Threading/ThreadExtensions/README.md
[10]: ../../W/PropertyHostMethods/InitializeProperties.md
[11]: ../../W/PropertyHostMethods/README.md
[12]: ../../W/PropertyHostMethods/IsDirty.md
[13]: ../../W/PropertyHostMethods/MarkAsClean.md
[14]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"