Gate Class
==========
  
A Gated thread. Execution of the Action will proceed when the Run method is called.



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.ThreadBase][2]  
    [W.Threading.Thread][3]  
      **W.Threading.Gate**  

  **Namespace:**  [W.Threading][4]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Gate : Thread
```

The **Gate** type exposes the following members.


Constructors
------------

                 | Name      | Description      
---------------- | --------- | ---------------- 
![Public method] | [Gate][5] | Construct a Gate 


Methods
-------

                    | Name                  | Description                                                                                                                                      
------------------- | --------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------ 
![Protected method] | [CallInvokeAction][6] | 
Used to wrap the call to InvokeAction with try/catch handlers. This method should call InvokeAction.
 (Overrides [Thread.CallInvokeAction()][7].) 
![Public method]    | [Run][8]              | Allows the Action to be called                                                                                                                   


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][9]          | Use Generic syntax for the as operator. (Defined by [AsExtensions][10].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][11]     | Serializes an object to a Json string (Defined by [AsExtensions][10].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][12]   | Starts a new thread (Defined by [ThreadExtensions][13].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][14] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][15].) 
![Public Extension Method]                | [IsDirty][16]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][15].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][17]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][15].)                                                                                                  


See Also
--------

#### Reference
[W.Threading Namespace][4]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../ThreadBase/README.md
[3]: ../Thread/README.md
[4]: ../README.md
[5]: _ctor.md
[6]: CallInvokeAction.md
[7]: ../Thread/CallInvokeAction.md
[8]: Run.md
[9]: ../../W/AsExtensions/As__1.md
[10]: ../../W/AsExtensions/README.md
[11]: ../../W/AsExtensions/AsJson__1.md
[12]: ../ThreadExtensions/CreateThread__1.md
[13]: ../ThreadExtensions/README.md
[14]: ../../W/PropertyHostMethods/InitializeProperties.md
[15]: ../../W/PropertyHostMethods/README.md
[16]: ../../W/PropertyHostMethods/IsDirty.md
[17]: ../../W/PropertyHostMethods/MarkAsClean.md
[18]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"