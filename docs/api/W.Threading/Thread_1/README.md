Thread&lt;TType> Class
======================
   A thread class which can pass a typed parameter into the thread method


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.ThreadSlim][2]  
    [W.Threading.Thread][3]  
      **W.Threading.Thread<TType>**  

  **Namespace:**  [W.Threading][4]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Thread<TType> : Thread

```

#### Type Parameters

##### *TType*
The argument Type to be passed into the thread method

The **Thread<TType>** type exposes the following members.


Constructors
------------

                 | Name                  | Description                                                        
---------------- | --------------------- | ------------------------------------------------------------------ 
![Public method] | [Thread&lt;TType>][5] | Constructs a new Thread which can accept a single, typed, paramter 


Properties
----------

                   | Name            | Description                                                                                                 
------------------ | --------------- | ----------------------------------------------------------------------------------------------------------- 
![Public property] | [IsComplete][6] | True if the thread has completed, otherwise False (Inherited from [ThreadSlim][2].)                         
![Public property] | [IsRunning][7]  | True if the thread is currently running and not complete, otherwise False (Inherited from [ThreadSlim][2].) 
![Public property] | [IsStarted][8]  | True if the thread is running or has completed (Inherited from [ThreadSlim][2].)                            
![Public property] | [Name][9]       | A user-defined name for this object (Inherited from [ThreadSlim][2].)                                       


Methods
-------

                    | Name                          | Description                                                                                                                                                       
------------------- | ----------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Dispose][10]                 | Disposes the Thread and releases resources (Inherited from [Thread][3].)                                                                                          
![Public method]    | [Equals][11]                  | (Inherited from [Object][1].)                                                                                                                                     
![Protected method] | [Finalize][12]                | (Inherited from [Object][1].)                                                                                                                                     
![Public method]    | [GetHashCode][13]             | (Inherited from [Object][1].)                                                                                                                                     
![Public method]    | [GetType][14]                 | (Inherited from [Object][1].)                                                                                                                                     
![Public method]    | [Join()][15]                  | (Inherited from [Thread][3].)                                                                                                                                     
![Public method]    | [Join(Int32)][16]             | (Inherited from [Thread][3].)                                                                                                                                     
![Public method]    | [Join(CancellationToken)][17] | (Inherited from [Thread][3].)                                                                                                                                     
![Protected method] | [MemberwiseClone][18]         | (Inherited from [Object][1].)                                                                                                                                     
![Public method]    | [SignalToStop][19]            | Signals the thread method to stop running (Inherited from [ThreadSlim][2].)                                                                                       
![Public method]    | [Start][20]                   | Starts the thread and waits for it to complete (Inherited from [ThreadSlim][2].)                                                                                  
![Public method]    | [StartAsync(TType)][21]       | Starts the thread and returns the Task associated with it                                                                                                         
![Public method]    | [StartAsync(Object[])][22]    | Starts the thread and returns the Task associated with it (Inherited from [ThreadSlim][2].)                                                                       
![Public method]    | [Stop()][23]                  | Signals the thread method to stop running and waits for it to complete (Inherited from [ThreadSlim][2].)                                                          
![Public method]    | [Stop(Int32)][24]             | Signals the thread method to stop running and waits the specified number of milliseconds for it to complete before returning (Inherited from [ThreadSlim][2].)    
![Public method]    | [Stop(CancellationToken)][25] | Signals the thread method to stop running and waits for the method to complete, while observing the specified CancellationToken (Inherited from [ThreadSlim][2].) 
![Public method]    | [ToString][26]                | (Inherited from [Object][1].)                                                                                                                                     
![Public method]    | [Wait()][27]                  | Wait for the thread to complete (Inherited from [ThreadSlim][2].)                                                                                                 
![Public method]    | [Wait(Int32)][28]             | Wait for the thread to complete (Inherited from [ThreadSlim][2].)                                                                                                 
![Public method]    | [Wait(CancellationToken)][29] | Wait for the thread to complete (Inherited from [ThreadSlim][2].)                                                                                                 


See Also
--------

#### Reference
[W.Threading Namespace][4]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../ThreadSlim/README.md
[3]: ../Thread/README.md
[4]: ../README.md
[5]: _ctor.md
[6]: ../ThreadSlim/IsComplete.md
[7]: ../ThreadSlim/IsRunning.md
[8]: ../ThreadSlim/IsStarted.md
[9]: ../ThreadSlim/Name.md
[10]: ../Thread/Dispose.md
[11]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[12]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[13]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[14]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[15]: ../Thread/Join.md
[16]: ../Thread/Join_1.md
[17]: ../Thread/Join_2.md
[18]: http://msdn.microsoft.com/en-us/library/57ctke0a
[19]: ../ThreadSlim/SignalToStop.md
[20]: ../ThreadSlim/Start.md
[21]: StartAsync.md
[22]: ../ThreadSlim/StartAsync.md
[23]: ../ThreadSlim/Stop.md
[24]: ../ThreadSlim/Stop_1.md
[25]: ../ThreadSlim/Stop_2.md
[26]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[27]: ../ThreadSlim/Wait.md
[28]: ../ThreadSlim/Wait_1.md
[29]: ../ThreadSlim/Wait_2.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"