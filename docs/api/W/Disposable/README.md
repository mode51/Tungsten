Disposable Class
================
   Provides the Disposable pattern as a base class


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Disposable**  
    [W.Threading.ParameterizedThread][2]  

  **Namespace:**  [W][3]  
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
![Public method] | [Disposable][4] | Initializes a new instance of the **Disposable** class 


Methods
-------

                    | Name                     | Description                                                    
------------------- | ------------------------ | -------------------------------------------------------------- 
![Public method]    | [Dispose][5]             | This code added to correctly implement the disposable pattern. 
![Public method]    | [Equals][6]              | (Inherited from [Object][1].)                                  
![Protected method] | [Finalize][7]            | (Inherited from [Object][1].)                                  
![Public method]    | [GetHashCode][8]         | (Inherited from [Object][1].)                                  
![Public method]    | [GetType][9]             | (Inherited from [Object][1].)                                  
![Protected method] | [MemberwiseClone][10]    | (Inherited from [Object][1].)                                  
![Protected method] | [OnDispose][11]          | Overload to dispose managed objects                            
![Protected method] | [OnDisposeUnmanaged][12] | Override to release unmanaged objects                          
![Public method]    | [ToString][13]           | (Inherited from [Object][1].)                                  


Fields
------

                   | Name              | Description                                        
------------------ | ----------------- | -------------------------------------------------- 
![Protected field] | [IsDisposed][14]  | If True, the object has been disposed              
![Protected field] | [IsDisposing][15] | If True, the object is in the process of disposing 


See Also
--------

#### Reference
[W Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../../W.Threading/ParameterizedThread/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: Dispose.md
[6]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[7]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[8]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[9]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[10]: http://msdn.microsoft.com/en-us/library/57ctke0a
[11]: OnDispose.md
[12]: OnDisposeUnmanaged.md
[13]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[14]: IsDisposed.md
[15]: IsDisposing.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Protected field]: ../../_icons/protfield.gif "Protected field"