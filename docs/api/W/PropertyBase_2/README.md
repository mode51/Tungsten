PropertyBase&lt;TOwner, TValue> Class
=====================================
   
[Missing &lt;summary> documentation for "T:W.PropertyBase`2"]



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.Lockers.StateLocker][2]&lt;[ReaderWriterLocker][3], **TValue**>  
    [W.Threading.Lockers.ReaderWriterLocker][4]&lt;**TValue**>  
      [W.LockableSlim][5]&lt;**TValue**>  
        [W.Lockable][6]&lt;**TValue**>  
          [W.PropertySlim][7]&lt;**TValue**>  
            **W.PropertyBase<TOwner, TValue>**  
              [W.Property&lt;TValue>][8]  
              [W.Property&lt;TOwner, TValue>][9]  

  **Namespace:**  [W][10]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public abstract class PropertyBase<TOwner, TValue> : PropertySlim<TValue>, 
	IProperty<TValue>, IProperty, IDisposable

```

#### Type Parameters

##### *TOwner*

[Missing &lt;typeparam name="TOwner"/> documentation for "T:W.PropertyBase`2"]


##### *TValue*

[Missing &lt;typeparam name="TValue"/> documentation for "T:W.PropertyBase`2"]


The **PropertyBase<TOwner, TValue>** type exposes the following members.


Constructors
------------

                 | Name                                                                                     | Description                                                              
---------------- | ---------------------------------------------------------------------------------------- | ------------------------------------------------------------------------ 
![Public method] | [PropertyBase&lt;TOwner, TValue>()][11]                                                  | Initializes a new instance of the **PropertyBase<TOwner, TValue>** class 
![Public method] | [PropertyBase&lt;TOwner, TValue>(Action&lt;Object, TValue, TValue>)][12]                 | Initializes a new instance of the **PropertyBase<TOwner, TValue>** class 
![Public method] | [PropertyBase&lt;TOwner, TValue>(TOwner)][13]                                            | Initializes a new instance of the **PropertyBase<TOwner, TValue>** class 
![Public method] | [PropertyBase&lt;TOwner, TValue>(TValue)][14]                                            | Initializes a new instance of the **PropertyBase<TOwner, TValue>** class 
![Public method] | [PropertyBase&lt;TOwner, TValue>(TOwner, Action&lt;Object, TValue, TValue>)][15]         | Initializes a new instance of the **PropertyBase<TOwner, TValue>** class 
![Public method] | [PropertyBase&lt;TOwner, TValue>(TOwner, TValue)][16]                                    | Initializes a new instance of the **PropertyBase<TOwner, TValue>** class 
![Public method] | [PropertyBase&lt;TOwner, TValue>(TValue, Action&lt;Object, TValue, TValue>)][17]         | Initializes a new instance of the **PropertyBase<TOwner, TValue>** class 
![Public method] | [PropertyBase&lt;TOwner, TValue>(TOwner, TValue, Action&lt;Object, TValue, TValue>)][18] | Initializes a new instance of the **PropertyBase<TOwner, TValue>** class 


Properties
----------

                   | Name               | Description                                                                                                                                              
------------------ | ------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public property] | [DefaultValue][19] | 
Allows the programmer to assign a default value which can be reset via the ResetToDefaultValue method. This value does not have to be the initial value.
 
![Public property] | [IsDirty][20]      | True if Value has changed since initialization or since the last call to MarkAsClean                                                                     
![Public property] | [Locker][21]       | The locking mechanism (SpinLock, Monitor, SemaphoreSlim, ReaderWriterLock) (Inherited from [StateLocker&lt;TLocker, TState>][2].)                        
![Public property] | [Owner][22]        | The property owner                                                                                                                                       
![Public property] | [Value][23]        | Get or Set the value (Inherited from [LockableSlim&lt;TValue>][5].)                                                                                      


Methods
-------

                    | Name                                                     | Description                                                                                                                                                                               
