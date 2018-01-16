PropertySlim&lt;TValue> Class
=============================
   
[Missing &lt;summary> documentation for "T:W.PropertySlim`1"]



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.Lockers.StateLocker][2]&lt;[ReaderWriterLocker][3], **TValue**>  
    [W.Threading.Lockers.ReaderWriterLocker][4]&lt;**TValue**>  
      [W.LockableSlim][5]&lt;**TValue**>  
        [W.Lockable][6]&lt;**TValue**>  
          **W.PropertySlim<TValue>**  
            [W.PropertyBase&lt;TOwner, TValue>][7]  

  **Namespace:**  [W][8]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public abstract class PropertySlim<TValue> : Lockable<TValue>, 
	INotifyPropertyChanging, INotifyPropertyChanged

```

#### Type Parameters

##### *TValue*

[Missing &lt;typeparam name="TValue"/> documentation for "T:W.PropertySlim`1"]


The **PropertySlim<TValue>** type exposes the following members.


Constructors
------------

                 | Name                                                                     | Description                                                      
---------------- | ------------------------------------------------------------------------ | ---------------------------------------------------------------- 
![Public method] | [PropertySlim&lt;TValue>()][9]                                           | Initializes a new instance of the **PropertySlim<TValue>** class 
![Public method] | [PropertySlim&lt;TValue>(Action&lt;Object, TValue, TValue>)][10]         | Initializes a new instance of the **PropertySlim<TValue>** class 
![Public method] | [PropertySlim&lt;TValue>(TValue)][11]                                    | Initializes a new instance of the **PropertySlim<TValue>** class 
![Public method] | [PropertySlim&lt;TValue>(TValue, Action&lt;Object, TValue, TValue>)][12] | Initializes a new instance of the **PropertySlim<TValue>** class 


Properties
----------

                   | Name         | Description                                                                                                                       
------------------ | ------------ | --------------------------------------------------------------------------------------------------------------------------------- 
![Public property] | [Locker][13] | The locking mechanism (SpinLock, Monitor, SemaphoreSlim, ReaderWriterLock) (Inherited from [StateLocker&lt;TLocker, TState>][2].) 
![Public property] | [Value][14]  | Get or Set the value (Inherited from [LockableSlim&lt;TValue>][5].)                                                               


Methods
-------

                    | Name                                                     | Description                                                                                                                                                                               
------------------- | -------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Dispose][15]                                            | Disposes the Lockable and releases resources (Inherited from [Lockable&lt;TValue>][6].)                                                                                                   
![Public method]    | [Equals][16]                                             | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [Finalize][17]                                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetHashCode][18]                                        | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetType][19]                                            | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [GetValue][20]                                           | Gets the underlying value (Inherited from [LockableSlim&lt;TValue>][5].)                                                                                                                  
![Protected method] | [InformWaiters][21]                                      | Informs those who are waiting on WaitForChanged that the value has changed (Inherited from [Lockable&lt;TValue>][6].)                                                                     
![Public method]    | [InLock(Action)][22]                                     | Performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                              
![Public method]    | [InLock(Action&lt;TState>)][23]                          | Performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                
![Public method]    | [InLock(Func&lt;TState, TState>)][24]                    | Performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].)                
![Public method]    | [InLock(LockTypeEnum, Action&lt;TState>)][25]            | Performs the action in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                          
![Public method]    | [InLock(LockTypeEnum, Func&lt;TState, TState>)][26]      | Performs the function in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                        
![Public method]    | [InLock&lt;TResult>(Func&lt;TResult>)][27]               | Performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                             
![Public method]    | [InLockAsync(Action)][28]                                | Asynchronously performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                               
![Public method]    | [InLockAsync(Action&lt;TState>)][29]                     | Asynchronously performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                 
![Public method]    | [InLockAsync(Func&lt;TState, TState>)][30]               | Asynchronously performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].) 
![Public method]    | [InLockAsync(LockTypeEnum, Action&lt;TState>)][31]       | Asynchronously performs the action in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                           
![Public method]    | [InLockAsync(LockTypeEnum, Func&lt;TState, TState>)][32] | Asynchronously performs the function in a lock, passing in the current state and assigning the state to the result (Inherited from [ReaderWriterLocker&lt;TState>][4].)                   
![Public method]    | [InLockAsync&lt;TResult>(Func&lt;TResult>)][33]          | Asynchronously performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                              
![Protected method] | [MemberwiseClone][34]                                    | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [OnPropertyChanged][35]                                  | 
Calls RaisePropertyChanged to raise the PropertyChanged event
                                                                                                                         
