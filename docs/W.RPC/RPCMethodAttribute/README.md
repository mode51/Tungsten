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

                           | Name                      | Description                                                                                                                                                                                                                     
-------------------------- | ------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][5]   | Starts a new thread (Defined by [ThreadExtensions][6].)                                                                                                                                                                         
![Public Extension Method] | [InitializeProperties][7] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][8].) 
![Public Extension Method] | [IsDirty][9]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][8].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][10]         | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][8].)                                                                                                  


See Also
--------

#### Reference
[W.RPC Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: http://msdn.microsoft.com/en-us/library/e8kc3626
[3]: ../README.md
[4]: _ctor.md
[5]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[6]: ../../W.Threading/ThreadExtensions/README.md
[7]: ../../W/PropertyHostMethods/InitializeProperties.md
[8]: ../../W/PropertyHostMethods/README.md
[9]: ../../W/PropertyHostMethods/IsDirty.md
[10]: ../../W/PropertyHostMethods/MarkAsClean.md
[11]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"