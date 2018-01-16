LockableSlim&lt;TValue> Class
=============================
   Uses ReaderWriterLock to provide thread-safe access to an underlying value


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.Lockers.StateLocker][2]&lt;[ReaderWriterLocker][3], **TValue**>  
    [W.Threading.Lockers.ReaderWriterLocker][4]&lt;**TValue**>  
      **W.LockableSlim<TValue>**  
        [W.Lockable&lt;TValue>][5]  

  **Namespace:**  [W][6]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class LockableSlim<TValue> : ReaderWriterLocker<TValue>

```

#### Type Parameters

##### *TValue*
The Type of value

The **LockableSlim<TValue>** type exposes the following members.


Constructors
------------

                 | Name                                 | Description                                                
---------------- | ------------------------------------ | ---------------------------------------------------------- 
![Public method] | [LockableSlim&lt;TValue>()][7]       | Constructs a new LockableSlim with a default initial value 
![Public method] | [LockableSlim&lt;TValue>(TValue)][8] | Constructs a new LockableSlim assigning an initial value   


Properties
----------

                   | Name        | Description                                                                                                                       
------------------ | ----------- | --------------------------------------------------------------------------------------------------------------------------------- 
![Public property] | [Locker][9] | The locking mechanism (SpinLock, Monitor, SemaphoreSlim, ReaderWriterLock) (Inherited from [StateLocker&lt;TLocker, TState>][2].) 
![Public property] | [Value][10] | Get or Set the value                                                                                                              


Methods
-------

                    | Name                                                     | Description                                                                                                                                                                               
------------------- | -------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Dispose][11]                                            | Disposes this object and releases resources (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                                          
![Public method]    | [Equals][12]                                             | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [Finalize][13]                                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetHashCode][14]                                        | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetType][15]                                            | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [GetValue][16]                                           | Gets the underlying value                                                                                                                                                                 
![Public method]    | [InLock(Action)][17]                                     | Performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                              
![Public method]    | [InLock(Action&lt;TState>)][18]                          | Performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                
![Public method]    | [InLock(Func&lt;TState, TState>)][19]                    | Performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].)                
![Public method]    | [InLock(LockTypeEnum, Action&lt;TState>)][20]            | Performs the action in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                          
![Public method]    | [InLock(LockTypeEnum, Func&lt;TState, TState>)][21]      | Performs the function in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                        
![Public method]    | [InLock&lt;TResult>(Func&lt;TResult>)][22]               | Performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                             
![Public method]    | [InLockAsync(Action)][23]                                | Asynchronously performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                               
![Public method]    | [InLockAsync(Action&lt;TState>)][24]                     | Asynchronously performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                 
![Public method]    | [InLockAsync(Func&lt;TState, TState>)][25]               | Asynchronously performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].) 
![Public method]    | [InLockAsync(LockTypeEnum, Action&lt;TState>)][26]       | Asynchronously performs the action in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                           
![Public method]    | [InLockAsync(LockTypeEnum, Func&lt;TState, TState>)][27] | Asynchronously performs the function in a lock, passing in the current state and assigning the state to the result (Inherited from [ReaderWriterLocker&lt;TState>][4].)                   
![Public method]    | [InLockAsync&lt;TResult>(Func&lt;TResult>)][28]          | Asynchronously performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                              
![Protected method] | [MemberwiseClone][29]                                    | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [SetValue][30]                                           | Sets the underlying value                                                                                                                                                                 
![Public method]    | [ToString][31]                                           | (Inherited from [Object][1].)                                                                                                                                                             


Fields
------

                   | Name        | Description                                                               
------------------ | ----------- | ------------------------------------------------------------------------- 
![Protected field] | [State][32] | The internal state (Inherited from [StateLocker&lt;TLocker, TState>][2].) 


Remarks
-------
Can be overridden to provide additional functionality

See Also
--------

#### Reference
[W Namespace][6]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../../W.Threading.Lockers/StateLocker_2/README.md
[3]: ../../W.Threading.Lockers/ReaderWriterLocker/README.md
[4]: ../../W.Threading.Lockers/ReaderWriterLocker_1/README.md
[5]: ../Lockable_1/README.md
[6]: ../README.md
[7]: _ctor.md
[8]: _ctor_1.md
[9]: ../../W.Threading.Lockers/StateLocker_2/Locker.md
[10]: Value.md
[11]: ../../W.Threading.Lockers/ReaderWriterLocker_1/Dispose.md
[12]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[13]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[14]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[15]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[16]: GetValue.md
[17]: ../../W.Threading.Lockers/StateLocker_2/InLock.md
[18]: ../../W.Threading.Lockers/StateLocker_2/InLock_1.md
[19]: ../../W.Threading.Lockers/StateLocker_2/InLock_2.md
[20]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLock.md
[21]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLock_1.md
[22]: ../../W.Threading.Lockers/StateLocker_2/InLock__1.md
[23]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync.md
[24]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync_1.md
[25]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync_2.md
[26]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLockAsync.md
[27]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLockAsync_1.md
[28]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync__1.md
[29]: http://msdn.microsoft.com/en-us/library/57ctke0a
[30]: SetValue.md
[31]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[32]: ../../W.Threading.Lockers/StateLocker_2/State.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Protected field]: ../../_icons/protfield.gif "Protected field"