Thread Class
============
  A thread wrapper which makes multi-threading easier


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.ThreadBase][2]  
    **W.Threading.Thread**  
      [W.Threading.Gate][3]  
      [W.Threading.Thread&lt;TCustomData>][4]  

  **Namespace:**  [W.Threading][5]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Thread : ThreadBase
```

The **Thread** type exposes the following members.


Constructors
------------

                 | Name        | Description                  
---------------- | ----------- | ---------------------------- 
![Public method] | [Thread][6] | Constructs the Thread object 


Properties
----------

                      | Name            | Description                                                                                                                         
--------------------- | --------------- | ----------------------------------------------------------------------------------------------------------------------------------- 
![Protected property] | [Action][7]     | The Action to execute on the thread (Inherited from [ThreadBase][2].)                                                               
![Protected property] | [Cts][8]        | The CancellationTokenSource which can be used to cancel the thread (Inherited from [ThreadBase][2].)                                
![Protected property] | [IsBusy][9]     | Value is True if the thread is currently running, otherwise False (Inherited from [ThreadBase][2].)                                 
![Public property]    | [IsRunning][10] | True if the thread is running, otherwise false (Inherited from [ThreadBase][2].)                                                    
![Protected property] | [OnExit][11]    | The Action to execute when the thread completes (Inherited from [ThreadBase][2].)                                                   
![Protected property] | [Success][12]   | The Value to send to the onExit Action. True if the thread returns successfully, otherwise False. (Inherited from [ThreadBase][2].) 


Methods
-------

                                 | Name                       | Description                                                                                                                                 
-------------------------------- | -------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------- 
![Protected method]              | [CallInvokeAction][13]     | Called by the host thread procedure, this method calls the Action (Overrides [ThreadBase.CallInvokeAction()][14].)                          
![Protected method]              | [CallInvokeOnComplete][15] | Calls the onExit Action when the thread returns (Overrides [ThreadBase.CallInvokeOnComplete(Exception)][16].)                               
![Public method]                 | [Cancel()][17]             | (Overrides [ThreadBase.Cancel()][18].)                                                                                                      
![Public method]                 | [Cancel(Int32)][19]        | (Overrides [ThreadBase.Cancel(Int32)][20].)                                                                                                 
![Public method]![Static member] | [Create][21]               | Starts a new thread                                                                                                                         
![Public method]                 | [Dispose][22]              | Releases all resources used by the **Thread** (Overrides [ThreadBase.Dispose()][23].)                                                       
![Public method]                 | [Equals][24]               | (Inherited from [Object][1].)                                                                                                               
![Protected method]              | [Finalize][25]             | Destructs the ThreadBase object. Calls Dispose. (Inherited from [ThreadBase][2].)                                                           
![Public method]                 | [GetHashCode][26]          | (Inherited from [Object][1].)                                                                                                               
![Public method]                 | [GetType][27]              | (Inherited from [Object][1].)                                                                                                               
![Protected method]              | [InvokeAction][28]         | Invokes the Action. Virtual for customization. (Inherited from [ThreadBase][2].)                                                            
![Protected method]              | [InvokeOnComplete][29]     | Invokes the onExit action. Virtual for customization. (Inherited from [ThreadBase][2].)                                                     
![Public method]                 | [Join()][30]               | Blocks the calling thread until the thread terminates (Overrides [ThreadBase.Join()][31].)                                                  
![Public method]                 | [Join(Int32)][32]          | Blocks the calling thread until either the thread terminates or the specified milliseconds elapse (Overrides [ThreadBase.Join(Int32)][33].) 
![Protected method]              | [MemberwiseClone][34]      | (Inherited from [Object][1].)                                                                                                               
![Public method]![Static member] | [Sleep][35]                | Blocks the calling thread for the specified time                                                                                            
![Protected method]              | [ThreadProc][36]           | The host thread procedure. This method calls the Action and subsequent onExit. (Inherited from [ThreadBase][2].)                            
![Public method]                 | [ToString][37]             | (Inherited from [Object][1].)                                                                                                               


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][38]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][39].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][40]     | Serializes an object to a Json string (Defined by [AsExtensions][39].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][41]      | Serializes an object to an xml string (Defined by [AsExtensions][39].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][42]   | Starts a new thread (Defined by [ThreadExtensions][43].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][44] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][45].) 
![Public Extension Method]                | [IsDirty][46]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][45].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][47]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][45].)                                                                                                  
![Public Extension Method]                | [WaitForValue][48]         | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][49].)                                                                                                         


See Also
--------

#### Reference
[W.Threading Namespace][5]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../ThreadBase/README.md
[3]: ../Gate/README.md
[4]: ../Thread_1/README.md
[5]: ../README.md
[6]: _ctor.md
[7]: ../ThreadBase/Action.md
[8]: ../ThreadBase/Cts.md
[9]: ../ThreadBase/IsBusy.md
[10]: ../ThreadBase/IsRunning.md
[11]: ../ThreadBase/OnExit.md
[12]: ../ThreadBase/Success.md
[13]: CallInvokeAction.md
[14]: ../ThreadBase/CallInvokeAction.md
[15]: CallInvokeOnComplete.md
[16]: ../ThreadBase/CallInvokeOnComplete.md
[17]: Cancel.md
[18]: ../ThreadBase/Cancel.md
[19]: Cancel_1.md
[20]: ../ThreadBase/Cancel_1.md
[21]: Create.md
[22]: Dispose.md
[23]: ../ThreadBase/Dispose.md
[24]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[25]: ../ThreadBase/Finalize.md
[26]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[27]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[28]: ../ThreadBase/InvokeAction.md
[29]: ../ThreadBase/InvokeOnComplete.md
[30]: Join.md
[31]: ../ThreadBase/Join.md
[32]: Join_1.md
[33]: ../ThreadBase/Join_1.md
[34]: http://msdn.microsoft.com/en-us/library/57ctke0a
[35]: Sleep.md
[36]: ../ThreadBase/ThreadProc.md
[37]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[38]: ../../W/AsExtensions/As__1.md
[39]: ../../W/AsExtensions/README.md
[40]: ../../W/AsExtensions/AsJson__1.md
[41]: ../../W/AsExtensions/AsXml__1.md
[42]: ../ThreadExtensions/CreateThread__1.md
[43]: ../ThreadExtensions/README.md
[44]: ../../W/PropertyHostMethods/InitializeProperties.md
[45]: ../../W/PropertyHostMethods/README.md
[46]: ../../W/PropertyHostMethods/IsDirty.md
[47]: ../../W/PropertyHostMethods/MarkAsClean.md
[48]: ../../W/ExtensionMethods/WaitForValue.md
[49]: ../../W/ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected property]: ../../_icons/protproperty.gif "Protected property"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Static member]: ../../_icons/static.gif "Static member"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"