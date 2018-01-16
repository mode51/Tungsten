Gate&lt;TParameterType> Class
=============================
   

**Note: This API is now obsolete.**
A thread Gate which supports passing in a typed parameter


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.Gate][2]  
    **W.Threading.Gate<TParameterType>**  

  **Namespace:**  [W.Threading][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
[ObsoleteAttribute("Gate is incorrectly implemented and will be removed at a later date.  Please use ThreadMethod instead.")]
public class Gate<TParameterType> : Gate

```

#### Type Parameters

##### *TParameterType*
The type of parameter that will be passed to the gated thread procedure

The **Gate<TParameterType>** type exposes the following members.


Constructors
------------

                 | Name                                                                                       | Description           
---------------- | ------------------------------------------------------------------------------------------ | --------------------- 
![Public method] | [Gate&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>)][4]                 | Constructs a new Gate 
![Public method] | [Gate&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>, TParameterType)][5] | Constructs a new Gate 


Properties
----------

                   | Name            | Description                                                                               
------------------ | --------------- | ----------------------------------------------------------------------------------------- 
![Public property] | [IsComplete][6] | True if the gated Action has completed, otherwise False (Inherited from [Gate][2].)       
![Public property] | [IsRunning][7]  | True if the Gate is currently open (running), otherwise False (Inherited from [Gate][2].) 


Methods
-------

                    | Name                      | Description                                                                                                                                              
------------------- | ------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Protected method] | [CallAction][8]           | Invokes the gated Action (Overrides [Gate.CallAction(CancellationToken)][9].)                                                                            
![Public method]    | [Cancel][10]              | Singals the gated Action that a Cancel has been requested (Inherited from [Gate][2].)                                                                    
![Public method]    | [Dispose][11]             | Cancels the gated Action, disposes the Gate and releases resources (Inherited from [Gate][2].)                                                           
![Public method]    | [Equals][12]              | (Inherited from [Object][1].)                                                                                                                            
![Protected method] | [Finalize][13]            | (Inherited from [Object][1].)                                                                                                                            
![Public method]    | [GetHashCode][14]         | (Inherited from [Object][1].)                                                                                                                            
![Public method]    | [GetType][15]             | (Inherited from [Object][1].)                                                                                                                            
![Public method]    | [Join()][16]              | Blocks the calling thread until the gated Action is complete (Inherited from [Gate][2].)                                                                 
![Public method]    | [Join(Int32)][17]         | Blocks the calling thread until the gated Action is complete, or until the specified number of milliseconds has elapsed (Inherited from [Gate][2].)      
![Protected method] | [MemberwiseClone][18]     | (Inherited from [Object][1].)                                                                                                                            
![Public method]    | [Run()][19]               | Opens the gate (allows the gated Action to be called), passing in the default value which was specified in the constructor (Overrides [Gate.Run()][20].) 
![Public method]    | [Run(TParameterType)][21] | Opens the gate (allows the gated Action to be called), passing in the specified typed value                                                              
![Public method]    | [ToString][22]            | (Inherited from [Object][1].)                                                                                                                            


See Also
--------

#### Reference
[W.Threading Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../Gate/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: _ctor_1.md
[6]: ../Gate/IsComplete.md
[7]: ../Gate/IsRunning.md
[8]: CallAction.md
[9]: ../Gate/CallAction.md
[10]: ../Gate/Cancel.md
[11]: ../Gate/Dispose.md
[12]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[13]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[14]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[15]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[16]: ../Gate/Join.md
[17]: ../Gate/Join_1.md
[18]: http://msdn.microsoft.com/en-us/library/57ctke0a
[19]: Run.md
[20]: ../Gate/Run.md
[21]: Run_1.md
[22]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"