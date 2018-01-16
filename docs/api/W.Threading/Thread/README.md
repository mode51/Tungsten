Thread Class
============
   A thread class


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.ThreadSlim][2]  
    **W.Threading.Thread**  
      [W.Threading.Thread&lt;TType>][3]  

  **Namespace:**  [W.Threading][4]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Thread : ThreadSlim
```

The **Thread** type exposes the following members.


Constructors
------------

                 | Name                                      | Description                                                                            
---------------- | ----------------------------------------- | -------------------------------------------------------------------------------------- 
![Public method] | [Thread(Action&lt;CancellationToken>)][5] | Constructs a new ThreadSlim using an Action&lt;CancellationToken> as the thread method 
![Public method] | [Thread(ThreadDelegate)][6]               | Constructs a new ThreadSlim using a ThreadDelegate as the thread method                


Properties
----------

                   | Name            | Description                                                                                                 
------------------ | --------------- | ----------------------------------------------------------------------------------------------------------- 
![Public property] | [IsComplete][7] | True if the thread has completed, otherwise False (Inherited from [ThreadSlim][2].)                         
![Public property] | [IsRunning][8]  | True if the thread is currently running and not complete, otherwise False (Inherited from [ThreadSlim][2].) 
![Public property] | [IsStarted][9]  | True if the thread is running or has completed (Inherited from [ThreadSlim][2].)                            
![Public property] | [Name][10]      | A user-defined name for this object (Inherited from [ThreadSlim][2].)                                       


Methods
-------

                                 | Name                                                        | Description                                                                                                                                                                                                                                                                                                        
-------------------------------- | ----------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ 
![Public method]![Static member] | [Create(Action&lt;CancellationToken>)][11]                  |                                                                                                                                                                                                                                                                                                                    
![Public method]![Static member] | [Create&lt;TType>(Action&lt;CancellationToken, TType>)][12] |                                                                                                                                                                                                                                                                                                                    
![Public method]                 | [Dispose][13]                                               | Disposes the Thread and releases resources                                                                                                                                                                                                                                                                         
![Public method]                 | [Equals][14]                                                | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                                      
![Protected method]              | [Finalize][15]                                              | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                                      
![Public method]                 | [GetHashCode][16]                                           | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                                      
![Public method]                 | [GetType][17]                                               | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                                      
![Public method]                 | [Join()][18]                                                |                                                                                                                                                                                                                                                                                                                    
![Public method]                 | [Join(Int32)][19]                                           |                                                                                                                                                                                                                                                                                                                    
![Public method]                 | [Join(CancellationToken)][20]                               |                                                                                                                                                                                                                                                                                                                    
![Protected method]              | [MemberwiseClone][21]                                       | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                                      
![Public method]                 | [SignalToStop][22]                                          | Signals the thread method to stop running (Inherited from [ThreadSlim][2].)                                                                                                                                                                                                                                        
![Public method]![Static member] | [Sleep(Int32)][23]                                          | Blocks the calling thread for the specified time                                                                                                                                                                                                                                                                   
![Public method]![Static member] | [Sleep(CPUProfileEnum)][24]                                 | Attempts to free the CPU for other processes, based on the desired level. Consequences will vary depending on your hardware architecture. The more processors/cores you have, the better performance you will have by selecting LowCPU. Likewise, on a single-core processor, you may wish to select HighCPU.      
![Public method]![Static member] | [Sleep(Int32, Boolean)][25]                                 | Blocks the calling thread for the specified time                                                                                                                                                                                                                                                                   
![Public method]![Static member] | [Sleep(CPUProfileEnum, Int32)][26]                          | Attempts to free the CPU for other processes, based on the desired level. Consequences will vary depending on your hardware architecture. The more processors/cores you have, the better performance you will have by selecting SpinWait1. Likewise, on a single-core processor, you may wish to select SpinWait0. 
![Public method]                 | [Start][27]                                                 | Starts the thread and waits for it to complete (Inherited from [ThreadSlim][2].)                                                                                                                                                                                                                                   
![Public method]                 | [StartAsync][28]                                            | Starts the thread and returns the Task associated with it (Inherited from [ThreadSlim][2].)                                                                                                                                                                                                                        
![Public method]                 | [Stop()][29]                                                | Signals the thread method to stop running and waits for it to complete (Inherited from [ThreadSlim][2].)                                                                                                                                                                                                           
![Public method]                 | [Stop(Int32)][30]                                           | Signals the thread method to stop running and waits the specified number of milliseconds for it to complete before returning (Inherited from [ThreadSlim][2].)                                                                                                                                                     
![Public method]                 | [Stop(CancellationToken)][31]                               | Signals the thread method to stop running and waits for the method to complete, while observing the specified CancellationToken (Inherited from [ThreadSlim][2].)                                                                                                                                                  
![Public method]                 | [ToString][32]                                              | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                                      
![Public method]                 | [Wait()][33]                                                | Wait for the thread to complete (Inherited from [ThreadSlim][2].)                                                                                                                                                                                                                                                  
![Public method]                 | [Wait(Int32)][34]                                           | Wait for the thread to complete (Inherited from [ThreadSlim][2].)                                                                                                                                                                                                                                                  
![Public method]                 | [Wait(CancellationToken)][35]                               | Wait for the thread to complete (Inherited from [ThreadSlim][2].)                                                                                                                                                                                                                                                  


See Also
--------

#### Reference
[W.Threading Namespace][4]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../ThreadSlim/README.md
[3]: ../Thread_1/README.md
[4]: ../README.md
[5]: _ctor.md
[6]: _ctor_1.md
[7]: ../ThreadSlim/IsComplete.md
[8]: ../ThreadSlim/IsRunning.md
[9]: ../ThreadSlim/IsStarted.md
[10]: ../ThreadSlim/Name.md
[11]: Create.md
[12]: Create__1.md
[13]: Dispose.md
[14]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[15]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[16]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[17]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[18]: Join.md
[19]: Join_1.md
[20]: Join_2.md
[21]: http://msdn.microsoft.com/en-us/library/57ctke0a
[22]: ../ThreadSlim/SignalToStop.md
[23]: Sleep.md
[24]: Sleep_2.md
[25]: Sleep_1.md
[26]: Sleep_3.md
[27]: ../ThreadSlim/Start.md
[28]: ../ThreadSlim/StartAsync.md
[29]: ../ThreadSlim/Stop.md
[30]: ../ThreadSlim/Stop_1.md
[31]: ../ThreadSlim/Stop_2.md
[32]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[33]: ../ThreadSlim/Wait.md
[34]: ../ThreadSlim/Wait_1.md
[35]: ../ThreadSlim/Wait_2.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"