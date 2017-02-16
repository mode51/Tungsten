CallResult&lt;TResult> Class
============================
  
Generic class to be used as a return value. CallResult encapsulates a success/failure, an exception and a return value.



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.CallResult][2]  
    **W.CallResult<TResult>**  

  **Namespace:**  [W][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class CallResult<TResult> : CallResult

```

#### Type Parameters

##### *TResult*
The type to be used for the Result member (the return value of the function)

The **CallResult<TResult>** type exposes the following members.


Constructors
------------

                 | Name                                                     | Description                                                                                            
---------------- | -------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ 
![Public method] | [CallResult&lt;TResult>()][4]                            | Default constructor                                                                                    
![Public method] | [CallResult&lt;TResult>(Boolean)][5]                     | Constructor accepting an initial Success value                                                         
![Public method] | [CallResult&lt;TResult>(Boolean, TResult)][6]            | Constructor accepting an initial Success value and an initial Result value                             
![Public method] | [CallResult&lt;TResult>(Boolean, TResult, Exception)][7] | Constructor accepting an initial Success value, an initial Result value and an initial Exception value 


Properties
----------

                                   | Name        | Description                                                        
---------------------------------- | ----------- | ------------------------------------------------------------------ 
![Public property]![Static member] | [Empty][8]  | Provides a new instance of an uninitialized CallResult&lt;TResult> 
![Public property]                 | [Result][9] | The return value                                                   


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][10]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][11].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][12]     | Serializes an object to a Json string (Defined by [AsExtensions][11].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][13]   | Starts a new thread (Defined by [ThreadExtensions][14].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][15] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][16].) 
![Public Extension Method]                | [IsDirty][17]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][16].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][18]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][16].)                                                                                                  


See Also
--------

#### Reference
[W Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../CallResult/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: _ctor_1.md
[6]: _ctor_2.md
[7]: _ctor_3.md
[8]: Empty.md
[9]: Result.md
[10]: ../AsExtensions/As__1.md
[11]: ../AsExtensions/README.md
[12]: ../AsExtensions/AsJson__1.md
[13]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[14]: ../../W.Threading/ThreadExtensions/README.md
[15]: ../PropertyHostMethods/InitializeProperties.md
[16]: ../PropertyHostMethods/README.md
[17]: ../PropertyHostMethods/IsDirty.md
[18]: ../PropertyHostMethods/MarkAsClean.md
[19]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"