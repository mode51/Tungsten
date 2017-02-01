Thread&lt;T> Class
==================
  A thread wrapper which makes multi-threading easier


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.ThreadBase][2]  
    [W.Threading.Thread][3]  
      **W.Threading.Thread<T>**  
        [W.Threading.Gate&lt;T>][4]  

  **Namespace:**  [W.Threading][5]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Thread<T> : Thread

```

#### Type Parameters

##### *T*

[Missing &lt;typeparam name="T"/> documentation for "T:W.Threading.Thread`1"]


The **Thread<T>** type exposes the following members.


Constructors
------------

                 | Name              | Description         
---------------- | ----------------- | ------------------- 
![Public method] | [Thread&lt;T>][6] | Starts a new thread 


Properties
----------

                      | Name            | Description 
--------------------- | --------------- | ----------- 
![Protected property] | [Action][7]     |             
![Public property]    | [CustomData][8] |             


Methods
-------

                                 | Name               | Description                                                                                               
-------------------------------- | ------------------ | --------------------------------------------------------------------------------------------------------- 
![Public method]![Static member] | [Create][9]        | Starts a new thread                                                                                       
![Protected method]              | [InvokeAction][10] | Overridden implementation which calls Action with CustomData (Overrides [ThreadBase.InvokeAction()][11].) 


Extension Methods
-----------------

                           | Name                       | Description                                                                                                                                                                                                                      
-------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][12]   | Starts a new thread (Defined by [ThreadExtensions][13].)                                                                                                                                                                         
![Public Extension Method] | [InitializeProperties][14] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][15].) 
![Public Extension Method] | [IsDirty][16]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][15].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][17]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][15].)                                                                                                  


See Also
--------

#### Reference
[W.Threading Namespace][5]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../ThreadBase/README.md
[3]: ../Thread/README.md
[4]: ../Gate_1/README.md
[5]: ../README.md
[6]: _ctor.md
[7]: Action.md
[8]: CustomData.md
[9]: Create.md
[10]: InvokeAction.md
[11]: ../ThreadBase/InvokeAction.md
[12]: ../ThreadExtensions/CreateThread__1.md
[13]: ../ThreadExtensions/README.md
[14]: ../../W/PropertyHostMethods/InitializeProperties.md
[15]: ../../W/PropertyHostMethods/README.md
[16]: ../../W/PropertyHostMethods/IsDirty.md
[17]: ../../W/PropertyHostMethods/MarkAsClean.md
[18]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected property]: ../../_icons/protproperty.gif "Protected property"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"