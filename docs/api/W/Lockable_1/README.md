Lockable&lt;TValue> Class
=========================
   
Extends LockableSlim with ValueChangedDelegate notification



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.Lockers.StateLocker][2]&lt;[ReaderWriterLocker][3], **TValue**>  
    [W.Threading.Lockers.ReaderWriterLocker][4]&lt;**TValue**>  
      [W.LockableSlim][5]&lt;**TValue**>  
        **W.Lockable<TValue>**  
          [W.PropertySlim&lt;TValue>][6]  

  **Namespace:**  [W][7]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Lockable<TValue> : LockableSlim<TValue>, 
	IDisposable

```

#### Type Parameters

##### *TValue*
The data Type to be used

The **Lockable<TValue>** type exposes the following members.


Constructors
------------

                 | Name                                                                 | Description                          
---------------- | -------------------------------------------------------------------- | ------------------------------------ 
![Public method] | [Lockable&lt;TValue>()][8]                                           | Constructs a new Lockable&lt;TValue> 
![Public method] | [Lockable&lt;TValue>(Action&lt;Object, TValue, TValue>)][9]          | Constructs a new Lockable&lt;TValue> 
![Public method] | [Lockable&lt;TValue>(TValue)][10]                                    | Constructs a new Lockable&lt;TValue> 
![Public method] | [Lockable&lt;TValue>(TValue, Action&lt;Object, TValue, TValue>)][11] | Constructs a new Lockable&lt;TValue> 


Properties
----------

                   | Name         | Description                                                                                                                       
------------------ | ------------ | --------------------------------------------------------------------------------------------------------------------------------- 
![Public property] | [Locker][12] | The locking mechanism (SpinLock, Monitor, SemaphoreSlim, ReaderWriterLock) (Inherited from [StateLocker&lt;TLocker, TState>][2].) 
![Public property] | [Value][13]  | Get or Set the value (Inherited from [LockableSlim&lt;TValue>][5].)                                                               


Methods
-------

                    | Name                                                     | Description                                                                                                                                                                               
------------------- | -------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Dispose][14]                                            | Disposes the Lockable and releases resources                                                                                                                                              
![Public method]    | [Equals][15]                                             | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [Finalize][16]                                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetHashCode][17]                                        | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetType][18]                                            | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [GetValue][19]                                           | Gets the underlying value (Inherited from [LockableSlim&lt;TValue>][5].)                                                                                                                  
![Protected method] | [InformWaiters][20]                                      | Informs those who are waiting on WaitForChanged that the value has changed                                                                                                                
![Public method]    | [InLock(Action)][21]                                     | Performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                              
![Public method]    | [InLock(Action&lt;TState>)][22]                          | Performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                
![Public method]    | [InLock(Func&lt;TState, TState>)][23]                    | Performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].)                
![Public method]    | [InLock(LockTypeEnum, Action&lt;TState>)][24]            | Performs the action in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                          
![Public method]    | [InLock(LockTypeEnum, Func&lt;TState, TState>)][25]      | Performs the function in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                        
![Public method]    | [InLock&lt;TResult>(Func&lt;TResult>)][26]               | Performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                             
![Public method]    | [InLockAsync(Action)][27]                                | Asynchronously performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                               
![Public method]    | [InLockAsync(Action&lt;TState>)][28]                     | Asynchronously performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                 
![Public method]    | [InLockAsync(Func&lt;TState, TState>)][29]               | Asynchronously performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].) 
![Public method]    | [InLockAsync(LockTypeEnum, Action&lt;TState>)][30]       | Asynchronously performs the action in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                           
![Public method]    | [InLockAsync(LockTypeEnum, Func&lt;TState, TState>)][31] | Asynchronously performs the function in a lock, passing in the current state and assigning the state to the result (Inherited from [ReaderWriterLocker&lt;TState>][4].)                   
![Public method]    | [InLockAsync&lt;TResult>(Func&lt;TResult>)][32]          | Asynchronously performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                              
![Protected method] | [MemberwiseClone][33]                                    | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [OnValueChanged][34]                                     | Calls RaiseValueChanged to raise the ValueChanged event                                                                                                                                   
![Protected method] | [RaiseValueChanged][35]                                  | Raises the ValueChanged event                                                                                                                                                             
![Protected method] | [SetValue][36]                                           | Sets the value and raises the ValueChanged event (Overrides [LockableSlim&lt;TValue>.SetValue(TValue)][37].)                                                                              
![Public method]    | [ToString][38]                                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [WaitForValueChanged][39]                                | Allows the caller to block until Value changes                                                                                                                                            


Events
------

                | Name               | Description                       
--------------- | ------------------ | --------------------------------- 
![Public event] | [ValueChanged][40] | Raised when the value has changed 


Fields
------

                   | Name        | Description                                                               
------------------ | ----------- | ------------------------------------------------------------------------- 
![Protected field] | [State][41] | The internal state (Inherited from [StateLocker&lt;TLocker, TState>][2].) 


See Also
--------

#### Reference
[W Namespace][7]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../../W.Threading.Lockers/StateLocker_2/README.md
[3]: ../../W.Threading.Lockers/ReaderWriterLocker/README.md
[4]: ../../W.Threading.Lockers/ReaderWriterLocker_1/README.md
[5]: ../LockableSlim_1/README.md
[6]: ../PropertySlim_1/README.md
[7]: ../README.md
[8]: _ctor.md
[9]: _ctor_1.md
[10]: _ctor_2.md
[11]: _ctor_3.md
[12]: ../../W.Threading.Lockers/StateLocker_2/Locker.md
[13]: ../LockableSlim_1/Value.md
[14]: Dispose.md
[15]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[16]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[17]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[18]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[19]: ../LockableSlim_1/GetValue.md
[20]: InformWaiters.md
[21]: ../../W.Threading.Lockers/StateLocker_2/InLock.md
[22]: ../../W.Threading.Lockers/StateLocker_2/InLock_1.md
[23]: ../../W.Threading.Lockers/StateLocker_2/InLock_2.md
[24]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLock.md
[25]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLock_1.md
[26]: ../../W.Threading.Lockers/StateLocker_2/InLock__1.md
[27]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync.md
[28]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync_1.md
[29]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync_2.md
[30]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLockAsync.md
[31]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLockAsync_1.md
[32]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync__1.md
[33]: http://msdn.microsoft.com/en-us/library/57ctke0a
[34]: OnValueChanged.md
[35]: RaiseValueChanged.md
[36]: SetValue.md
[37]: ../LockableSlim_1/SetValue.md
[38]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[39]: WaitForValueChanged.md
[40]: ValueChanged.md
[41]: ../../W.Threading.Lockers/StateLocker_2/State.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Protected field]: ../../_icons/protfield.gif "Protected field"