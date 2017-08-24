Gate Class
==========
  
A Gated thread. Execution of the Action will proceed when the Run method is called.



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.ThreadBase][2]  
    [W.Threading.Thread][3]  
      **W.Threading.Gate**  

  **Namespace:**  [W.Threading][4]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Gate : Thread
```

The **Gate** type exposes the following members.


Constructors
------------

                 | Name      | Description      
---------------- | --------- | ---------------- 
![Public method] | [Gate][5] | Construct a Gate 


Properties
----------

                      | Name           | Description                                                                                                                         
--------------------- | -------------- | ----------------------------------------------------------------------------------------------------------------------------------- 
![Protected property] | [Action][6]    | The Action to execute on the thread (Inherited from [ThreadBase][2].)                                                               
![Protected property] | [Cts][7]       | The CancellationTokenSource which can be used to cancel the thread (Inherited from [ThreadBase][2].)                                
![Protected property] | [IsBusy][8]    | Value is True if the thread is currently running, otherwise False (Inherited from [ThreadBase][2].)                                 
![Public property]    | [IsRunning][9] | True if the thread is running, otherwise false (Inherited from [ThreadBase][2].)                                                    
![Protected property] | [OnExit][10]   | The Action to execute when the thread completes (Inherited from [ThreadBase][2].)                                                   
![Protected property] | [Success][11]  | The Value to send to the onExit Action. True if the thread returns successfully, otherwise False. (Inherited from [ThreadBase][2].) 


Methods
-------

                    | Name                       | Description                                                                                                                                       
------------------- | -------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------- 
![Protected method] | [CallInvokeAction][12]     | 
Used to wrap the call to InvokeAction with try/catch handlers. This method should call InvokeAction.
 (Overrides [Thread.CallInvokeAction()][13].) 
![Protected method] | [CallInvokeOnComplete][14] | Calls the onExit Action when the thread returns (Inherited from [Thread][3].)                                                                     
![Public method]    | [Cancel()][15]             | Signals the task to cancel                                                                                                                        
![Public method]    | [Cancel(Int32)][16]        | (Inherited from [Thread][3].)                                                                                                                     
![Public method]    | [Dispose][17]              | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. (Overrides [Thread.Dispose()][18].)      
![Public method]    | [Equals][19]               | (Inherited from [Object][1].)                                                                                                                     
![Protected method] | [Finalize][20]             | Destructs the ThreadBase object. Calls Dispose. (Inherited from [ThreadBase][2].)                                                                 
![Public method]    | [GetHashCode][21]          | (Inherited from [Object][1].)                                                                                                                     
![Public method]    | [GetType][22]              | (Inherited from [Object][1].)                                                                                                                     
![Protected method] | [InvokeAction][23]         | Invokes the Action. Virtual for customization. (Inherited from [ThreadBase][2].)                                                                  
![Protected method] | [InvokeOnComplete][24]     | Invokes the onExit action. Virtual for customization. (Inherited from [ThreadBase][2].)                                                           
![Public method]    | [Join()][25]               | Blocks the calling thread until the thread terminates (Overrides [Thread.Join()][26].)                                                            
![Public method]    | [Join(Int32)][27]          | Blocks the calling thread until either the thread terminates or the specified milliseconds elapse (Overrides [Thread.Join(Int32)][28].)           
![Protected method] | [MemberwiseClone][29]      | (Inherited from [Object][1].)                                                                                                                     
![Public method]    | [Run][30]                  | Allows the Action to be called                                                                                                                    
![Protected method] | [ThreadProc][31]           | The host thread procedure. This method calls the Action and subsequent onExit. (Inherited from [ThreadBase][2].)                                  
![Public method]    | [ToString][32]             | (Inherited from [Object][1].)                                                                                                                     


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][33]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][34].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][35]     | Serializes an object to a Json string (Defined by [AsExtensions][34].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][36]      | Serializes an object to an xml string (Defined by [AsExtensions][34].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][37]   | Starts a new thread (Defined by [ThreadExtensions][38].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][39] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][40].) 
![Public Extension Method]                | [IsDirty][41]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][40].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][42]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][40].)                                                                                                  
![Public Extension Method]                | [WaitForValue][43]         | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][44].)                                                                                                         


See Also
--------

#### Reference
[W.Threading Namespace][4]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../ThreadBase/README.md
[3]: ../Thread/README.md
[4]: ../README.md
[5]: _ctor.md
[6]: ../ThreadBase/Action.md
[7]: ../ThreadBase/Cts.md
[8]: ../ThreadBase/IsBusy.md
[9]: ../ThreadBase/IsRunning.md
[10]: ../ThreadBase/OnExit.md
[11]: ../ThreadBase/Success.md
[12]: CallInvokeAction.md
[13]: ../Thread/CallInvokeAction.md
[14]: ../Thread/CallInvokeOnComplete.md
[15]: Cancel.md
[16]: ../Thread/Cancel_1.md
[17]: Dispose.md
[18]: ../Thread/Dispose.md
[19]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[20]: ../ThreadBase/Finalize.md
[21]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[22]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[23]: ../ThreadBase/InvokeAction.md
[24]: ../ThreadBase/InvokeOnComplete.md
[25]: Join.md
[26]: ../Thread/Join.md
[27]: Join_1.md
[28]: ../Thread/Join_1.md
[29]: http://msdn.microsoft.com/en-us/library/57ctke0a
[30]: Run.md
[31]: ../ThreadBase/ThreadProc.md
[32]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[33]: ../../W/AsExtensions/As__1.md
[34]: ../../W/AsExtensions/README.md
[35]: ../../W/AsExtensions/AsJson__1.md
[36]: ../../W/AsExtensions/AsXml__1.md
[37]: ../ThreadExtensions/CreateThread__1.md
[38]: ../ThreadExtensions/README.md
[39]: ../../W/PropertyHostMethods/InitializeProperties.md
[40]: ../../W/PropertyHostMethods/README.md
[41]: ../../W/PropertyHostMethods/IsDirty.md
[42]: ../../W/PropertyHostMethods/MarkAsClean.md
[43]: ../../W/ExtensionMethods/WaitForValue.md
[44]: ../../W/ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected property]: ../../_icons/protproperty.gif "Protected property"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"