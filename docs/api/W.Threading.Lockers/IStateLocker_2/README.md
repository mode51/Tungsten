IStateLocker&lt;TLocker, TState> Interface
==========================================
   The required implementation for a stateful locking object

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public interface IStateLocker<TLocker, TState> : ILocker<TLocker>, 
	ILocker

```

#### Type Parameters

##### *TLocker*

[Missing &lt;typeparam name="TLocker"/> documentation for "T:W.Threading.Lockers.IStateLocker`2"]


##### *TState*

[Missing &lt;typeparam name="TState"/> documentation for "T:W.Threading.Lockers.IStateLocker`2"]


The **IStateLocker<TLocker, TState>** type exposes the following members.


Properties
----------

                   | Name        | Description                                                            
------------------ | ----------- | ---------------------------------------------------------------------- 
![Public property] | [Locker][2] | The object used for locking (Inherited from [ILocker&lt;TLocker>][3].) 


Methods
-------

                 | Name                                            | Description                                                                   
---------------- | ----------------------------------------------- | ----------------------------------------------------------------------------- 
![Public method] | [InLock(Action)][4]                             | Perform some action in a lock (Inherited from [ILocker][5].)                  
![Public method] | [InLock(Action&lt;TState>)][6]                  | Perform some action in a lock                                                 
![Public method] | [InLock(Func&lt;TState, TState>)][7]            | Perform some function in a lock                                               
![Public method] | [InLock&lt;TResult>(Func&lt;TResult>)][8]       | Perform some function in a lock (Inherited from [ILocker][5].)                
![Public method] | [InLockAsync(Action)][9]                        | (Inherited from [ILocker][5].)                                                
![Public method] | [InLockAsync(Action&lt;TState>)][10]            |                                                                               
![Public method] | [InLockAsync(Func&lt;TState, TState>)][11]      | Asyncrhonously perform some function in a lock                                
![Public method] | [InLockAsync&lt;TResult>(Func&lt;TResult>)][12] | Asyncrhonously perform some function in a lock (Inherited from [ILocker][5].) 


See Also
--------

#### Reference
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: ../ILocker_1/Locker.md
[3]: ../ILocker_1/README.md
[4]: ../ILocker/InLock.md
[5]: ../ILocker/README.md
[6]: InLock.md
[7]: InLock_1.md
[8]: ../ILocker/InLock__1.md
[9]: ../ILocker/InLockAsync.md
[10]: InLockAsync.md
[11]: InLockAsync_1.md
[12]: ../ILocker/InLockAsync__1.md
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public method]: ../../_icons/pubmethod.gif "Public method"