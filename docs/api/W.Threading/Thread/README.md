Thread Class
============
   A thread class


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.Thread**  
    [W.Threading.Thread&lt;TParameterType>][2]  

  **Namespace:**  [W.Threading][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Thread : IDisposable
```

The **Thread** type exposes the following members.


Constructors
------------

                 | Name                                               | Description             
---------------- | -------------------------------------------------- | ----------------------- 
![Public method] | [Thread(Action&lt;CancellationToken>)][4]          | Constructs a new Thread 
![Public method] | [Thread(Action&lt;CancellationToken>, Boolean)][5] | Constructs a new Thread 


Properties
----------

                   | Name            | Description                                                           
------------------ | --------------- | --------------------------------------------------------------------- 
![Public property] | [Exception][6]  | The exception, if one was caught                                      
![Public property] | [IsComplete][7] | True if the thread has completed, otherwise False                     
![Public property] | [IsFaulted][8]  | True if the thread raised an exception, otherwise False               
![Public property] | [IsRunning][9]  | **Obsolete.**True if the thread is currently running, otherwise False 
![Public property] | [IsStarted][10] | True if the thread has been started, otherwise False                  
![Public property] | [Token][11]     | Not available until after Start has been called                       


Methods
-------

                                 | Name                                                                                   | Description                                                                                                                                                                                                                                                                                                   
-------------------------------- | -------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Protected method]              | [CallAction][12]                                                                       | Calls the action encapsulated by this thread. This method can be overridden to provide more specific functionality.                                                                                                                                                                                           
![Public method]                 | [Cancel][13]                                                                           | **Obsolete.**Cancel the thread                                                                                                                                                                                                                                                                                
![Public method]![Static member] | [Create(Action&lt;CancellationToken>)][14]                                             | Starts a new thread                                                                                                                                                                                                                                                                                           
![Public method]![Static member] | [Create(Action&lt;CancellationToken>, Boolean)][15]                                    | Starts a new thread                                                                                                                                                                                                                                                                                           
![Public method]![Static member] | [Create&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>)][16]          | Creates and starts a new thread                                                                                                                                                                                                                                                                               
![Public method]![Static member] | [Create&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>, Boolean)][17] | Creates a new thread                                                                                                                                                                                                                                                                                          
![Public method]                 | [Dispose][18]                                                                          | Dispose the thread and free resources                                                                                                                                                                                                                                                                         
![Public method]                 | [Equals][19]                                                                           | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                                 
![Protected method]              | [Finalize][20]                                                                         | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                                 
![Public method]                 | [GetHashCode][21]                                                                      | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                                 
![Public method]                 | [GetType][22]                                                                          | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                                 
![Public method]                 | [Join()][23]                                                                           | Block the calling thread until this thread object has completed                                                                                                                                                                                                                                               
![Public method]                 | [Join(Int32)][24]                                                                      | Block the calling thread until this thread object has completed or until the timeout has occurred                                                                                                                                                                                                             
![Protected method]              | [MemberwiseClone][25]                                                                  | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                                 
![Public method]![Static member] | [Sleep(Int32)][26]                                                                     | Blocks the calling thread for the specified time                                                                                                                                                                                                                                                              
![Public method]![Static member] | [Sleep(CPUProfileEnum)][27]                                                            | Attempts to free the CPU for other processes, based on the desired level. Consequences will vary depending on your hardware architecture. The more processors/cores you have, the better performance you will have by selecting LowCPU. Likewise, on a single-core processor, you may wish to select HighCPU. 
![Public method]![Static member] | [Sleep(Int32, Boolean)][28]                                                            | Blocks the calling thread for the specified time                                                                                                                                                                                                                                                              
![Public method]                 | [Start()][29]                                                                          | Start the thread                                                                                                                                                                                                                                                                                              
![Public method]                 | [Start(Int32)][30]                                                                     | Start the thread with a CancellationToken which will timeout in the specified milliseconds                                                                                                                                                                                                                    
![Public method]                 | [Stop][31]                                                                             | Stop the thread. This calls Cancel on the CancellationToken                                                                                                                                                                                                                                                   
![Public method]                 | [ToString][32]                                                                         | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                                 


Extension Methods
-----------------

                                          | Name                                                                                         | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][33]                                                                           | Use Generic syntax for the as operator. (Defined by [AsExtensions][34].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][35]                                                                       | Serializes an object to a Json string (Defined by [AsExtensions][34].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][36]                                                                        | Serializes an object to an xml string (Defined by [AsExtensions][34].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>)][37]          | Overloaded. Creates and starts a new thread and (Defined by [ThreadExtensions][38].)                                                                                                                                             
![Public Extension Method]                | [CreateThread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>, Boolean)][39] | Overloaded. Creates a new thread (Defined by [ThreadExtensions][38].)                                                                                                                                                            
![Public Extension Method]                | [InitializeProperties][40]                                                                   | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][41].) 
![Public Extension Method]                | [IsDirty][42]                                                                                | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][41].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][43]                                                                            | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][41].)                                                                                                  
![Public Extension Method]                | [WaitForValueAsync][44]                                                                      | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][45].)                                                                                                         


See Also
--------

#### Reference
[W.Threading Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../Thread_1/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: _ctor_1.md
[6]: Exception.md
[7]: IsComplete.md
[8]: IsFaulted.md
[9]: IsRunning.md
[10]: IsStarted.md
[11]: Token.md
[12]: CallAction.md
[13]: Cancel.md
[14]: Create.md
[15]: Create_1.md
[16]: Create__1.md
[17]: Create__1_1.md
[18]: Dispose.md
[19]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[20]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[21]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[22]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[23]: Join.md
[24]: Join_1.md
[25]: http://msdn.microsoft.com/en-us/library/57ctke0a
[26]: Sleep.md
[27]: Sleep_2.md
[28]: Sleep_1.md
[29]: Start.md
[30]: Start_1.md
[31]: Stop.md
[32]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[33]: ../../W/AsExtensions/As__1.md
[34]: ../../W/AsExtensions/README.md
[35]: ../../W/AsExtensions/AsJson__1.md
[36]: ../../W/AsExtensions/AsXml__1.md
[37]: ../ThreadExtensions/CreateThread__1.md
[38]: ../ThreadExtensions/README.md
[39]: ../ThreadExtensions/CreateThread__1_1.md
[40]: ../../W/PropertyHostMethods/InitializeProperties.md
[41]: ../../W/PropertyHostMethods/README.md
[42]: ../../W/PropertyHostMethods/IsDirty.md
[43]: ../../W/PropertyHostMethods/MarkAsClean.md
[44]: ../../W/ExtensionMethods/WaitForValueAsync.md
[45]: ../../W/ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Static member]: ../../_icons/static.gif "Static member"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"