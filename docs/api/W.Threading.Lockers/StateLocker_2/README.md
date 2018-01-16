StateLocker&lt;TLocker, TState> Class
=====================================
   Extends a locker (SpinLocker, MonitorLocker, ReaderWriterLocker, SemaphoreSlimLocker) with an internal state value


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.Lockers.StateLocker<TLocker, TState>**  
    [W.Threading.Lockers.MonitorLocker&lt;TState>][2]  
    [W.Threading.Lockers.ReaderWriterLocker&lt;TState>][3]  
    [W.Threading.Lockers.SemaphoreSlimLocker&lt;TState>][4]  
    [W.Threading.Lockers.SpinLocker&lt;TState>][5]  

  **Namespace:**  [W.Threading.Lockers][6]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class StateLocker<TLocker, TState> : IStateLocker<TLocker, TState>, 
	ILocker<TLocker>, ILocker
where TLocker : new(), ILocker

```

#### Type Parameters

##### *TLocker*
The Type of Locker to extend

##### *TState*
The Type of the internal state value

The **StateLocker<TLocker, TState>** type exposes the following members.


Constructors
------------

                 | Name                                 | Description                                                              
---------------- | ------------------------------------ | ------------------------------------------------------------------------ 
![Public method] | [StateLocker&lt;TLocker, TState>][7] | Initializes a new instance of the **StateLocker<TLocker, TState>** class 


Properties
----------

                   | Name        | Description                                                                
------------------ | ----------- | -------------------------------------------------------------------------- 
![Public property] | [Locker][8] | The locking mechanism (SpinLock, Monitor, SemaphoreSlim, ReaderWriterLock) 


Methods
-------

                    | Name                                            | Description                                                                                                                        
------------------- | ----------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Equals][9]                                     | (Inherited from [Object][1].)                                                                                                      
![Protected method] | [Finalize][10]                                  | (Inherited from [Object][1].)                                                                                                      
![Public method]    | [GetHashCode][11]                               | (Inherited from [Object][1].)                                                                                                      
![Public method]    | [GetType][12]                                   | (Inherited from [Object][1].)                                                                                                      
![Public method]    | [InLock(Action)][13]                            | Performs an action from within a lock                                                                                              
![Public method]    | [InLock(Action&lt;TState>)][14]                 | Performs an action from within a lock, passing in the current state                                                                
![Public method]    | [InLock(Func&lt;TState, TState>)][15]           | Performs a function from within a lock, passing in the current state and assigning the state to the function result                
![Public method]    | [InLock&lt;TResult>(Func&lt;TResult>)][16]      | Performs a function from within a lock                                                                                             
![Public method]    | [InLockAsync(Action)][17]                       | Asynchronously performs an action from within a lock                                                                               
![Public method]    | [InLockAsync(Action&lt;TState>)][18]            | Asynchronously performs an action from within a lock, passing in the current state                                                 
![Public method]    | [InLockAsync(Func&lt;TState, TState>)][19]      | Asynchronously performs a function from within a lock, passing in the current state and assigning the state to the function result 
![Public method]    | [InLockAsync&lt;TResult>(Func&lt;TResult>)][20] | Asynchronously performs a function from within a lock                                                                              
![Protected method] | [MemberwiseClone][21]                           | (Inherited from [Object][1].)                                                                                                      
![Public method]    | [ToString][22]                                  | (Inherited from [Object][1].)                                                                                                      


Fields
------

                   | Name        | Description        
------------------ | ----------- | ------------------ 
![Protected field] | [State][23] | The internal state 


Remarks
-------
This class adds the state functionality by wrapping the TLocker and re-implementing the ILocker interface

See Also
--------

#### Reference
[W.Threading.Lockers Namespace][6]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../MonitorLocker_1/README.md
[3]: ../ReaderWriterLocker_1/README.md
[4]: ../SemaphoreSlimLocker_1/README.md
[5]: ../SpinLocker_1/README.md
[6]: ../README.md
[7]: _ctor.md
[8]: Locker.md
[9]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[10]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[11]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[12]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[13]: InLock.md
[14]: InLock_1.md
[15]: InLock_2.md
[16]: InLock__1.md
[17]: InLockAsync.md
[18]: InLockAsync_1.md
[19]: InLockAsync_2.md
[20]: InLockAsync__1.md
[21]: http://msdn.microsoft.com/en-us/library/57ctke0a
[22]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[23]: State.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Protected field]: ../../_icons/protfield.gif "Protected field"