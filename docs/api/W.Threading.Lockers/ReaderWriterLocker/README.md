ReaderWriterLocker Class
========================
   Uses ReaderWriterLockSlim to provide thread-safety


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.Lockers.ReaderWriterLocker**  

  **Namespace:**  [W.Threading.Lockers][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class ReaderWriterLocker : IDisposable, 
	ILocker<ReaderWriterLockSlim>, ILocker
```

The **ReaderWriterLocker** type exposes the following members.


Constructors
------------

                 | Name                                         | Description                                                                   
---------------- | -------------------------------------------- | ----------------------------------------------------------------------------- 
![Public method] | [ReaderWriterLocker()][3]                    | Constructs a new ReaderWriterLocker with a LockRecursionPolicy of NoRecursion 
![Public method] | [ReaderWriterLocker(LockRecursionPolicy)][4] | Constructs a new ReaderWriterLocker using the specified LockRecursionPolicy   


Properties
----------

                   | Name        | Description                                    
------------------ | ----------- | ---------------------------------------------- 
![Public property] | [Locker][5] | The ReaderWriterLockSlim used to perform locks 


Methods
-------

                    | Name                                                        | Description                                            
------------------- | ----------------------------------------------------------- | ------------------------------------------------------ 
![Public method]    | [Dispose][6]                                                | Disposes the instance and releases resources           
![Public method]    | [Equals][7]                                                 | (Inherited from [Object][1].)                          
![Protected method] | [Finalize][8]                                               | (Inherited from [Object][1].)                          
![Public method]    | [GetHashCode][9]                                            | (Inherited from [Object][1].)                          
![Public method]    | [GetType][10]                                               | (Inherited from [Object][1].)                          
![Public method]    | [InLock(Action)][11]                                        | Performs the action in a read lock                     
![Public method]    | [InLock(LockTypeEnum, Action)][12]                          | Executes an action from within a ReaderWriterLockSlim  
![Public method]    | [InLock&lt;TResult>(Func&lt;TResult>)][13]                  | Performs the function in a read lock                   
![Public method]    | [InLock&lt;TValue>(LockTypeEnum, Func&lt;TValue>)][14]      | Executes a function from within a ReaderWriterLockSlim 
![Public method]    | [InLockAsync(Action)][15]                                   | Asynchronously performs the action in a read lock      
![Public method]    | [InLockAsync(LockTypeEnum, Action)][16]                     | Executes an action from within a ReaderWriterLockSlim  
![Public method]    | [InLockAsync&lt;TResult>(Func&lt;TResult>)][17]             | Asynchronously performs the function in a read lock    
![Public method]    | [InLockAsync&lt;TValue>(LockTypeEnum, Func&lt;TValue>)][18] | Executes a function from within a ReaderWriterLockSlim 
![Protected method] | [MemberwiseClone][19]                                       | (Inherited from [Object][1].)                          
![Public method]    | [ToString][20]                                              | (Inherited from [Object][1].)                          


Remarks
-------
Can be overridden to provide additional functionality

See Also
--------

#### Reference
[W.Threading.Lockers Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: _ctor_1.md
[5]: Locker.md
[6]: Dispose.md
[7]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[8]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[9]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[10]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[11]: InLock.md
[12]: InLock_1.md
[13]: InLock__1.md
[14]: InLock__1_1.md
[15]: InLockAsync.md
[16]: InLockAsync_1.md
[17]: InLockAsync__1.md
[18]: InLockAsync__1_1.md
[19]: http://msdn.microsoft.com/en-us/library/57ctke0a
[20]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"