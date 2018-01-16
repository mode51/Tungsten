Property&lt;TValue> Class
=========================
   A Property with no owner (self-owned)


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.Lockers.StateLocker][2]&lt;[ReaderWriterLocker][3], **TValue**>  
    [W.Threading.Lockers.ReaderWriterLocker][4]&lt;**TValue**>  
      [W.LockableSlim][5]&lt;**TValue**>  
        [W.Lockable][6]&lt;**TValue**>  
          [W.PropertySlim][7]&lt;**TValue**>  
            [W.PropertyBase][8]&lt;**Property**&lt;**TValue**>, **TValue**>  
              **W.Property<TValue>**  

  **Namespace:**  [W][9]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Property<TValue> : PropertyBase<Property<TValue>, TValue>

```

#### Type Parameters

##### *TValue*
The type of the property value

The **Property<TValue>** type exposes the following members.


Constructors
------------

                 | Name                                                                 | Description                                                  
---------------- | -------------------------------------------------------------------- | ------------------------------------------------------------ 
![Public method] | [Property&lt;TValue>()][10]                                          | Initializes a new instance of the **Property<TValue>** class 
![Public method] | [Property&lt;TValue>(Action&lt;Object, TValue, TValue>)][11]         | Initializes a new instance of the **Property<TValue>** class 
![Public method] | [Property&lt;TValue>(TValue)][12]                                    | Initializes a new instance of the **Property<TValue>** class 
![Public method] | [Property&lt;TValue>(TValue, Action&lt;Object, TValue, TValue>)][13] | Initializes a new instance of the **Property<TValue>** class 


Properties
----------

                   | Name               | Description                                                                                                                                                                                                     
------------------ | ------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public property] | [DefaultValue][14] | 
Allows the programmer to assign a default value which can be reset via the ResetToDefaultValue method. This value does not have to be the initial value.
 (Inherited from [PropertyBase&lt;TOwner, TValue>][8].) 
![Public property] | [IsDirty][15]      | True if Value has changed since initialization or since the last call to MarkAsClean (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                     
![Public property] | [Locker][16]       | The locking mechanism (SpinLock, Monitor, SemaphoreSlim, ReaderWriterLock) (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                               
![Public property] | [Owner][17]        | The property owner (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                                                                                       
![Public property] | [Value][18]        | Get or Set the value (Inherited from [LockableSlim&lt;TValue>][5].)                                                                                                                                             


Methods
-------

                    | Name                                                     | Description                                                                                                                                                                               
------------------- | -------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Dispose][19]                                            | Disposes the Lockable and releases resources (Inherited from [Lockable&lt;TValue>][6].)                                                                                                   
![Public method]    | [Equals][20]                                             | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [Finalize][21]                                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetHashCode][22]                                        | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetType][23]                                            | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [GetValue][24]                                           | Gets the underlying value (Inherited from [LockableSlim&lt;TValue>][5].)                                                                                                                  
![Protected method] | [InformWaiters][25]                                      | Informs those who are waiting on WaitForChanged that the value has changed (Inherited from [Lockable&lt;TValue>][6].)                                                                     
![Public method]    | [InLock(Action)][26]                                     | Performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                              
![Public method]    | [InLock(Action&lt;TState>)][27]                          | Performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                
![Public method]    | [InLock(Func&lt;TState, TState>)][28]                    | Performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].)                
![Public method]    | [InLock(LockTypeEnum, Action&lt;TState>)][29]            | Performs the action in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                          
![Public method]    | [InLock(LockTypeEnum, Func&lt;TState, TState>)][30]      | Performs the function in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                        
![Public method]    | [InLock&lt;TResult>(Func&lt;TResult>)][31]               | Performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                             
![Public method]    | [InLockAsync(Action)][32]                                | Asynchronously performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                               
![Public method]    | [InLockAsync(Action&lt;TState>)][33]                     | Asynchronously performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                 
![Public method]    | [InLockAsync(Func&lt;TState, TState>)][34]               | Asynchronously performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].) 
![Public method]    | [InLockAsync(LockTypeEnum, Action&lt;TState>)][35]       | Asynchronously performs the action in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                           
![Public method]    | [InLockAsync(LockTypeEnum, Func&lt;TState, TState>)][36] | Asynchronously performs the function in a lock, passing in the current state and assigning the state to the result (Inherited from [ReaderWriterLocker&lt;TState>][4].)                   
![Public method]    | [InLockAsync&lt;TResult>(Func&lt;TResult>)][37]          | Asynchronously performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                              
![Public method]    | [LoadValue][38]                                          | Sets Value without raising notification events (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                                     
![Protected method] | [MemberwiseClone][39]                                    | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [OnPropertyChanged][40]                                  | 
Calls RaisePropertyChanged to raise the PropertyChanged event
 (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                  
![Protected method] | [OnPropertyChanging][41]                                 | 
Calls RaisePropertyChanging to raise the PropertyChanging event
 (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                
![Protected method] | [OnValueChanged][42]                                     | Calls RaiseValueChanged to raise the ValueChanged event (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                            
![Protected method] | [RaiseOnPropertyChanged][43]                             | 
Raises the PropertyChanged event
 (Inherited from [PropertySlim&lt;TValue>][7].)                                                                                                       
![Protected method] | [RaiseOnPropertyChanging][44]                            | 
Raises the PropertyChanging event
 (Inherited from [PropertySlim&lt;TValue>][7].)                                                                                                      
![Protected method] | [RaiseValueChanged][45]                                  | Raises the ValueChanged event (Inherited from [Lockable&lt;TValue>][6].)                                                                                                                  
![Public method]    | [ResetToDefaultValue][46]                                | Resets the Value to the value provided by DefaultValue (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                             
![Protected method] | [SetValue][47]                                           | 
Calls OnPropertyChanged on assignment
 (Inherited from [PropertySlim&lt;TValue>][7].)                                                                                                  
![Public method]    | [ToString][48]                                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [WaitForValueChanged][49]                                | Allows the caller to block until Value changes (Inherited from [Lockable&lt;TValue>][6].)                                                                                                 


Events
------

                | Name                   | Description                                                                  
--------------- | ---------------------- | ---------------------------------------------------------------------------- 
![Public event] | [PropertyChanged][50]  | (Inherited from [PropertySlim&lt;TValue>][7].)                               
![Public event] | [PropertyChanging][51] | (Inherited from [PropertySlim&lt;TValue>][7].)                               
![Public event] | [ValueChanged][52]     | Raised when the value has changed (Inherited from [Lockable&lt;TValue>][6].) 


Fields
------

                   | Name        | Description                                                               
------------------ | ----------- | ------------------------------------------------------------------------- 
![Protected field] | [State][53] | The internal state (Inherited from [StateLocker&lt;TLocker, TState>][2].) 


See Also
--------

#### Reference
[W Namespace][9]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../../W.Threading.Lockers/StateLocker_2/README.md
[3]: ../../W.Threading.Lockers/ReaderWriterLocker/README.md
[4]: ../../W.Threading.Lockers/ReaderWriterLocker_1/README.md
[5]: ../LockableSlim_1/README.md
[6]: ../Lockable_1/README.md
[7]: ../PropertySlim_1/README.md
[8]: ../PropertyBase_2/README.md
[9]: ../README.md
[10]: _ctor.md
[11]: _ctor_1.md
[12]: _ctor_2.md
[13]: _ctor_3.md
[14]: ../PropertyBase_2/DefaultValue.md
[15]: ../PropertyBase_2/IsDirty.md
[16]: ../../W.Threading.Lockers/StateLocker_2/Locker.md
[17]: ../PropertyBase_2/Owner.md
[18]: ../LockableSlim_1/Value.md
[19]: ../Lockable_1/Dispose.md
[20]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[21]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[22]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[23]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[24]: ../LockableSlim_1/GetValue.md
[25]: ../Lockable_1/InformWaiters.md
[26]: ../../W.Threading.Lockers/StateLocker_2/InLock.md
[27]: ../../W.Threading.Lockers/StateLocker_2/InLock_1.md
[28]: ../../W.Threading.Lockers/StateLocker_2/InLock_2.md
[29]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLock.md
[30]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLock_1.md
[31]: ../../W.Threading.Lockers/StateLocker_2/InLock__1.md
[32]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync.md
[33]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync_1.md
[34]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync_2.md
[35]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLockAsync.md
[36]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLockAsync_1.md
[37]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync__1.md
[38]: ../PropertyBase_2/LoadValue.md
[39]: http://msdn.microsoft.com/en-us/library/57ctke0a
[40]: ../PropertyBase_2/OnPropertyChanged.md
[41]: ../PropertyBase_2/OnPropertyChanging.md
[42]: ../PropertyBase_2/OnValueChanged.md
[43]: ../PropertySlim_1/RaiseOnPropertyChanged.md
[44]: ../PropertySlim_1/RaiseOnPropertyChanging.md
[45]: ../Lockable_1/RaiseValueChanged.md
[46]: ../PropertyBase_2/ResetToDefaultValue.md
[47]: ../PropertySlim_1/SetValue.md
[48]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[49]: ../Lockable_1/WaitForValueChanged.md
[50]: ../PropertySlim_1/PropertyChanged.md
[51]: ../PropertySlim_1/PropertyChanging.md
[52]: ../Lockable_1/ValueChanged.md
[53]: ../../W.Threading.Lockers/StateLocker_2/State.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Protected field]: ../../_icons/protfield.gif "Protected field"