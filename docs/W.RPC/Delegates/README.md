Delegates Class
===============
  
[Missing &lt;summary> documentation for "T:W.RPC.Delegates"]



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.RPC.Delegates**  

  **Namespace:**  [W.RPC][2]  
  **Assembly:**  Tungsten.RPC.Interfaces (in Tungsten.RPC.Interfaces.dll)

Syntax
------

```csharp
public class Delegates
```

The **Delegates** type exposes the following members.


Constructors
------------

                 | Name           | Description                                           
---------------- | -------------- | ----------------------------------------------------- 
![Public method] | [Delegates][3] | Initializes a new instance of the **Delegates** class 


Extension Methods
-----------------

                           | Name                      | Description                                                                                                                                                                                                                     
-------------------------- | ------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][4]   | Starts a new thread (Defined by [ThreadExtensions][5].)                                                                                                                                                                         
![Public Extension Method] | [InitializeProperties][6] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][7].) 
![Public Extension Method] | [IsDirty][8]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][7].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][9]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][7].)                                                                                                  


See Also
--------

#### Reference
[W.RPC Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[5]: ../../W.Threading/ThreadExtensions/README.md
[6]: ../../W/PropertyHostMethods/InitializeProperties.md
[7]: ../../W/PropertyHostMethods/README.md
[8]: ../../W/PropertyHostMethods/IsDirty.md
[9]: ../../W/PropertyHostMethods/MarkAsClean.md
[10]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"