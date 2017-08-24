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

                                   | Name           | Description                                                                             
---------------------------------- | -------------- | --------------------------------------------------------------------------------------- 
![Public property]![Static member] | [Empty][8]     | Provides a new instance of an uninitialized CallResult&lt;TResult>                      
![Public property]                 | [Exception][9] | Provide exception data to the caller if desired (Inherited from [CallResult][2].)       
![Public property]                 | [Result][10]   | The return value                                                                        
![Public property]                 | [Success][11]  | Set to True if the function succeeds, otherwise False (Inherited from [CallResult][2].) 


Methods
-------

                    | Name                  | Description                   
------------------- | --------------------- | ----------------------------- 
![Public method]    | [Equals][12]          | (Inherited from [Object][1].) 
![Protected method] | [Finalize][13]        | (Inherited from [Object][1].) 
![Public method]    | [GetHashCode][14]     | (Inherited from [Object][1].) 
![Public method]    | [GetType][15]         | (Inherited from [Object][1].) 
![Protected method] | [MemberwiseClone][16] | (Inherited from [Object][1].) 
![Public method]    | [ToString][17]        | (Inherited from [Object][1].) 


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][18]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][19].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][20]     | Serializes an object to a Json string (Defined by [AsExtensions][19].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][21]      | Serializes an object to an xml string (Defined by [AsExtensions][19].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][22]   | Starts a new thread (Defined by [ThreadExtensions][23].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][24] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][25].) 
![Public Extension Method]                | [IsDirty][26]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][25].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][27]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][25].)                                                                                                  
![Public Extension Method]                | [WaitForValue][28]         | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][29].)                                                                                                         


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
[9]: ../CallResult/Exception.md
[10]: Result.md
[11]: ../CallResult/Success.md
[12]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[13]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[14]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[15]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[16]: http://msdn.microsoft.com/en-us/library/57ctke0a
[17]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[18]: ../AsExtensions/As__1.md
[19]: ../AsExtensions/README.md
[20]: ../AsExtensions/AsJson__1.md
[21]: ../AsExtensions/AsXml__1.md
[22]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[23]: ../../W.Threading/ThreadExtensions/README.md
[24]: ../PropertyHostMethods/InitializeProperties.md
[25]: ../PropertyHostMethods/README.md
[26]: ../PropertyHostMethods/IsDirty.md
[27]: ../PropertyHostMethods/MarkAsClean.md
[28]: ../ExtensionMethods/WaitForValue.md
[29]: ../ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"