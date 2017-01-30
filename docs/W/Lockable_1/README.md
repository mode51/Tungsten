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
-------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][8]    | Starts a new thread (Defined by [ThreadExtensions][9].)                                                                                                                                                                          
![Public Extension Method] | [InitializeProperties][10] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][11].) 
![Public Extension Method] | [IsDirty][12]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][11].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][13]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][11].)                                                                                                  


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
[8]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[9]: ../../W.Threading/ThreadExtensions/README.md
[10]: ../PropertyHostMethods/InitializeProperties.md
[11]: ../PropertyHostMethods/README.md
[12]: ../PropertyHostMethods/IsDirty.md
[13]: ../PropertyHostMethods/MarkAsClean.md
[14]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"