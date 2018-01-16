SemaphoreSlimExtensions Class
=============================
   Extensions to simplify locking with SemaphoreSlim


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.LockExtensions.SemaphoreSlimExtensions**  

  **Namespace:**  [W.LockExtensions][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static class SemaphoreSlimExtensions
```


Methods
-------

                                 | Name                                                      | Description                                    
-------------------------------- | --------------------------------------------------------- | ---------------------------------------------- 
![Public method]![Static member] | [InLock(SemaphoreSlim, Action)][3]                        | Performs the action in a lock                  
![Public method]![Static member] | [InLock&lt;TType>(SemaphoreSlim, Func&lt;TType>)][4]      | Performs the action in a lock                  
![Public method]![Static member] | [InLockAsync(SemaphoreSlim, Action)][5]                   | Asynchronously performs the action in a lock   
![Public method]![Static member] | [InLockAsync&lt;TType>(SemaphoreSlim, Func&lt;TType>)][6] | Asynchronously performs the function in a lock 


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
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"