Gate&lt;T> Class
================
  
A Gated thread. Execution of the Action will proceed when the Run method is called.



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.ThreadBase][2]  
    [W.Threading.Thread][3]  
      [W.Threading.Thread][4]&lt;**T**>  
        **W.Threading.Gate<T>**  

  **Namespace:**  [W.Threading][5]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Gate<T> : Thread<T>

```

#### Type Parameters

##### *T*

[Missing &lt;typeparam name="T"/> documentation for "T:W.Threading.Gate`1"]


The **Gate<T>** type exposes the following members.


Constructors
------------

                 | Name            | Description      
---------------- | --------------- | ---------------- 
![Public method] | [Gate&lt;T>][6] | Construct a Gate 


Methods
-------

                    | Name                  | Description                                                                                                                                      
------------------- | --------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------ 
![Protected method] | [CallInvokeAction][7] | 
Used to wrap the call to InvokeAction with try/catch handlers. This method should call InvokeAction.
 (Overrides [Thread.CallInvokeAction()][8].) 
![Public method]    | [Run][9]              | Allows the Action to be called                                                                                                                   


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][10]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][11].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][12]     | Serializes an object to a Json string (Defined by [AsExtensions][11].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][13]      | Serializes an object to an xml string (Defined by [AsExtensions][11].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][14]   | Starts a new thread (Defined by [ThreadExtensions][15].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][16] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][17].) 
![Public Extension Method]                | [IsDirty][18]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][17].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][19]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][17].)                                                                                                  


See Also
--------

#### Reference
[W.Threading Namespace][5]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../ThreadBase/README.md
[3]: ../Thread/README.md
[4]: ../Thread_1/README.md
[5]: ../README.md
[6]: _ctor.md
[7]: CallInvokeAction.md
[8]: ../Thread/CallInvokeAction.md
[9]: Run.md
[10]: ../../W/AsExtensions/As__1.md
[11]: ../../W/AsExtensions/README.md
[12]: ../../W/AsExtensions/AsJson__1.md
[13]: ../../W/AsExtensions/AsXml__1.md
[14]: ../ThreadExtensions/CreateThread__1.md
[15]: ../ThreadExtensions/README.md
[16]: ../../W/PropertyHostMethods/InitializeProperties.md
[17]: ../../W/PropertyHostMethods/README.md
[18]: ../../W/PropertyHostMethods/IsDirty.md
[19]: ../../W/PropertyHostMethods/MarkAsClean.md
[20]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"