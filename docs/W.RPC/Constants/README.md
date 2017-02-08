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

                           | Name                      | Description                                                                                                                                                                                                                     
-------------------------- | ------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][6]   | Starts a new thread (Defined by [ThreadExtensions][7].)                                                                                                                                                                         
![Public Extension Method] | [InitializeProperties][8] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][9].) 
![Public Extension Method] | [IsDirty][10]             | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][9].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][11]         | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][9].)                                                                                                  


See Also
--------

#### Reference
[W.RPC Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: DefaultConnectTimeout.md
[5]: DefaultMakeRPCCallTimeout.md
[6]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[7]: ../../W.Threading/ThreadExtensions/README.md
[8]: ../../W/PropertyHostMethods/InitializeProperties.md
[9]: ../../W/PropertyHostMethods/README.md
[10]: ../../W/PropertyHostMethods/IsDirty.md
[11]: ../../W/PropertyHostMethods/MarkAsClean.md
[12]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public field]: ../../_icons/pubfield.gif "Public field"
[Static member]: ../../_icons/static.gif "Static member"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"