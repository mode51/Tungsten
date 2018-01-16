Disposer Class
==============
   
Aids in implementing a clean Dispose method. Supports re-entrancy but only calls the cleanup Action once.



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Disposer**  

  **Namespace:**  [W][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Disposer
```

The **Disposer** type exposes the following members.


Constructors
------------

                 | Name          | Description                                          
---------------- | ------------- | ---------------------------------------------------- 
![Public method] | [Disposer][3] | Initializes a new instance of the **Disposer** class 


Properties
----------

                   | Name            | Description                                                    
------------------ | --------------- | -------------------------------------------------------------- 
![Public property] | [IsDisposed][4] | True if Cleanup has been called and completed, otherwise False 


Methods
-------

                    | Name                         | Description                                    
------------------- | ---------------------------- | ---------------------------------------------- 
![Public method]    | [Cleanup(Action)][5]         | Calls the action (should contain cleanup code) 
![Public method]    | [Cleanup(Object, Action)][6] | Calls the action (should contain cleanup code) 
![Public method]    | [Equals][7]                  | (Inherited from [Object][1].)                  
![Protected method] | [Finalize][8]                | (Inherited from [Object][1].)                  
![Public method]    | [GetHashCode][9]             | (Inherited from [Object][1].)                  
![Public method]    | [GetType][10]                | (Inherited from [Object][1].)                  
![Protected method] | [MemberwiseClone][11]        | (Inherited from [Object][1].)                  
![Public method]    | [ToString][12]               | (Inherited from [Object][1].)                  


See Also
--------

#### Reference
[W Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: IsDisposed.md
[5]: Cleanup.md
[6]: Cleanup_1.md
[7]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[8]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[9]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[10]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[11]: http://msdn.microsoft.com/en-us/library/57ctke0a
[12]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"