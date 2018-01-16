ReaderWriterLockSlimExtensions Class
====================================
   Extensions to simplify locking with ReaderWriterLockSlim


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.LockExtensions.ReaderWriterLockSlimExtensions**  

  **Namespace:**  [W.LockExtensions][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static class ReaderWriterLockSlimExtensions
```

The **ReaderWriterLockSlimExtensions** type exposes the following members.


Methods
-------

                                 | Name                                                                           | Description                                             
-------------------------------- | ------------------------------------------------------------------------------ | ------------------------------------------------------- 
![Public method]![Static member] | [InLock(ReaderWriterLockSlim, LockTypeEnum, Action)][3]                        | Performs the action in a lock                           
![Public method]![Static member] | [InLock&lt;TType>(ReaderWriterLockSlim, LockTypeEnum, Func&lt;TType>)][4]      | Performs the function in a lock                         
![Public method]![Static member] | [InLockAsync(ReaderWriterLockSlim, LockTypeEnum, Action)][5]                   | Asynchronously performs the action in a lock            
![Public method]![Static member] | [InLockAsync&lt;TType>(ReaderWriterLockSlim, LockTypeEnum, Func&lt;TType>)][6] | Asynchronously performs the function in a lock          
![Public method]![Static member] | [Lock][7]                                                                      | Enters a read or write lock on the ReaderWriterLockSlim 
![Public method]![Static member] | [Unlock][8]                                                                    | Exits a read or write lock on the ReaderWriterLockSlim  


See Also
--------

#### Reference
[W.LockExtensions Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: InLock.md
[4]: InLock__1.md
[5]: InLockAsync.md
[6]: InLockAsync__1.md
[7]: Lock.md
[8]: Unlock.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"