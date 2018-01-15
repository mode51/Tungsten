Disposable Class
================
   Provides the Disposable pattern as a base class


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Disposable**  
    [W.LockableSlim&lt;TValue>][2]  
    [W.Threading.ParameterizedThread][3]  

  **Namespace:**  [W][4]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Disposable : IDisposable
```

The **Disposable** type exposes the following members.


Constructors
------------

                 | Name            | Description                                            
---------------- | --------------- | ------------------------------------------------------ 
![Public method] | [Disposable][5] | Initializes a new instance of the **Disposable** class 


Methods
-------

                    | Name                     | Description                                                                                                                          
------------------- | ------------------------ | ------------------------------------------------------------------------------------------------------------------------------------ 
![Public method]    | [Dispose][6]             | This code added to correctly implement the disposable pattern.                                                                       
![Public method]    | [Equals][7]              | (Inherited from [Object][1].)                                                                                                        
![Protected method] | [Finalize][8]            | Override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources. (Overrides [Object.Finalize()][9].) 
![Public method]    | [GetHashCode][10]        | (Inherited from [Object][1].)                                                                                                        
![Public method]    | [GetType][11]            | (Inherited from [Object][1].)                                                                                                        
![Protected method] | [MemberwiseClone][12]    | (Inherited from [Object][1].)                                                                                                        
![Protected method] | [OnDispose][13]          | Overload to dispose managed objects                                                                                                  
![Protected method] | [OnDisposeUnmanaged][14] | Override to release unmanaged objects                                                                                                
![Public method]    | [ToString][15]           | (Inherited from [Object][1].)                                                                                                        


Fields
------

                   | Name              | Description                                        
------------------ | ----------------- | -------------------------------------------------- 
![Protected field] | [IsDisposed][16]  | If True, the object has been disposed              
![Protected field] | [IsDisposing][17] | If True, the object is in the process of disposing 


See Also
--------

#### Reference
[W Namespace][4]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../LockableSlim_1/README.md
[3]: ../../W.Threading/ParameterizedThread/README.md
[4]: ../README.md
[5]: _ctor.md
[6]: Dispose.md
[7]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[8]: Finalize.md
[9]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[10]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[11]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[12]: http://msdn.microsoft.com/en-us/library/57ctke0a
[13]: OnDispose.md
[14]: OnDisposeUnmanaged.md
[15]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[16]: IsDisposed.md
[17]: IsDisposing.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Protected field]: ../../_icons/protfield.gif "Protected field"