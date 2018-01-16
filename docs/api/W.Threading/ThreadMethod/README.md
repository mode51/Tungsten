ThreadMethod Class
==================
   Adds multi-threading and additional functionality to an Action or ThreadMethodDelegate


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.ThreadMethod**  

  **Namespace:**  [W.Threading][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class ThreadMethod : IDisposable
```

The **ThreadMethod** type exposes the following members.


Constructors
------------

                 | Name              | Description                  
---------------- | ----------------- | ---------------------------- 
![Public method] | [ThreadMethod][3] | Construct a new ThreadMethod 


Properties
----------

                   | Name            | Description                                                               
------------------ | --------------- | ------------------------------------------------------------------------- 
![Public property] | [IsComplete][4] | True if the method has completed, otherwise False                         
![Public property] | [IsRunning][5]  | True if the method is currently running and not complete, otherwise False 
![Public property] | [IsStarted][6]  | True if the method is running or has completed                            
![Public property] | [Name][7]       | A user-defined name for this object                                       


Methods
-------

                                 | Name                                                               | Description                                                                            
-------------------------------- | ------------------------------------------------------------------ | -------------------------------------------------------------------------------------- 
![Public method]![Static member] | [Create(Action)][8]                                                | Creates a new ThreadMethod from the Action                                             
![Public method]![Static member] | [Create(AnyMethodDelegate)][9]                                     | Creates a new ThreadMethod from the ThreadMethodDelegate                               
![Public method]![Static member] | [Create&lt;T>(Action&lt;T>)][10]                                   | Creates a new ThreadMethod from the Action                                             
![Public method]![Static member] | [Create&lt;T1, T2>(Action&lt;T1, T2>)][11]                         | Creates a new ThreadMethod from the Action                                             
![Public method]![Static member] | [Create&lt;T1, T2, T3>(Action&lt;T1, T2, T3>)][12]                 | Creates a new ThreadMethod from the Action                                             
![Public method]![Static member] | [Create&lt;T1, T2, T3, T4>(Action&lt;T1, T2, T3, T4>)][13]         | Creates a new ThreadMethod from the Action                                             
![Public method]![Static member] | [Create&lt;T1, T2, T3, T4, T5>(Action&lt;T1, T2, T3, T4, T5>)][14] | Creates a new ThreadMethod from the Action                                             
![Public method]                 | [Dispose][15]                                                      | Disposes the ThreadMethod and releases resources                                       
![Public method]                 | [Equals][16]                                                       | (Inherited from [Object][1].)                                                          
![Protected method]              | [Finalize][17]                                                     | Destructs the ThreadMethod and releases resources (Overrides [Object.Finalize()][18].) 
![Public method]                 | [GetHashCode][19]                                                  | (Inherited from [Object][1].)                                                          
![Public method]                 | [GetType][20]                                                      | (Inherited from [Object][1].)                                                          
![Protected method]              | [Initialize][21]                                                   | Initializes variables and creates the task associated with this ThreadMethod           
![Protected method]              | [MemberwiseClone][22]                                              | (Inherited from [Object][1].)                                                          
![Public method]                 | [RunSynchronously][23]                                             | Synchronously runs the method                                                          
![Public method]                 | [Start][24]                                                        | Asynchronously runs the method                                                         
![Public method]                 | [StartAsync][25]                                                   | Asynchronously runs the method                                                         
![Public method]                 | [ToString][26]                                                     | (Inherited from [Object][1].)                                                          
![Public method]                 | [Wait()][27]                                                       | Waits for the thread method to complete                                                
![Public method]                 | [Wait(Int32)][28]                                                  | Waits for the thread method to complete                                                
![Public method]                 | [Wait(CancellationToken)][29]                                      | Waits for the thread method to complete                                                


See Also
--------

#### Reference
[W.Threading Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: IsComplete.md
[5]: IsRunning.md
[6]: IsStarted.md
[7]: Name.md
[8]: Create.md
[9]: Create_1.md
[10]: Create__1.md
[11]: Create__2.md
[12]: Create__3.md
[13]: Create__4.md
[14]: Create__5.md
[15]: Dispose.md
[16]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[17]: Finalize.md
[18]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[19]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[20]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[21]: Initialize.md
[22]: http://msdn.microsoft.com/en-us/library/57ctke0a
[23]: RunSynchronously.md
[24]: Start.md
[25]: StartAsync.md
[26]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[27]: Wait.md
[28]: Wait_1.md
[29]: Wait_2.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"