------------------- | -------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Dispose][24]                                            | Disposes the Lockable and releases resources (Inherited from [Lockable&lt;TValue>][6].)                                                                                                   
![Public method]    | [Equals][25]                                             | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [Finalize][26]                                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetHashCode][27]                                        | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetType][28]                                            | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [GetValue][29]                                           | Gets the underlying value (Inherited from [LockableSlim&lt;TValue>][5].)                                                                                                                  
![Protected method] | [InformWaiters][30]                                      | Informs those who are waiting on WaitForChanged that the value has changed (Inherited from [Lockable&lt;TValue>][6].)                                                                     
![Public method]    | [InLock(Action)][31]                                     | Performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                              
![Public method]    | [InLock(Action&lt;TState>)][32]                          | Performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                
![Public method]    | [InLock(Func&lt;TState, TState>)][33]                    | Performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].)                
![Public method]    | [InLock(LockTypeEnum, Action&lt;TState>)][34]            | Performs the action in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                          
![Public method]    | [InLock(LockTypeEnum, Func&lt;TState, TState>)][35]      | Performs the function in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                                        
![Public method]    | [InLock&lt;TResult>(Func&lt;TResult>)][36]               | Performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                             
![Public method]    | [InLockAsync(Action)][37]                                | Asynchronously performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                               
![Public method]    | [InLockAsync(Action&lt;TState>)][38]                     | Asynchronously performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                 
![Public method]    | [InLockAsync(Func&lt;TState, TState>)][39]               | Asynchronously performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].) 
![Public method]    | [InLockAsync(LockTypeEnum, Action&lt;TState>)][40]       | Asynchronously performs the action in a lock, passing in the current state (Inherited from [ReaderWriterLocker&lt;TState>][4].)                                                           
![Public method]    | [InLockAsync(LockTypeEnum, Func&lt;TState, TState>)][41] | Asynchronously performs the function in a lock, passing in the current state and assigning the state to the result (Inherited from [ReaderWriterLocker&lt;TState>][4].)                   
![Public method]    | [InLockAsync&lt;TResult>(Func&lt;TResult>)][42]          | Asynchronously performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                              
![Public method]    | [LoadValue][43]                                          | Sets Value without raising notification events                                                                                                                                            
![Protected method] | [MemberwiseClone][44]                                    | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [OnPropertyChanged][45]                                  | 
Calls RaisePropertyChanged to raise the PropertyChanged event
 (Overrides [PropertySlim&lt;TValue>.OnPropertyChanged(String)][46].)                                                    
![Protected method] | [OnPropertyChanging][47]                                 | 
Calls RaisePropertyChanging to raise the PropertyChanging event
 (Overrides [PropertySlim&lt;TValue>.OnPropertyChanging(String)][48].)                                                 
![Protected method] | [OnValueChanged][49]                                     | Calls RaiseValueChanged to raise the ValueChanged event (Overrides [Lockable&lt;TValue>.OnValueChanged(Object, TValue, TValue)][50].)                                                     
![Protected method] | [RaiseOnPropertyChanged][51]                             | 
Raises the PropertyChanged event
 (Inherited from [PropertySlim&lt;TValue>][7].)                                                                                                       
![Protected method] | [RaiseOnPropertyChanging][52]                            | 
Raises the PropertyChanging event
 (Inherited from [PropertySlim&lt;TValue>][7].)                                                                                                      
![Protected method] | [RaiseValueChanged][53]                                  | Raises the ValueChanged event (Inherited from [Lockable&lt;TValue>][6].)                                                                                                                  
![Public method]    | [ResetToDefaultValue][54]                                | Resets the Value to the value provided by DefaultValue                                                                                                                                    
![Protected method] | [SetValue][55]                                           | 
Calls OnPropertyChanged on assignment
 (Inherited from [PropertySlim&lt;TValue>][7].)                                                                                                  
