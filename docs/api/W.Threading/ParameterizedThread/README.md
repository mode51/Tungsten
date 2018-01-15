ParameterizedThread Class
=========================
   A thread class which supports a variable number of arguments


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Disposable][2]  
    **W.Threading.ParameterizedThread**  

  **Namespace:**  [W.Threading][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class ParameterizedThread : Disposable
```

The **ParameterizedThread** type exposes the following members.


Constructors
------------

                 | Name                     | Description                          
---------------- | ------------------------ | ------------------------------------ 
![Public method] | [ParameterizedThread][4] | Constructs a new ParameterizedThread 


Properties
----------

                   | Name            | Description                                                  
------------------ | --------------- | ------------------------------------------------------------ 
![Public property] | [Exception][5]  | The exception, if one was caught                             
![Public property] | [IsComplete][6] | True if the thread has completed, otherwise False            
![Public property] | [IsFaulted][7]  | True if the thread raised an exception, otherwise False      
![Public property] | [IsStarted][8]  | True if the thread has been started, otherwise False         
![Public property] | [Task][9]       | The underlying Task associated with this ParameterizedThread 
![Public property] | [Token][10]     | Not available until after Start has been called              


Methods
-------

                                 | Name                         | Description                                                                                                                        
-------------------------------- | ---------------------------- | ---------------------------------------------------------------------------------------------------------------------------------- 
![Public method]![Static member] | [Create][11]                 | Starts a new ParameterizedThread                                                                                                   
![Public method]                 | [Dispose][12]                | This code added to correctly implement the disposable pattern. (Inherited from [Disposable][2].)                                   
![Public method]                 | [Equals][13]                 | (Inherited from [Object][1].)                                                                                                      
![Protected method]              | [Finalize][14]               | Override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources. (Inherited from [Disposable][2].) 
![Public method]                 | [GetHashCode][15]            | (Inherited from [Object][1].)                                                                                                      
![Public method]                 | [GetType][16]                | (Inherited from [Object][1].)                                                                                                      
![Public method]                 | [Join()][17]                 | Block the calling thread until this thread object has completed                                                                    
![Public method]                 | [Join(Int32)][18]            | Block the calling thread until this thread object has completed or until the timeout has occurred                                  
![Protected method]              | [MemberwiseClone][19]        | (Inherited from [Object][1].)                                                                                                      
![Protected method]              | [OnDispose][20]              | Dispose the thread and free resources (Overrides [Disposable.OnDispose()][21].)                                                    
![Protected method]              | [OnDisposeUnmanaged][22]     | Override to release unmanaged objects (Inherited from [Disposable][2].)                                                            
![Public method]                 | [Start(Object[])][23]        | Start the thread                                                                                                                   
![Public method]                 | [Start(Int32, Object[])][24] | Start the thread with a CancellationToken which will timeout in the specified milliseconds                                         
![Public method]                 | [Stop][25]                   | Stop the thread. This calls Cancel on the CancellationToken                                                                        
![Public method]                 | [ToString][26]               | (Inherited from [Object][1].)                                                                                                      


Fields
------

                   | Name              | Description                                                                          
------------------ | ----------------- | ------------------------------------------------------------------------------------ 
![Protected field] | [IsDisposed][27]  | If True, the object has been disposed (Inherited from [Disposable][2].)              
![Protected field] | [IsDisposing][28] | If True, the object is in the process of disposing (Inherited from [Disposable][2].) 


See Also
--------

#### Reference
[W.Threading Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../../W/Disposable/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: Exception.md
[6]: IsComplete.md
[7]: IsFaulted.md
[8]: IsStarted.md
[9]: Task.md
[10]: Token.md
[11]: Create.md
[12]: ../../W/Disposable/Dispose.md
[13]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[14]: ../../W/Disposable/Finalize.md
[15]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[16]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[17]: Join.md
[18]: Join_1.md
[19]: http://msdn.microsoft.com/en-us/library/57ctke0a
[20]: OnDispose.md
[21]: ../../W/Disposable/OnDispose.md
[22]: ../../W/Disposable/OnDisposeUnmanaged.md
[23]: Start_1.md
[24]: Start.md
[25]: Stop.md
[26]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[27]: ../../W/Disposable/IsDisposed.md
[28]: ../../W/Disposable/IsDisposing.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Protected field]: ../../_icons/protfield.gif "Protected field"