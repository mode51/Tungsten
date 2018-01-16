ReaderWriterLocker&lt;TState> Class
===================================
   Extends ReaderWriterLocker with an internal state variable


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.Lockers.StateLocker][2]&lt;[ReaderWriterLocker][3], **TState**>  
    **W.Threading.Lockers.ReaderWriterLocker<TState>**  
      [W.LockableSlim&lt;TValue>][4]  

  **Namespace:**  [W.Threading.Lockers][5]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class ReaderWriterLocker<TState> : StateLocker<ReaderWriterLocker, TState>, 
	IDisposable

```

#### Type Parameters

##### *TState*
The state Type

The **ReaderWriterLocker<TState>** type exposes the following members.


Constructors
------------

                 | Name                               | Description                                                            
---------------- | ---------------------------------- | ---------------------------------------------------------------------- 
![Public method] | [ReaderWriterLocker&lt;TState>][6] | Initializes a new instance of the **ReaderWriterLocker<TState>** class 


Properties
----------

                   | Name        | Description                                                                                                                       
------------------ | ----------- | --------------------------------------------------------------------------------------------------------------------------------- 
![Public property] | [Locker][7] | The locking mechanism (SpinLock, Monitor, SemaphoreSlim, ReaderWriterLock) (Inherited from [StateLocker&lt;TLocker, TState>][2].) 


Methods
-------

                    | Name                                                     | Description                                                                                                                                                                               
------------------- | -------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Dispose][8]                                             | Disposes this object and releases resources                                                                                                                                               
![Public method]    | [Equals][9]                                              | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [Finalize][10]                                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetHashCode][11]                                        | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetType][12]                                            | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [InLock(Action)][13]                                     | Performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                              
![Public method]    | [InLock(Action&lt;TState>)][14]                          | Performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                
![Public method]    | [InLock(Func&lt;TState, TState>)][15]                    | Performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].)                
![Public method]    | [InLock(LockTypeEnum, Action&lt;TState>)][16]            | Performs the action in a lock, passing in the current state                                                                                                                               
![Public method]    | [InLock(LockTypeEnum, Func&lt;TState, TState>)][17]      | Performs the function in a lock, passing in the current state                                                                                                                             
![Public method]    | [InLock&lt;TResult>(Func&lt;TResult>)][18]               | Performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                             
![Public method]    | [InLockAsync(Action)][19]                                | Asynchronously performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                               
![Public method]    | [InLockAsync(Action&lt;TState>)][20]                     | Asynchronously performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                 
![Public method]    | [InLockAsync(Func&lt;TState, TState>)][21]               | Asynchronously performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].) 
![Public method]    | [InLockAsync(LockTypeEnum, Action&lt;TState>)][22]       | Asynchronously performs the action in a lock, passing in the current state                                                                                                                
![Public method]    | [InLockAsync(LockTypeEnum, Func&lt;TState, TState>)][23] | Asynchronously performs the function in a lock, passing in the current state and assigning the state to the result                                                                        
![Public method]    | [InLockAsync&lt;TResult>(Func&lt;TResult>)][24]          | Asynchronously performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                              
![Protected method] | [MemberwiseClone][25]                                    | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [ToString][26]                                           | (Inherited from [Object][1].)                                                                                                                                                             


Fields
------

                   | Name        | Description                                                               
------------------ | ----------- | ------------------------------------------------------------------------- 
![Protected field] | [State][27] | The internal state (Inherited from [StateLocker&lt;TLocker, TState>][2].) 


See Also
--------

#### Reference
[W.Threading.Lockers Namespace][5]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../StateLocker_2/README.md
[3]: ../ReaderWriterLocker/README.md
[4]: ../../W/LockableSlim_1/README.md
[5]: ../README.md
[6]: _ctor.md
[7]: ../StateLocker_2/Locker.md
[8]: Dispose.md
[9]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[10]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[11]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[12]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[13]: ../StateLocker_2/InLock.md
[14]: ../StateLocker_2/InLock_1.md
[15]: ../StateLocker_2/InLock_2.md
[16]: InLock.md
[17]: InLock_1.md
[18]: ../StateLocker_2/InLock__1.md
[19]: ../StateLocker_2/InLockAsync.md
[20]: ../StateLocker_2/InLockAsync_1.md
[21]: ../StateLocker_2/InLockAsync_2.md
[22]: InLockAsync.md
[23]: InLockAsync_1.md
[24]: ../StateLocker_2/InLockAsync__1.md
[25]: http://msdn.microsoft.com/en-us/library/57ctke0a
[26]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[27]: ../StateLocker_2/State.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Protected field]: ../../_icons/protfield.gif "Protected field"