MonitorLocker&lt;TState> Class
==============================
   Extends MonitorLocker with an internal state variable


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.Lockers.StateLocker][2]&lt;[MonitorLocker][3], **TState**>  
    **W.Threading.Lockers.MonitorLocker<TState>**  

  **Namespace:**  [W.Threading.Lockers][4]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class MonitorLocker<TState> : StateLocker<MonitorLocker, TState>

```

#### Type Parameters

##### *TState*
The state Type

The **MonitorLocker<TState>** type exposes the following members.


Constructors
------------

                 | Name                          | Description                                                       
---------------- | ----------------------------- | ----------------------------------------------------------------- 
![Public method] | [MonitorLocker&lt;TState>][5] | Initializes a new instance of the **MonitorLocker<TState>** class 


Properties
----------

                   | Name        | Description                                                                                                                       
------------------ | ----------- | --------------------------------------------------------------------------------------------------------------------------------- 
![Public property] | [Locker][6] | The locking mechanism (SpinLock, Monitor, SemaphoreSlim, ReaderWriterLock) (Inherited from [StateLocker&lt;TLocker, TState>][2].) 


Methods
-------

                    | Name                                            | Description                                                                                                                                                                               
------------------- | ----------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Equals][7]                                     | (Inherited from [Object][1].)                                                                                                                                                             
![Protected method] | [Finalize][8]                                   | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetHashCode][9]                                | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [GetType][10]                                   | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [InLock(Action)][11]                            | Performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                              
![Public method]    | [InLock(Action&lt;TState>)][12]                 | Performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                
![Public method]    | [InLock(Func&lt;TState, TState>)][13]           | Performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].)                
![Public method]    | [InLock&lt;TResult>(Func&lt;TResult>)][14]      | Performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                                             
![Public method]    | [InLockAsync(Action)][15]                       | Asynchronously performs an action from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                               
![Public method]    | [InLockAsync(Action&lt;TState>)][16]            | Asynchronously performs an action from within a lock, passing in the current state (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                 
![Public method]    | [InLockAsync(Func&lt;TState, TState>)][17]      | Asynchronously performs a function from within a lock, passing in the current state and assigning the state to the function result (Inherited from [StateLocker&lt;TLocker, TState>][2].) 
![Public method]    | [InLockAsync&lt;TResult>(Func&lt;TResult>)][18] | Asynchronously performs a function from within a lock (Inherited from [StateLocker&lt;TLocker, TState>][2].)                                                                              
![Protected method] | [MemberwiseClone][19]                           | (Inherited from [Object][1].)                                                                                                                                                             
![Public method]    | [ToString][20]                                  | (Inherited from [Object][1].)                                                                                                                                                             


Fields
------

                   | Name        | Description                                                               
------------------ | ----------- | ------------------------------------------------------------------------- 
![Protected field] | [State][21] | The internal state (Inherited from [StateLocker&lt;TLocker, TState>][2].) 


Remarks
-------
Same as StateLocker&lt;MonitorLocker;, TState>

See Also
--------

#### Reference
[W.Threading.Lockers Namespace][4]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../StateLocker_2/README.md
[3]: ../MonitorLocker/README.md
[4]: ../README.md
[5]: _ctor.md
[6]: ../StateLocker_2/Locker.md
[7]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[8]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[9]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[10]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[11]: ../StateLocker_2/InLock.md
[12]: ../StateLocker_2/InLock_1.md
[13]: ../StateLocker_2/InLock_2.md
[14]: ../StateLocker_2/InLock__1.md
[15]: ../StateLocker_2/InLockAsync.md
[16]: ../StateLocker_2/InLockAsync_1.md
[17]: ../StateLocker_2/InLockAsync_2.md
[18]: ../StateLocker_2/InLockAsync__1.md
[19]: http://msdn.microsoft.com/en-us/library/57ctke0a
[20]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[21]: ../StateLocker_2/State.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Protected field]: ../../_icons/protfield.gif "Protected field"