SpinLocker Class
================
   Uses SpinLock to provide thread-safety


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.Lockers.SpinLocker**  

  **Namespace:**  [W.Threading.Lockers][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class SpinLocker : ILocker<SpinLock>, 
	ILocker
```

The **SpinLocker** type exposes the following members.


Constructors
------------

                 | Name            | Description                                            
---------------- | --------------- | ------------------------------------------------------ 
![Public method] | [SpinLocker][3] | Initializes a new instance of the **SpinLocker** class 


Properties
----------

                   | Name        | Description                        
------------------ | ----------- | ---------------------------------- 
![Public property] | [Locker][4] | The SpinLock used to perform locks 


Methods
-------

                    | Name                                            | Description                                
------------------- | ----------------------------------------------- | ------------------------------------------ 
![Public method]    | [Equals][5]                                     | (Inherited from [Object][1].)              
![Protected method] | [Finalize][6]                                   | (Inherited from [Object][1].)              
![Public method]    | [GetHashCode][7]                                | (Inherited from [Object][1].)              
![Public method]    | [GetType][8]                                    | (Inherited from [Object][1].)              
![Public method]    | [InLock(Action)][9]                             | Performs an action from within a SpinLock  
![Public method]    | [InLock&lt;TResult>(Func&lt;TResult>)][10]      | Performs a function from within a SpinLock 
![Public method]    | [InLockAsync(Action)][11]                       | Performs an action from within a SpinLock  
![Public method]    | [InLockAsync&lt;TResult>(Func&lt;TResult>)][12] | Performs a function from within a SpinLock 
![Protected method] | [MemberwiseClone][13]                           | (Inherited from [Object][1].)              
![Public method]    | [ToString][14]                                  | (Inherited from [Object][1].)              


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
[4]: Locker.md
[5]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[6]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[7]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[8]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[9]: InLock.md
[10]: InLock__1.md
[11]: InLockAsync.md
[12]: InLockAsync__1.md
[13]: http://msdn.microsoft.com/en-us/library/57ctke0a
[14]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"