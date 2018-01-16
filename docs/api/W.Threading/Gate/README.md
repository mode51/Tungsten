Gate Class
==========
   

**Note: This API is now obsolete.**
A thread Gate is a background thread which is initially closed. When a Gate is opened, the Action runs until completion. The Gate can be opened (Run) any number of times.


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.Gate**  
    [W.Threading.Gate&lt;TParameterType>][2]  

  **Namespace:**  [W.Threading][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
[ObsoleteAttribute("Gate is incorrectly implemented and will be removed at a later date.  Please use ThreadMethod instead.")]
public class Gate : IDisposable
```

The **Gate** type exposes the following members.


Constructors
------------

                 | Name      | Description           
---------------- | --------- | --------------------- 
![Public method] | [Gate][4] | Constructs a new Gate 


Properties
----------

                   | Name            | Description                                                   
------------------ | --------------- | ------------------------------------------------------------- 
![Public property] | [IsComplete][5] | True if the gated Action has completed, otherwise False       
![Public property] | [IsRunning][6]  | True if the Gate is currently open (running), otherwise False 


Methods
-------

                    | Name                  | Description                                                                                                             
------------------- | --------------------- | ----------------------------------------------------------------------------------------------------------------------- 
![Protected method] | [CallAction][7]       | Invokes the gated Action                                                                                                
![Public method]    | [Cancel][8]           | Singals the gated Action that a Cancel has been requested                                                               
![Public method]    | [Dispose][9]          | Cancels the gated Action, disposes the Gate and releases resources                                                      
![Public method]    | [Equals][10]          | (Inherited from [Object][1].)                                                                                           
![Protected method] | [Finalize][11]        | (Inherited from [Object][1].)                                                                                           
![Public method]    | [GetHashCode][12]     | (Inherited from [Object][1].)                                                                                           
![Public method]    | [GetType][13]         | (Inherited from [Object][1].)                                                                                           
![Public method]    | [Join()][14]          | Blocks the calling thread until the gated Action is complete                                                            
![Public method]    | [Join(Int32)][15]     | Blocks the calling thread until the gated Action is complete, or until the specified number of milliseconds has elapsed 
![Protected method] | [MemberwiseClone][16] | (Inherited from [Object][1].)                                                                                           
![Public method]    | [Run][17]             | Signals the thread to open the gate (allows the gated Action to be called).                                             
![Public method]    | [ToString][18]        | (Inherited from [Object][1].)                                                                                           


See Also
--------

#### Reference
[W.Threading Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../Gate_1/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: IsComplete.md
[6]: IsRunning.md
[7]: CallAction.md
[8]: Cancel.md
[9]: Dispose.md
[10]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[11]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[12]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[13]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[14]: Join.md
[15]: Join_1.md
[16]: http://msdn.microsoft.com/en-us/library/57ctke0a
[17]: Run.md
[18]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"