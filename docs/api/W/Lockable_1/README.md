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
![Public property] | [LockObject][5]    | The object used internally for lock statements                              
![Public property] | [UnlockedValue][6] | 
To be used by caller, with LockObject, to batch read/writes under one lock)
 
![Public property] | [Value][7]         | 
Provides automatic locking during read/writes
                           


Methods
-------

                    | Name                                              | Description                                        
------------------- | ------------------------------------------------- | -------------------------------------------------- 
![Public method]    | [Equals][8]                                       | (Inherited from [Object][1].)                      
![Public method]    | [ExecuteInLock(Action&lt;TValue>)][9]             | Executes an action within a lock of the LockObject 
![Public method]    | [ExecuteInLock(Func&lt;TValue, TValue>)][10]      | Executes an action within a lock of the LockObject 
![Public method]    | [ExecuteInLockAsync(Action&lt;TValue>)][11]       | Executes a task within a lock of the LockObject    
![Public method]    | [ExecuteInLockAsync(Func&lt;TValue, TValue>)][12] | Executes a task within a lock of the LockObject    
![Protected method] | [Finalize][13]                                    | (Inherited from [Object][1].)                      
![Public method]    | [GetHashCode][14]                                 | (Inherited from [Object][1].)                      
![Public method]    | [GetType][15]                                     | (Inherited from [Object][1].)                      
![Protected method] | [MemberwiseClone][16]                             | (Inherited from [Object][1].)                      
![Public method]    | [ToString][17]                                    | (Inherited from [Object][1].)                      


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
[8]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[9]: ExecuteInLock.md
[10]: ExecuteInLock_1.md
[11]: ExecuteInLockAsync.md
[12]: ExecuteInLockAsync_1.md
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
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"