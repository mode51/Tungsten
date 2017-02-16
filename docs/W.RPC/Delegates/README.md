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
----------------------------------------- | ------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][4]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][5].)                                                                                                                                                          
![Public Extension Method]                | [AsJson&lt;TType>][6]     | Serializes an object to a Json string (Defined by [AsExtensions][5].)                                                                                                                                                            
![Public Extension Method]                | [CreateThread&lt;T>][7]   | Starts a new thread (Defined by [ThreadExtensions][8].)                                                                                                                                                                          
![Public Extension Method]                | [InitializeProperties][9] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][10].) 
![Public Extension Method]                | [IsDirty][11]             | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][10].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][12]         | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][10].)                                                                                                  


See Also
--------

#### Reference
[W.RPC Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: ../../W/AsExtensions/As__1.md
[5]: ../../W/AsExtensions/README.md
[6]: ../../W/AsExtensions/AsJson__1.md
[7]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[8]: ../../W.Threading/ThreadExtensions/README.md
[9]: ../../W/PropertyHostMethods/InitializeProperties.md
[10]: ../../W/PropertyHostMethods/README.md
[11]: ../../W/PropertyHostMethods/IsDirty.md
[12]: ../../W/PropertyHostMethods/MarkAsClean.md
[13]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"