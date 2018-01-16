Property&lt;TOwner, TValue> Class
=================================
   A generic Property with an owner


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.Lockers.StateLocker][2]&lt;[ReaderWriterLocker][3], **TValue**>  
    [W.Threading.Lockers.ReaderWriterLocker][4]&lt;**TValue**>  
      [W.LockableSlim][5]&lt;**TValue**>  
        [W.Lockable][6]&lt;**TValue**>  
          [W.PropertySlim][7]&lt;**TValue**>  
            [W.PropertyBase][8]&lt;**TOwner**, **TValue**>  
              **W.Property<TOwner, TValue>**  

  **Namespace:**  [W][9]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Property<TOwner, TValue> : PropertyBase<TOwner, TValue>, 
	IOwnedProperty
where TOwner : class

```

#### Type Parameters

##### *TOwner*
The type of owner

##### *TValue*
The type of the property value

The **Property<TOwner, TValue>** type exposes the following members.


Constructors
------------

                 | Name                                                                                 | Description                                                          
---------------- | ------------------------------------------------------------------------------------ | -------------------------------------------------------------------- 
![Public method] | [Property&lt;TOwner, TValue>()][10]                                                  | Constructs a new Property                                            
![Public method] | [Property&lt;TOwner, TValue>(Action&lt;Object, TValue, TValue>)][11]                 | Initializes a new instance of the **Property<TOwner, TValue>** class 
![Public method] | [Property&lt;TOwner, TValue>(TOwner)][12]                                            | Constructs a new Property                                            
![Public method] | [Property&lt;TOwner, TValue>(TValue)][13]                                            | Constructs a new Property                                            
![Public method] | [Property&lt;TOwner, TValue>(TOwner, Action&lt;Object, TValue, TValue>)][14]         | Initializes a new instance of the **Property<TOwner, TValue>** class 
![Public method] | [Property&lt;TOwner, TValue>(TOwner, TValue)][15]                                    | Constructs a new Property                                            
![Public method] | [Property&lt;TOwner, TValue>(TValue, Action&lt;Object, TValue, TValue>)][16]         | Initializes a new instance of the **Property<TOwner, TValue>** class 
![Public method] | [Property&lt;TOwner, TValue>(TOwner, TValue, Action&lt;Object, TValue, TValue>)][17] | Initializes a new instance of the **Property<TOwner, TValue>** class 


Properties
----------

                   | Name               | Description                                                                                                                                                                                                     
------------------ | ------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public property] | [DefaultValue][18] | 
Allows the programmer to assign a default value which can be reset via the ResetToDefaultValue method. This value does not have to be the initial value.
 (Inherited from [PropertyBase&lt;TOwner, TValue>][8].) 
![Public property] | [IsDirty][19]      | True if Value has changed since initialization or since the last call to MarkAsClean (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                     
![Public property] | [Locker][20]       | The locking mechanism (SpinLock, Monitor, SemaphoreSlim, ReaderWriterLock) (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                               
![Public property] | [Owner][21]        | The property owner (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                                                                                       
![Public property] | [Value][22]        | Get or Set the value (Inherited from [LockableSlim&lt;TValue>][5].)                                                                                                                                             


Methods
-------

                    | Name                                                     | Description                                                                                                                                                                               
------------------- | -------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Dispose][23]                                            | Disposes the Lockable and releases resources (Inherited from [Lockable&lt;TValue>][6].)                                                                                                   
![Public method]    | [Equals][24]                                             | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [Finalize][25]                                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetHashCode][26]                                        | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetType][27]                                            | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [GetValue][28]                                           | Gets the underlying value (Inherited from [LockableSlim&lt;TValue>][5].)                                                                                                                  
![Protected method] | [InformWaiters][29]                                      | Informs those who are waiting on WaitForChanged that the value has changed (Inherited from [Lockable&lt;TValue>][6].)                                                                     
![Public method]    | [InLock(Action)][30]                                     | Performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                              
![Public method]    | [InLock(Action&lt;TState>)][31]                          | Performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                
![Public method]    | [InLock(Func&lt;TState, TState>)][32]                    | Performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].)                
![Public method]    | [InLock(LockTypeEnum, Action&lt;TState>)][33]            | Performs the action in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                          
![Public method]    | [InLock(LockTypeEnum, Func&lt;TState, TState>)][34]      | Performs the function in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                        
![Public method]    | [InLock&lt;TResult>(Func&lt;TResult>)][35]               | Performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                             
![Public method]    | [InLockAsync(Action)][36]                                | Asynchronously performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                               
![Public method]    | [InLockAsync(Action&lt;TState>)][37]                     | Asynchronously performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                 
![Public method]    | [InLockAsync(Func&lt;TState, TState>)][38]               | Asynchronously performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].) 
![Public method]    | [InLockAsync(LockTypeEnum, Action&lt;TState>)][39]       | Asynchronously performs the action in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                           
![Public method]    | [InLockAsync(LockTypeEnum, Func&lt;TState, TState>)][40] | Asynchronously performs the function in a lock, passing in the current state and assigning the state to the result (Inherited from [ReaderWriterLocker&lt;TState>][4].)                   
![Public method]    | [InLockAsync&lt;TResult>(Func&lt;TResult>)][41]          | Asynchronously performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                              
![Public method]    | [LoadValue][42]                                          | Sets Value without raising notification events (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                                     
![Protected method] | [MemberwiseClone][43]                                    | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [OnPropertyChanged][44]                                  | 
Calls RaisePropertyChanged to raise the PropertyChanged event
 (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                  
![Protected method] | [OnPropertyChanging][45]                                 | 
Calls RaisePropertyChanging to raise the PropertyChanging event
 (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                
![Protected method] | [OnValueChanged][46]                                     | Calls RaiseValueChanged to raise the ValueChanged event (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                            
![Protected method] | [RaiseOnPropertyChanged][47]                             | 
Raises the PropertyChanged event
 (Inherited from [PropertySlim&lt;TValue>][7].)                                                                                                       
![Protected method] | [RaiseOnPropertyChanging][48]                            | 
Raises the PropertyChanging event
 (Inherited from [PropertySlim&lt;TValue>][7].)                                                                                                      
![Protected method] | [RaiseValueChanged][49]                                  | Raises the ValueChanged event (Inherited from [Lockable&lt;TValue>][6].)                                                                                                                  
![Public method]    | [ResetToDefaultValue][50]                                | Resets the Value to the value provided by DefaultValue (Inherited from [PropertyBase&lt;TOwner, TValue>][8].)                                                                             
![Protected method] | [SetValue][51]                                           | 
Calls OnPropertyChanged on assignment
 (Inherited from [PropertySlim&lt;TValue>][7].)                                                                                                  
![Public method]    | [ToString][52]                                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [WaitForValueChanged][53]                                | Allows the caller to block until Value changes (Inherited from [Lockable&lt;TValue>][6].)                                                                                                 


Events
------

                | Name                   | Description                                                                  
--------------- | ---------------------- | ---------------------------------------------------------------------------- 
![Public event] | [PropertyChanged][54]  | (Inherited from [PropertySlim&lt;TValue>][7].)                               
![Public event] | [PropertyChanging][55] | (Inherited from [PropertySlim&lt;TValue>][7].)                               
![Public event] | [ValueChanged][56]     | Raised when the value has changed (Inherited from [Lockable&lt;TValue>][6].) 


Fields
------

                   | Name        | Description                                                               
------------------ | ----------- | ------------------------------------------------------------------------- 
![Protected field] | [State][57] | The internal state (Inherited from [StateLocker&lt;TLocker, TState>][2].) 


Explicit Interface Implementations
----------------------------------

                                                      | Name                          | Description 
----------------------------------------------------- | ----------------------------- | ----------- 
![Explicit interface implementation]![Private method] | [IOwnedProperty.SetOwner][58] |             


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
[13]: _ctor_6.md
[14]: _ctor_3.md
[15]: _ctor_4.md
[16]: _ctor_7.md
[17]: _ctor_5.md
[18]: ../PropertyBase_2/DefaultValue.md
[19]: ../PropertyBase_2/IsDirty.md
[20]: ../../W.Threading.Lockers/StateLocker_2/Locker.md
[21]: ../PropertyBase_2/Owner.md
[22]: ../LockableSlim_1/Value.md
[23]: ../Lockable_1/Dispose.md
[24]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[25]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[26]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[27]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[28]: ../LockableSlim_1/GetValue.md
[29]: ../Lockable_1/InformWaiters.md
[30]: ../../W.Threading.Lockers/StateLocker_2/InLock.md
[31]: ../../W.Threading.Lockers/StateLocker_2/InLock_1.md
[32]: ../../W.Threading.Lockers/StateLocker_2/InLock_2.md
[33]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLock.md
[34]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLock_1.md
[35]: ../../W.Threading.Lockers/StateLocker_2/InLock__1.md
[36]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync.md
[37]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync_1.md
[38]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync_2.md
[39]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLockAsync.md
[40]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLockAsync_1.md
[41]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync__1.md
[42]: ../PropertyBase_2/LoadValue.md
[43]: http://msdn.microsoft.com/en-us/library/57ctke0a
[44]: ../PropertyBase_2/OnPropertyChanged.md
[45]: ../PropertyBase_2/OnPropertyChanging.md
[46]: ../PropertyBase_2/OnValueChanged.md
[47]: ../PropertySlim_1/RaiseOnPropertyChanged.md
[48]: ../PropertySlim_1/RaiseOnPropertyChanging.md
[49]: ../Lockable_1/RaiseValueChanged.md
[50]: ../PropertyBase_2/ResetToDefaultValue.md
[51]: ../PropertySlim_1/SetValue.md
[52]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[53]: ../Lockable_1/WaitForValueChanged.md
[54]: ../PropertySlim_1/PropertyChanged.md
[55]: ../PropertySlim_1/PropertyChanging.md
[56]: ../Lockable_1/ValueChanged.md
[57]: ../../W.Threading.Lockers/StateLocker_2/State.md
[58]: W_IOwnedProperty_SetOwner.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Protected field]: ../../_icons/protfield.gif "Protected field"
[Explicit interface implementation]: ../../_icons/pubinterface.gif "Explicit interface implementation"
[Private method]: ../../_icons/privmethod.gif "Private method"