![Protected method] | [OnPropertyChanging][36]                                 | 
Calls RaisePropertyChanging to raise the PropertyChanging event
                                                                                                                       
![Protected method] | [OnValueChanged][37]                                     | Calls RaiseValueChanged to raise the ValueChanged event (Inherited from [Lockable&lt;TValue>][6].)                                                                                        
![Protected method] | [RaiseOnPropertyChanged][38]                             | 
Raises the PropertyChanged event
                                                                                                                                                      
![Protected method] | [RaiseOnPropertyChanging][39]                            | 
Raises the PropertyChanging event
                                                                                                                                                     
![Protected method] | [RaiseValueChanged][40]                                  | Raises the ValueChanged event (Inherited from [Lockable&lt;TValue>][6].)                                                                                                                  
![Protected method] | [SetValue][41]                                           | 
Calls OnPropertyChanged on assignment
 (Overrides [Lockable&lt;TValue>.SetValue(TValue)][42].)                                                                                         
![Public method]    | [ToString][43]                                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [WaitForValueChanged][44]                                | Allows the caller to block until Value changes (Inherited from [Lockable&lt;TValue>][6].)                                                                                                 


Events
------

                | Name                   | Description                                                                  
--------------- | ---------------------- | ---------------------------------------------------------------------------- 
![Public event] | [PropertyChanged][45]  |                                                                              
![Public event] | [PropertyChanging][46] |                                                                              
![Public event] | [ValueChanged][47]     | Raised when the value has changed (Inherited from [Lockable&lt;TValue>][6].) 


Fields
------

                   | Name        | Description                                                               
------------------ | ----------- | ------------------------------------------------------------------------- 
![Protected field] | [State][48] | The internal state (Inherited from [StateLocker&lt;TLocker, TState>][2].) 


See Also
--------

#### Reference
[W Namespace][8]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../../W.Threading.Lockers/StateLocker_2/README.md
[3]: ../../W.Threading.Lockers/ReaderWriterLocker/README.md
[4]: ../../W.Threading.Lockers/ReaderWriterLocker_1/README.md
[5]: ../LockableSlim_1/README.md
[6]: ../Lockable_1/README.md
[7]: ../PropertyBase_2/README.md
[8]: ../README.md
[9]: _ctor.md
[10]: _ctor_1.md
[11]: _ctor_2.md
[12]: _ctor_3.md
[13]: ../../W.Threading.Lockers/StateLocker_2/Locker.md
[14]: ../LockableSlim_1/Value.md
[15]: ../Lockable_1/Dispose.md
[16]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[17]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[18]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[19]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[20]: ../LockableSlim_1/GetValue.md
[21]: ../Lockable_1/InformWaiters.md
[22]: ../../W.Threading.Lockers/StateLocker_2/InLock.md
[23]: ../../W.Threading.Lockers/StateLocker_2/InLock_1.md
[24]: ../../W.Threading.Lockers/StateLocker_2/InLock_2.md
[25]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLock.md
[26]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLock_1.md
[27]: ../../W.Threading.Lockers/StateLocker_2/InLock__1.md
[28]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync.md
[29]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync_1.md
[30]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync_2.md
[31]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLockAsync.md
[32]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLockAsync_1.md
[33]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync__1.md
[34]: http://msdn.microsoft.com/en-us/library/57ctke0a
[35]: OnPropertyChanged.md
[36]: OnPropertyChanging.md
[37]: ../Lockable_1/OnValueChanged.md
[38]: RaiseOnPropertyChanged.md
[39]: RaiseOnPropertyChanging.md
[40]: ../Lockable_1/RaiseValueChanged.md
[41]: SetValue.md
[42]: ../Lockable_1/SetValue.md
[43]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[44]: ../Lockable_1/WaitForValueChanged.md
[45]: PropertyChanged.md
[46]: PropertyChanging.md
[47]: ../Lockable_1/ValueChanged.md
[48]: ../../W.Threading.Lockers/StateLocker_2/State.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Protected field]: ../../_icons/protfield.gif "Protected field"