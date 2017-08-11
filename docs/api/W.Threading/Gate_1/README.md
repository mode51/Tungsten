Gate&lt;T> Class
================
  
A Gated thread. Execution of the Action will proceed when the Run method is called.



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.ThreadBase][2]  
    [W.Threading.Thread][3]  
      [W.Threading.Thread][4]&lt;**T**>  
        **W.Threading.Gate<T>**  

  **Namespace:**  [W.Threading][5]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Gate<T> : Thread<T>

```

#### Type Parameters

##### *T*

[Missing &lt;typeparam name="T"/> documentation for "T:W.Threading.Gate`1"]


The **Gate<T>** type exposes the following members.


Constructors
------------

                 | Name            | Description      
---------------- | --------------- | ---------------- 
![Public method] | [Gate&lt;T>][6] | Construct a Gate 


Properties
----------

                      | Name            | Description                                                                                                                         
--------------------- | --------------- | ----------------------------------------------------------------------------------------------------------------------------------- 
![Protected property] | [Action][7]     | The Action to be run on a new thread (Inherited from [Thread&lt;TCustomData>][4].)                                                  
![Protected property] | [Cts][8]        | The CancellationTokenSource which can be used to cancel the thread (Inherited from [ThreadBase][2].)                                
![Public property]    | [CustomData][9] | The custom data to pass into the Action (Inherited from [Thread&lt;TCustomData>][4].)                                               
![Protected property] | [IsBusy][10]    | Value is True if the thread is currently running, otherwise False (Inherited from [ThreadBase][2].)                                 
![Public property]    | [IsRunning][11] | True if the thread is running, otherwise false (Inherited from [ThreadBase][2].)                                                    
![Protected property] | [OnExit][12]    | The Action to execute when the thread completes (Inherited from [ThreadBase][2].)                                                   
![Protected property] | [Success][13]   | The Value to send to the onExit Action. True if the thread returns successfully, otherwise False. (Inherited from [ThreadBase][2].) 


Methods
-------

                    | Name                       | Description                                                                                                                                       
------------------- | -------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------- 
![Protected method] | [CallInvokeAction][14]     | 
Used to wrap the call to InvokeAction with try/catch handlers. This method should call InvokeAction.
 (Overrides [Thread.CallInvokeAction()][15].) 
![Protected method] | [CallInvokeOnComplete][16] | Calls the onExit Action when the thread returns (Inherited from [Thread][3].)                                                                     
![Public method]    | [Cancel()][17]             | Signals the task to cancel                                                                                                                        
![Public method]    | [Cancel(Int32)][18]        | (Inherited from [Thread][3].)                                                                                                                     
![Public method]    | [Dispose][19]              | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. (Overrides [Thread.Dispose()][20].)      
![Public method]    | [Equals][21]               | (Inherited from [Object][1].)                                                                                                                     
![Protected method] | [Finalize][22]             | Destructs the ThreadBase object. Calls Dispose. (Inherited from [ThreadBase][2].)                                                                 
![Public method]    | [GetHashCode][23]          | (Inherited from [Object][1].)                                                                                                                     
![Public method]    | [GetType][24]              | (Inherited from [Object][1].)                                                                                                                     
![Protected method] | [InvokeAction][25]         | Invokes the Action (Inherited from [Thread&lt;TCustomData>][4].)                                                                                  
![Protected method] | [InvokeOnComplete][26]     | Invokes the onExit action. Virtual for customization. (Inherited from [ThreadBase][2].)                                                           
![Public method]    | [Join()][27]               | Blocks the calling thread until the thread terminates (Overrides [Thread.Join()][28].)                                                            
![Public method]    | [Join(Int32)][29]          | Blocks the calling thread until either the thread terminates or the specified milliseconds elapse (Overrides [Thread.Join(Int32)][30].)           
![Protected method] | [MemberwiseClone][31]      | (Inherited from [Object][1].)                                                                                                                     
![Public method]    | [Run][32]                  | Allows the Action to be called                                                                                                                    
![Protected method] | [ThreadProc][33]           | The host thread procedure. This method calls the Action and subsequent onExit. (Inherited from [ThreadBase][2].)                                  
![Public method]    | [ToString][34]             | (Inherited from [Object][1].)                                                                                                                     


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][35]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][36].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][37]     | Serializes an object to a Json string (Defined by [AsExtensions][36].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][38]      | Serializes an object to an xml string (Defined by [AsExtensions][36].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][39]   | Starts a new thread (Defined by [ThreadExtensions][40].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][41] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][42].) 
![Public Extension Method]                | [IsDirty][43]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][42].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][44]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][42].)                                                                                                  


See Also
--------

#### Reference
[W.Threading Namespace][5]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../ThreadBase/README.md
[3]: ../Thread/README.md
[4]: ../Thread_1/README.md
[5]: ../README.md
[6]: _ctor.md
[7]: ../Thread_1/Action.md
[8]: ../ThreadBase/Cts.md
[9]: ../Thread_1/CustomData.md
[10]: ../ThreadBase/IsBusy.md
[11]: ../ThreadBase/IsRunning.md
[12]: ../ThreadBase/OnExit.md
[13]: ../ThreadBase/Success.md
[14]: CallInvokeAction.md
[15]: ../Thread/CallInvokeAction.md
[16]: ../Thread/CallInvokeOnComplete.md
[17]: Cancel.md
[18]: ../Thread/Cancel_1.md
[19]: Dispose.md
[20]: ../Thread/Dispose.md
[21]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[22]: ../ThreadBase/Finalize.md
[23]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[24]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[25]: ../Thread_1/InvokeAction.md
[26]: ../ThreadBase/InvokeOnComplete.md
[27]: Join.md
[28]: ../Thread/Join.md
[29]: Join_1.md
[30]: ../Thread/Join_1.md
[31]: http://msdn.microsoft.com/en-us/library/57ctke0a
[32]: Run.md
[33]: ../ThreadBase/ThreadProc.md
[34]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[35]: ../../W/AsExtensions/As__1.md
[36]: ../../W/AsExtensions/README.md
[37]: ../../W/AsExtensions/AsJson__1.md
[38]: ../../W/AsExtensions/AsXml__1.md
[39]: ../ThreadExtensions/CreateThread__1.md
[40]: ../ThreadExtensions/README.md
[41]: ../../W/PropertyHostMethods/InitializeProperties.md
[42]: ../../W/PropertyHostMethods/README.md
[43]: ../../W/PropertyHostMethods/IsDirty.md
[44]: ../../W/PropertyHostMethods/MarkAsClean.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected property]: ../../_icons/protproperty.gif "Protected property"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"