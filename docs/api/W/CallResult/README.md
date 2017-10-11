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


Methods
-------

                    | Name                  | Description                   
------------------- | --------------------- | ----------------------------- 
![Public method]    | [Equals][10]          | (Inherited from [Object][1].) 
![Protected method] | [Finalize][11]        | (Inherited from [Object][1].) 
![Public method]    | [GetHashCode][12]     | (Inherited from [Object][1].) 
![Public method]    | [GetType][13]         | (Inherited from [Object][1].) 
![Protected method] | [MemberwiseClone][14] | (Inherited from [Object][1].) 
![Public method]    | [ToString][15]        | (Inherited from [Object][1].) 


Extension Methods
-----------------

                                          | Name                                                                                         | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][16]                                                                           | Use Generic syntax for the as operator. (Defined by [AsExtensions][17].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][18]                                                                       | Serializes an object to a Json string (Defined by [AsExtensions][17].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][19]                                                                        | Serializes an object to an xml string (Defined by [AsExtensions][17].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>)][20]          | Overloaded. Creates and starts a new thread and (Defined by [ThreadExtensions][21].)                                                                                                                                             
![Public Extension Method]                | [CreateThread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>, Boolean)][22] | Overloaded. Creates a new thread (Defined by [ThreadExtensions][21].)                                                                                                                                                            
![Public Extension Method]                | [InitializeProperties][23]                                                                   | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][24].) 
![Public Extension Method]                | [IsDirty][25]                                                                                | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][24].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][26]                                                                            | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][24].)                                                                                                  
![Public Extension Method]                | [WaitForValueAsync][27]                                                                      | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][28].)                                                                                                         


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
[10]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[11]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[12]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[13]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[14]: http://msdn.microsoft.com/en-us/library/57ctke0a
[15]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[16]: ../AsExtensions/As__1.md
[17]: ../AsExtensions/README.md
[18]: ../AsExtensions/AsJson__1.md
[19]: ../AsExtensions/AsXml__1.md
[20]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[21]: ../../W.Threading/ThreadExtensions/README.md
[22]: ../../W.Threading/ThreadExtensions/CreateThread__1_1.md
[23]: ../PropertyHostMethods/InitializeProperties.md
[24]: ../PropertyHostMethods/README.md
[25]: ../PropertyHostMethods/IsDirty.md
[26]: ../PropertyHostMethods/MarkAsClean.md
[27]: ../ExtensionMethods/WaitForValueAsync.md
[28]: ../ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"