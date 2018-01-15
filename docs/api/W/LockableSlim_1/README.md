LockableSlim&lt;TValue> Class
=============================
   
Provides thread safety via ReaderWriterLockSlim locking. This is more efficient than Lockable&lt;TValue>.



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Disposable][2]  
    **W.LockableSlim<TValue>**  

  **Namespace:**  [W][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class LockableSlim<TValue> : Disposable

```

#### Type Parameters

##### *TValue*
The Type of the lockable value

The **LockableSlim<TValue>** type exposes the following members.


Constructors
------------

                 | Name                                 | Description                                                    
---------------- | ------------------------------------ | -------------------------------------------------------------- 
![Public method] | [LockableSlim&lt;TValue>()][4]       | Constructor which initializes Value with the default of TValue 
![Public method] | [LockableSlim&lt;TValue>(TValue)][5] | Constructor which initializes Value with the specified value   


Properties
----------

                   | Name       | Description                                   
------------------ | ---------- | --------------------------------------------- 
![Public property] | [Value][6] | 
Provides automatic locking during read/writes
 


Methods
-------

                    | Name                     | Description                                                                                         
------------------- | ------------------------ | --------------------------------------------------------------------------------------------------- 
![Public method]    | [Dispose][7]             | This code added to correctly implement the disposable pattern. (Inherited from [Disposable][2].)    
![Public method]    | [Equals][8]              | (Inherited from [Object][1].)                                                                       
![Protected method] | [Finalize][9]            | Destructs the LockableSlim instance (Overrides [Disposable.Finalize()][10].)                        
![Public method]    | [GetHashCode][11]        | (Inherited from [Object][1].)                                                                       
![Public method]    | [GetType][12]            | (Inherited from [Object][1].)                                                                       
![Protected method] | [MemberwiseClone][13]    | (Inherited from [Object][1].)                                                                       
![Protected method] | [OnDispose][14]          | Disposes the LockableSlim instance and releases resources (Overrides [Disposable.OnDispose()][15].) 
![Protected method] | [OnDisposeUnmanaged][16] | Override to release unmanaged objects (Inherited from [Disposable][2].)                             
![Public method]    | [ToString][17]           | (Inherited from [Object][1].)                                                                       


Fields
------

                   | Name              | Description                                                                          
------------------ | ----------------- | ------------------------------------------------------------------------------------ 
![Protected field] | [IsDisposed][18]  | If True, the object has been disposed (Inherited from [Disposable][2].)              
![Protected field] | [IsDisposing][19] | If True, the object is in the process of disposing (Inherited from [Disposable][2].) 


See Also
--------

#### Reference
[W Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../Disposable/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: _ctor_1.md
[6]: Value.md
[7]: ../Disposable/Dispose.md
[8]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[9]: Finalize.md
[10]: ../Disposable/Finalize.md
[11]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[12]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[13]: http://msdn.microsoft.com/en-us/library/57ctke0a
[14]: OnDispose.md
[15]: ../Disposable/OnDispose.md
[16]: ../Disposable/OnDisposeUnmanaged.md
[17]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[18]: ../Disposable/IsDisposed.md
[19]: ../Disposable/IsDisposing.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Protected field]: ../../_icons/protfield.gif "Protected field"