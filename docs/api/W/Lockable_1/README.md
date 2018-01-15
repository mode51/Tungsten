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
The Type of the lockable value

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
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"