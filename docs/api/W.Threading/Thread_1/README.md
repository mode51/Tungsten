Thread&lt;TParameterType> Class
===============================
   A thread class which can pass a typed parameter into the thread action


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.Thread][2]  
    **W.Threading.Thread<TParameterType>**  

  **Namespace:**  [W.Threading][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Thread<TParameterType> : Thread

```

#### Type Parameters

##### *TParameterType*
The argument Type to be passed into the thread action

The **Thread<TParameterType>** type exposes the following members.


Constructors
------------

                 | Name                                                                                                  | Description                                          
---------------- | ----------------------------------------------------------------------------------------------------- | ---------------------------------------------------- 
![Public method] | [Thread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>)][4]                          | Constructs a new Thread which can accept a parameter 
![Public method] | [Thread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>, Boolean)][5]                 | Constructs a new Thread which can accept a parameter 
![Public method] | [Thread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>, TParameterType)][6]          | Constructs a new Thread which can accept a parameter 
![Public method] | [Thread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>, TParameterType, Boolean)][7] | Constructs a new Thread which can accept a parameter 


Properties
----------

                   | Name            | Description                                                                                         
------------------ | --------------- | --------------------------------------------------------------------------------------------------- 
![Public property] | [Exception][8]  | The exception, if one was caught (Inherited from [Thread][2].)                                      
![Public property] | [IsComplete][9] | True if the thread has completed, otherwise False (Inherited from [Thread][2].)                     
![Public property] | [IsFaulted][10] | True if the thread raised an exception, otherwise False (Inherited from [Thread][2].)               
![Public property] | [IsRunning][11] |  **Obsolete.**True if the thread is currently running, otherwise False (Inherited from [Thread][2].) 
![Public property] | [IsStarted][12] | True if the thread has been started, otherwise False (Inherited from [Thread][2].)                  
![Public property] | [Token][13]     | Not available until after Start has been called (Inherited from [Thread][2].)                       


Methods
-------

                    | Name                               | Description                                                                                                                                                                 
------------------- | ---------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Protected method] | [CallAction][14]                   | Calls the action encapsulated by this thread. This method can be overridden to provide more specific functionality. (Overrides [Thread.CallAction(CancellationToken)][15].) 
![Public method]    | [Cancel][16]                       |  **Obsolete.**Cancel the thread (Inherited from [Thread][2].)                                                                                                               
![Public method]    | [Dispose][17]                      | Dispose the thread and free resources (Inherited from [Thread][2].)                                                                                                         
![Public method]    | [Equals][18]                       | (Inherited from [Object][1].)                                                                                                                                               
![Protected method] | [Finalize][19]                     | (Inherited from [Object][1].)                                                                                                                                               
![Public method]    | [GetHashCode][20]                  | (Inherited from [Object][1].)                                                                                                                                               
![Public method]    | [GetType][21]                      | (Inherited from [Object][1].)                                                                                                                                               
![Public method]    | [Join()][22]                       | Block the calling thread until this thread object has completed (Inherited from [Thread][2].)                                                                               
![Public method]    | [Join(Int32)][23]                  | Block the calling thread until this thread object has completed or until the timeout has occurred (Inherited from [Thread][2].)                                             
![Protected method] | [MemberwiseClone][24]              | (Inherited from [Object][1].)                                                                                                                                               
![Public method]    | [Start()][25]                      | Starts the thread if it's not already running (Overrides [Thread.Start()][26].)                                                                                             
![Public method]    | [Start(Int32)][27]                 | Starts the thread if it's not already running (Overrides [Thread.Start(Int32)][28].)                                                                                        
![Public method]    | [Start(TParameterType)][29]        | Starts the thread if it's not already running, passing in the specified argument                                                                                            
![Public method]    | [Start(TParameterType, Int32)][30] | Starts the thread if it's not already running                                                                                                                               
![Public method]    | [Stop][31]                         | Stop the thread. This calls Cancel on the CancellationToken (Inherited from [Thread][2].)                                                                                   
![Public method]    | [ToString][32]                     | (Inherited from [Object][1].)                                                                                                                                               


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
[2]: ../Thread/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: _ctor_1.md
[6]: _ctor_2.md
[7]: _ctor_3.md
[8]: ../Thread/Exception.md
[9]: ../Thread/IsComplete.md
[10]: ../Thread/IsFaulted.md
[11]: ../Thread/IsRunning.md
[12]: ../Thread/IsStarted.md
[13]: ../Thread/Token.md
[14]: CallAction.md
[15]: ../Thread/CallAction.md
[16]: ../Thread/Cancel.md
[17]: ../Thread/Dispose.md
[18]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[19]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[20]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[21]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[22]: ../Thread/Join.md
[23]: ../Thread/Join_1.md
[24]: http://msdn.microsoft.com/en-us/library/57ctke0a
[25]: Start.md
[26]: ../Thread/Start.md
[27]: Start_1.md
[28]: ../Thread/Start_1.md
[29]: Start_2.md
[30]: Start_3.md
[31]: ../Thread/Stop.md
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
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"