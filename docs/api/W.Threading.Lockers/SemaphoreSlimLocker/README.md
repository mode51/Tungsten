SemaphoreSlimLocker Class
=========================
   Uses SemaphoreSlim to provide resource locking


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.Lockers.SemaphoreSlimLocker**  

  **Namespace:**  [W.Threading.Lockers][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class SemaphoreSlimLocker : ILocker<SemaphoreSlim>, 
	ILocker, IDisposable
```

The **SemaphoreSlimLocker** type exposes the following members.


Constructors
------------

                 | Name                                   | Description                                                                                            
---------------- | -------------------------------------- | ------------------------------------------------------------------------------------------------------ 
![Public method] | [SemaphoreSlimLocker()][3]             | Constructs a new SemaphoreSlimLocker with an initial request count of 1 and maximum request count of 1 
![Public method] | [SemaphoreSlimLocker(Int32)][4]        | Constructs a new SemaphoreSlimLocker                                                                   
![Public method] | [SemaphoreSlimLocker(Int32, Int32)][5] | Constructs a new SemaphoreSlimLocker                                                                   


Properties
----------

                   | Name        | Description                             
------------------ | ----------- | --------------------------------------- 
![Public property] | [Locker][6] | The SemaphoreSlim used to perform locks 


Methods
-------

                    | Name                                          | Description                                             
------------------- | --------------------------------------------- | ------------------------------------------------------- 
![Public method]    | [Dispose][7]                                  | Disposes the SemaphoreSlimLocker and releases resources 
![Public method]    | [Equals][8]                                   | (Inherited from [Object][1].)                           
![Protected method] | [Finalize][9]                                 | (Inherited from [Object][1].)                           
![Public method]    | [GetHashCode][10]                             | (Inherited from [Object][1].)                           
![Public method]    | [GetType][11]                                 | (Inherited from [Object][1].)                           
![Public method]    | [InLock(Action)][12]                          | Executes an action from within a SemaphoreSlim          
![Public method]    | [InLock&lt;TValue>(Func&lt;TValue>)][13]      | Executes a function from within a SemaphoreSlim         
![Public method]    | [InLockAsync(Action)][14]                     | Executes an action from within a SemaphoreSlim          
![Public method]    | [InLockAsync&lt;TValue>(Func&lt;TValue>)][15] | Executes a function from within a SemaphoreSlim         
![Protected method] | [MemberwiseClone][16]                         | (Inherited from [Object][1].)                           
![Public method]    | [ToString][17]                                | (Inherited from [Object][1].)                           


See Also
--------

#### Reference
[W.Threading.Lockers Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: _ctor_1.md
[5]: _ctor_2.md
[6]: Locker.md
[7]: Dispose.md
[8]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[9]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[10]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[11]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[12]: InLock.md
[13]: InLock__1.md
[14]: InLockAsync.md
[15]: InLockAsync__1.md
[16]: http://msdn.microsoft.com/en-us/library/57ctke0a
[17]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"