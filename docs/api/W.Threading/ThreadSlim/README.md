ThreadSlim Class
================
   A lighter thread class than W.Threading.Thread


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.ThreadSlim**  
    [W.Threading.Thread][2]  

  **Namespace:**  [W.Threading][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class ThreadSlim : IDisposable
```

The **ThreadSlim** type exposes the following members.


Constructors
------------

                 | Name                                          | Description                                                                            
---------------- | --------------------------------------------- | -------------------------------------------------------------------------------------- 
![Public method] | [ThreadSlim(Action&lt;CancellationToken>)][4] | Constructs a new ThreadSlim using an Action&lt;CancellationToken> as the thread method 
![Public method] | [ThreadSlim(ThreadDelegate)][5]               | Constructs a new ThreadSlim object                                                     


Properties
----------

                   | Name            | Description                                                               
------------------ | --------------- | ------------------------------------------------------------------------- 
![Public property] | [IsComplete][6] | True if the thread has completed, otherwise False                         
![Public property] | [IsRunning][7]  | True if the thread is currently running and not complete, otherwise False 
![Public property] | [IsStarted][8]  | True if the thread is running or has completed                            
![Public property] | [Name][9]       | A user-defined name for this object                                       


Methods
-------

                                 | Name                                                                                                                | Description                                                                                                                     
-------------------------------- | ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------- 
![Public method]![Static member] | [Create(Action&lt;CancellationToken>)][10]                                                                          | Constructs a new ThreadSlim using an Action&lt;CancellationToken> as the thread method                                          
![Public method]![Static member] | [Create(ThreadDelegate)][11]                                                                                        | Constructs a new ThreadSlim using an Action&lt;CancellationToken> as the thread method                                          
![Public method]![Static member] | [Create&lt;TArg>(Action&lt;CancellationToken, TArg>)][12]                                                           | Constructs a new ThreadSlim using an Action&lt;CancellationToken> as the thread method                                          
![Public method]![Static member] | [Create&lt;TArg1, TArg2>(Action&lt;CancellationToken, TArg1, TArg2>)][13]                                           | Constructs a new ThreadSlim using an Action&lt;CancellationToken> as the thread method                                          
![Public method]![Static member] | [Create&lt;TArg1, TArg2, TArg3>(Action&lt;CancellationToken, TArg1, TArg2, TArg3>)][14]                             | Constructs a new ThreadSlim using an Action&lt;CancellationToken> as the thread method                                          
![Public method]![Static member] | [Create&lt;TArg1, TArg2, TArg3, TArg4>(Action&lt;CancellationToken, TArg1, TArg2, TArg3, TArg4>)][15]               | Constructs a new ThreadSlim using an Action&lt;CancellationToken> as the thread method                                          
![Public method]![Static member] | [Create&lt;TArg1, TArg2, TArg3, TArg4, TArg5>(Action&lt;CancellationToken, TArg1, TArg2, TArg3, TArg4, TArg5>)][16] | Constructs a new ThreadSlim using an Action&lt;CancellationToken> as the thread method                                          
![Public method]                 | [Dispose][17]                                                                                                       | Releases all resources used by the **ThreadSlim**                                                                               
![Public method]                 | [Equals][18]                                                                                                        | (Inherited from [Object][1].)                                                                                                   
![Protected method]              | [Finalize][19]                                                                                                      | (Inherited from [Object][1].)                                                                                                   
![Public method]                 | [GetHashCode][20]                                                                                                   | (Inherited from [Object][1].)                                                                                                   
![Public method]                 | [GetType][21]                                                                                                       | (Inherited from [Object][1].)                                                                                                   
![Protected method]              | [MemberwiseClone][22]                                                                                               | (Inherited from [Object][1].)                                                                                                   
![Public method]                 | [SignalToStop][23]                                                                                                  | Signals the thread method to stop running                                                                                       
![Public method]                 | [Start][24]                                                                                                         | Starts the thread and waits for it to complete                                                                                  
![Public method]                 | [StartAsync][25]                                                                                                    | Starts the thread and returns the Task associated with it                                                                       
![Public method]                 | [Stop()][26]                                                                                                        | Signals the thread method to stop running and waits for it to complete                                                          
![Public method]                 | [Stop(Int32)][27]                                                                                                   | Signals the thread method to stop running and waits the specified number of milliseconds for it to complete before returning    
![Public method]                 | [Stop(CancellationToken)][28]                                                                                       | Signals the thread method to stop running and waits for the method to complete, while observing the specified CancellationToken 
![Public method]                 | [ToString][29]                                                                                                      | (Inherited from [Object][1].)                                                                                                   
![Public method]                 | [Wait()][30]                                                                                                        | Wait for the thread to complete                                                                                                 
![Public method]                 | [Wait(Int32)][31]                                                                                                   | Wait for the thread to complete                                                                                                 
![Public method]                 | [Wait(CancellationToken)][32]                                                                                       | Wait for the thread to complete                                                                                                 


See Also
--------

#### Reference
[W.Threading Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../Thread/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: _ctor_1.md
[6]: IsComplete.md
[7]: IsRunning.md
[8]: IsStarted.md
[9]: Name.md
[10]: Create.md
[11]: Create_1.md
[12]: Create__1.md
[13]: Create__2.md
[14]: Create__3.md
[15]: Create__4.md
[16]: Create__5.md
[17]: Dispose.md
[18]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[19]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[20]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[21]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[22]: http://msdn.microsoft.com/en-us/library/57ctke0a
[23]: SignalToStop.md
[24]: Start.md
[25]: StartAsync.md
[26]: Stop.md
[27]: Stop_1.md
[28]: Stop_2.md
[29]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[30]: Wait.md
[31]: Wait_1.md
[32]: Wait_2.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"