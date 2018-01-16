W.Threading.Lockers Namespace
=============================
Lockers are designed to make locking of thread resources easier and less verbose


Classes
-------

                | Class                                | Description                                                                                                        
--------------- | ------------------------------------ | ------------------------------------------------------------------------------------------------------------------ 
![Public class] | [MonitorLocker][1]                   | Uses Monitor to provide thread-safety                                                                              
![Public class] | [MonitorLocker&lt;TState>][2]        | Extends MonitorLocker with an internal state variable                                                              
![Public class] | [ReaderWriterLocker][3]              | Uses ReaderWriterLockSlim to provide thread-safety                                                                 
![Public class] | [ReaderWriterLocker&lt;TState>][4]   | Extends ReaderWriterLocker with an internal state variable                                                         
![Public class] | [SemaphoreSlimLocker][5]             | Uses SemaphoreSlim to provide locking                                                                              
![Public class] | [SemaphoreSlimLocker&lt;TState>][6]  | Extends SemaphoreSlimLocker with an internal state variable                                                        
![Public class] | [SpinLocker][7]                      | Uses SpinLock to provide thread-safety                                                                             
![Public class] | [SpinLocker&lt;TState>][8]           | Extends SpinLocker with an internal state variable                                                                 
![Public class] | [StateLocker&lt;TLocker, TState>][9] | Extends a locker (SpinLocker, MonitorLocker, ReaderWriterLocker, SemaphoreSlimLocker) with an internal state value 


Interfaces
----------

                    | Interface                              | Description                                               
------------------- | -------------------------------------- | --------------------------------------------------------- 
![Public interface] | [ILocker][10]                          | The required implementation for a locking object          
![Public interface] | [ILocker&lt;TLocker>][11]              | The required implementation for a locking object          
![Public interface] | [IStateLocker&lt;TLocker, TState>][12] | The required implementation for a stateful locking object 


Delegates
---------

                   | Delegate                                 | Description                                                            
------------------ | ---------------------------------------- | ---------------------------------------------------------------------- 
![Public delegate] | [StateAssignmentDelegate&lt;TState>][13] | Delegate which can be used to assign a new value to the internal state 


Enumerations
------------

                      | Enumeration        | Description                                                      
--------------------- | ------------------ | ---------------------------------------------------------------- 
![Public enumeration] | [LockTypeEnum][14] | Used by ReaderWriterLocker to specify the type of lock to obtain 

[1]: MonitorLocker/README.md
[2]: MonitorLocker_1/README.md
[3]: ReaderWriterLocker/README.md
[4]: ReaderWriterLocker_1/README.md
[5]: SemaphoreSlimLocker/README.md
[6]: SemaphoreSlimLocker_1/README.md
[7]: SpinLocker/README.md
[8]: SpinLocker_1/README.md
[9]: StateLocker_2/README.md
[10]: ILocker/README.md
[11]: ILocker_1/README.md
[12]: IStateLocker_2/README.md
[13]: StateAssignmentDelegate_1/README.md
[14]: LockTypeEnum/README.md
[Public class]: ../_icons/pubclass.gif "Public class"
[Public interface]: ../_icons/pubinterface.gif "Public interface"
[Public delegate]: ../_icons/pubdelegate.gif "Public delegate"
[Public enumeration]: ../_icons/pubenumeration.gif "Public enumeration"