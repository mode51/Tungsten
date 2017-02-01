CallResult Class
================
  A non-generic return value for a function. CallResult encapsulates a success/failure and an exception.


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.CallResult**  
    [W.CallResult&lt;TResult>][2]  

  **Namespace:**  [W][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class CallResult
```

The **CallResult** type exposes the following members.


Constructors
------------

                 | Name                                | Description                                                                               
---------------- | ----------------------------------- | ----------------------------------------------------------------------------------------- 
![Public method] | [CallResult()][4]                   | Default constructor, initializes Success to false                                         
![Public method] | [CallResult(Boolean)][5]            | Constructor which accepts an initial value for Success                                    
![Public method] | [CallResult(Boolean, Exception)][6] | Constructor which accepts an initial value for Success and an initial value for Exception 


Properties
----------

                                   | Name           | Description                                            
---------------------------------- | -------------- | ------------------------------------------------------ 
![Public property]![Static member] | [Empty][7]     | Provides a new instance of an uninitialized CallResult 
![Public property]                 | [Exception][8] | Provide exception data to the caller if desired        
![Public property]                 | [Success][9]   | Set to True if the function succeeds, otherwise False  


Extension Methods
-----------------

                           | Name                       | Description                                                                                                                                                                                                                      
-------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][10]   | Starts a new thread (Defined by [ThreadExtensions][11].)                                                                                                                                                                         
![Public Extension Method] | [InitializeProperties][12] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][13].) 
![Public Extension Method] | [IsDirty][14]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][13].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][15]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][13].)                                                                                                  


See Also
--------

#### Reference
[W Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../CallResult_1/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: _ctor_1.md
[6]: _ctor_2.md
[7]: Empty.md
[8]: Exception.md
[9]: Success.md
[10]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[11]: ../../W.Threading/ThreadExtensions/README.md
[12]: ../PropertyHostMethods/InitializeProperties.md
[13]: ../PropertyHostMethods/README.md
[14]: ../PropertyHostMethods/IsDirty.md
[15]: ../PropertyHostMethods/MarkAsClean.md
[16]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"