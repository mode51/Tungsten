Lockable&lt;TValue> Class
=========================
  
Provides thread safety via locking



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Lockable<TValue>**  

  **Namespace:**  [W][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Lockable<TValue>

```

#### Type Parameters

##### *TValue*

[Missing &lt;typeparam name="TValue"/> documentation for "T:W.Lockable`1"]


The **Lockable<TValue>** type exposes the following members.


Constructors
------------

                 | Name                             | Description                                                    
---------------- | -------------------------------- | -------------------------------------------------------------- 
![Public method] | [Lockable&lt;TValue>()][3]       | 
Constructor which initializes Value with the default of TValue
 
![Public method] | [Lockable&lt;TValue>(TValue)][4] | Constructor which initializes Value with the specified value   


Properties
----------

                   | Name               | Description                                                                 
------------------ | ------------------ | --------------------------------------------------------------------------- 
![Public property] | [LockObject][5]    |                                                                             
![Public property] | [UnlockedValue][6] | 
To be used by caller, with LockObject, to batch read/writes under one lock)
 
![Public property] | [Value][7]         | 
Provides automatic locking during read/writes
                           


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][8]          | Use Generic syntax for the as operator. (Defined by [AsExtensions][9].)                                                                                                                                                          
![Public Extension Method]                | [AsJson&lt;TType>][10]     | Serializes an object to a Json string (Defined by [AsExtensions][9].)                                                                                                                                                            
![Public Extension Method]                | [AsXml&lt;TType>][11]      | Serializes an object to an xml string (Defined by [AsExtensions][9].)                                                                                                                                                            
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
[W Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: _ctor_1.md
[5]: LockObject.md
[6]: UnlockedValue.md
[7]: Value.md
[8]: ../AsExtensions/As__1.md
[9]: ../AsExtensions/README.md
[10]: ../AsExtensions/AsJson__1.md
[11]: ../AsExtensions/AsXml__1.md
[12]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[13]: ../../W.Threading/ThreadExtensions/README.md
[14]: ../PropertyHostMethods/InitializeProperties.md
[15]: ../PropertyHostMethods/README.md
[16]: ../PropertyHostMethods/IsDirty.md
[17]: ../PropertyHostMethods/MarkAsClean.md
[18]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"