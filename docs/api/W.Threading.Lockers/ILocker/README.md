ILocker Interface
=================
   The required implementation for a locking object

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public interface ILocker
```


Methods
-------

                 | Name                                           | Description                                    
---------------- | ---------------------------------------------- | ---------------------------------------------- 
![Public method] | [InLock(Action)][2]                            | Perform some action in a lock                  
![Public method] | [InLock&lt;TResult>(Func&lt;TResult>)][3]      | Perform some function in a lock                
![Public method] | [InLockAsync(Action)][4]                       |                                                
![Public method] | [InLockAsync&lt;TResult>(Func&lt;TResult>)][5] | Asyncrhonously perform some function in a lock 


See Also
--------

#### Reference
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: InLock.md
[3]: InLock__1.md
[4]: InLockAsync.md
[5]: InLockAsync__1.md
[Public method]: ../../_icons/pubmethod.gif "Public method"