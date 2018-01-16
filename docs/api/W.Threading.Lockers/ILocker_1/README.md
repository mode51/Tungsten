ILocker&lt;TLocker> Interface
=============================
   The required implementation for a locking object

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public interface ILocker<TLocker> : ILocker

```

#### Type Parameters

##### *TLocker*
The type of locker to use (SpinLock, Monitor, SemaphoreSlim, ReaderWriterLock)

The **ILocker<TLocker>** type exposes the following members.


Properties
----------

                   | Name        | Description                 
------------------ | ----------- | --------------------------- 
![Public property] | [Locker][2] | The object used for locking 


Methods
-------

                 | Name                                           | Description                                                                   
---------------- | ---------------------------------------------- | ----------------------------------------------------------------------------- 
![Public method] | [InLock(Action)][3]                            | Perform some action in a lock (Inherited from [ILocker][4].)                  
![Public method] | [InLock&lt;TResult>(Func&lt;TResult>)][5]      | Perform some function in a lock (Inherited from [ILocker][4].)                
![Public method] | [InLockAsync(Action)][6]                       | (Inherited from [ILocker][4].)                                                
![Public method] | [InLockAsync&lt;TResult>(Func&lt;TResult>)][7] | Asyncrhonously perform some function in a lock (Inherited from [ILocker][4].) 


See Also
--------

#### Reference
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: Locker.md
[3]: ../ILocker/InLock.md
[4]: ../ILocker/README.md
[5]: ../ILocker/InLock__1.md
[6]: ../ILocker/InLockAsync.md
[7]: ../ILocker/InLockAsync__1.md
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public method]: ../../_icons/pubmethod.gif "Public method"