![Public method]    | [ToString][56]                                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [WaitForValueChanged][57]                                | Allows the caller to block until Value changes (Inherited from [Lockable&lt;TValue>][6].)                                                                                                 


Events
------

                | Name                   | Description                                                                  
--------------- | ---------------------- | ---------------------------------------------------------------------------- 
![Public event] | [PropertyChanged][58]  | (Inherited from [PropertySlim&lt;TValue>][7].)                               
![Public event] | [PropertyChanging][59] | (Inherited from [PropertySlim&lt;TValue>][7].)                               
![Public event] | [ValueChanged][60]     | Raised when the value has changed (Inherited from [Lockable&lt;TValue>][6].) 


Fields
------

                   | Name        | Description                                                               
------------------ | ----------- | ------------------------------------------------------------------------- 
![Protected field] | [State][61] | The internal state (Inherited from [StateLocker&lt;TLocker, TState>][2].) 


See Also
--------

#### Reference
[W Namespace][10]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../../W.Threading.Lockers/StateLocker_2/README.md
[3]: ../../W.Threading.Lockers/ReaderWriterLocker/README.md
[4]: ../../W.Threading.Lockers/ReaderWriterLocker_1/README.md
[5]: ../LockableSlim_1/README.md
[6]: ../Lockable_1/README.md
[7]: ../PropertySlim_1/README.md
[8]: ../Property_1/README.md
[9]: ../Property_2/README.md
[10]: ../README.md
[11]: _ctor.md
[12]: _ctor_1.md
[13]: _ctor_2.md
[14]: _ctor_6.md
[15]: _ctor_3.md
[16]: _ctor_4.md
[17]: _ctor_7.md
[18]: _ctor_5.md
[19]: DefaultValue.md
[20]: IsDirty.md
[21]: ../../W.Threading.Lockers/StateLocker_2/Locker.md
[22]: Owner.md
[23]: ../LockableSlim_1/Value.md
[24]: ../Lockable_1/Dispose.md
[25]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[26]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[27]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[28]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[29]: ../LockableSlim_1/GetValue.md
[30]: ../Lockable_1/InformWaiters.md
[31]: ../../W.Threading.Lockers/StateLocker_2/InLock.md
[32]: ../../W.Threading.Lockers/StateLocker_2/InLock_1.md
[33]: ../../W.Threading.Lockers/StateLocker_2/InLock_2.md
[34]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLock.md
[35]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLock_1.md
[36]: ../../W.Threading.Lockers/StateLocker_2/InLock__1.md
[37]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync.md
[38]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync_1.md
[39]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync_2.md
[40]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLockAsync.md
[41]: ../../W.Threading.Lockers/ReaderWriterLocker_1/InLockAsync_1.md
[42]: ../../W.Threading.Lockers/StateLocker_2/InLockAsync__1.md
[43]: LoadValue.md
[44]: http://msdn.microsoft.com/en-us/library/57ctke0a
[45]: OnPropertyChanged.md
[46]: ../PropertySlim_1/OnPropertyChanged.md
[47]: OnPropertyChanging.md
[48]: ../PropertySlim_1/OnPropertyChanging.md
[49]: OnValueChanged.md
[50]: ../Lockable_1/OnValueChanged.md
[51]: ../PropertySlim_1/RaiseOnPropertyChanged.md
[52]: ../PropertySlim_1/RaiseOnPropertyChanging.md
[53]: ../Lockable_1/RaiseValueChanged.md
[54]: ResetToDefaultValue.md
[55]: ../PropertySlim_1/SetValue.md
[56]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[57]: ../Lockable_1/WaitForValueChanged.md
[58]: ../PropertySlim_1/PropertyChanged.md
[59]: ../PropertySlim_1/PropertyChanging.md
[60]: ../Lockable_1/ValueChanged.md
[61]: ../../W.Threading.Lockers/StateLocker_2/State.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Protected field]: ../../_icons/protfield.gif "Protected field"