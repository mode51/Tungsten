Thread&lt;TCustomData> Class
============================
  A thread wrapper which makes multi-threading easier


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.ThreadBase][2]  
    [W.Threading.Thread][3]  
      **W.Threading.Thread<TCustomData>**  
        [W.Threading.Gate&lt;T>][4]  

  **Namespace:**  [W.Threading][5]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Thread<TCustomData> : Thread

```

#### Type Parameters

##### *TCustomData*
The type of custom data to pass to the thread Action

The **Thread<TCustomData>** type exposes the following members.


Constructors
------------

                 | Name                        | Description                    
---------------- | --------------------------- | ------------------------------ 
![Public method] | [Thread&lt;TCustomData>][6] | Constructs a new Thread object 


Properties
----------

                      | Name            | Description                                                                                                                         
--------------------- | --------------- | ----------------------------------------------------------------------------------------------------------------------------------- 
![Protected property] | [Action][7]     | The Action to be run on a new thread                                                                                                
![Protected property] | [Cts][8]        | The CancellationTokenSource which can be used to cancel the thread (Inherited from [ThreadBase][2].)                                
![Public property]    | [CustomData][9] | The custom data to pass into the Action                                                                                             
![Protected property] | [IsBusy][10]    | Value is True if the thread is currently running, otherwise False (Inherited from [ThreadBase][2].)                                 
![Public property]    | [IsRunning][11] | True if the thread is running, otherwise false (Inherited from [ThreadBase][2].)                                                    
![Protected property] | [OnExit][12]    | The Action to execute when the thread completes (Inherited from [ThreadBase][2].)                                                   
![Protected property] | [Success][13]   | The Value to send to the onExit Action. True if the thread returns successfully, otherwise False. (Inherited from [ThreadBase][2].) 


Methods
-------

                                 | Name                             | Description                                                                                                                     
-------------------------------- | -------------------------------- | ------------------------------------------------------------------------------------------------------------------------------- 
![Protected method]              | [CallInvokeAction][14]           | Called by the host thread procedure, this method calls the Action (Inherited from [Thread][3].)                                 
![Protected method]              | [CallInvokeOnComplete][15]       | Calls the onExit Action when the thread returns (Inherited from [Thread][3].)                                                   
![Public method]                 | [Cancel()][16]                   | (Inherited from [Thread][3].)                                                                                                   
![Public method]                 | [Cancel(Int32)][17]              | (Inherited from [Thread][3].)                                                                                                   
![Public method]![Static member] | [Create&lt;TCustomDataType>][18] | Starts a new thread                                                                                                             
![Public method]                 | [Dispose][19]                    | (Inherited from [Thread][3].)                                                                                                   
![Public method]                 | [Equals][20]                     | (Inherited from [Object][1].)                                                                                                   
![Protected method]              | [Finalize][21]                   | Destructs the ThreadBase object. Calls Dispose. (Inherited from [ThreadBase][2].)                                               
![Public method]                 | [GetHashCode][22]                | (Inherited from [Object][1].)                                                                                                   
![Public method]                 | [GetType][23]                    | (Inherited from [Object][1].)                                                                                                   
![Protected method]              | [InvokeAction][24]               | Invokes the Action (Overrides [ThreadBase.InvokeAction(CancellationTokenSource)][25].)                                          
![Protected method]              | [InvokeOnComplete][26]           | Invokes the onExit action. Virtual for customization. (Inherited from [ThreadBase][2].)                                         
![Public method]                 | [Join()][27]                     | Blocks the calling thread until the thread terminates (Inherited from [Thread][3].)                                             
![Public method]                 | [Join(Int32)][28]                | Blocks the calling thread until either the thread terminates or the specified milliseconds elapse (Inherited from [Thread][3].) 
![Protected method]              | [MemberwiseClone][29]            | (Inherited from [Object][1].)                                                                                                   
![Protected method]              | [ThreadProc][30]                 | The host thread procedure. This method calls the Action and subsequent onExit. (Inherited from [ThreadBase][2].)                
![Public method]                 | [ToString][31]                   | (Inherited from [Object][1].)                                                                                                   


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][32]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][33].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][34]     | Serializes an object to a Json string (Defined by [AsExtensions][33].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][35]      | Serializes an object to an xml string (Defined by [AsExtensions][33].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][36]   | Starts a new thread (Defined by [ThreadExtensions][37].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][38] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][39].) 
![Public Extension Method]                | [IsDirty][40]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][39].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][41]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][39].)                                                                                                  


See Also
--------

#### Reference
[W.Threading Namespace][5]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../ThreadBase/README.md
[3]: ../Thread/README.md
[4]: ../Gate_1/README.md
[5]: ../README.md
[6]: _ctor.md
[7]: Action.md
[8]: ../ThreadBase/Cts.md
[9]: CustomData.md
[10]: ../ThreadBase/IsBusy.md
[11]: ../ThreadBase/IsRunning.md
[12]: ../ThreadBase/OnExit.md
[13]: ../ThreadBase/Success.md
[14]: ../Thread/CallInvokeAction.md
[15]: ../Thread/CallInvokeOnComplete.md
[16]: ../Thread/Cancel.md
[17]: ../Thread/Cancel_1.md
[18]: Create__1.md
[19]: ../Thread/Dispose.md
[20]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[21]: ../ThreadBase/Finalize.md
[22]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[23]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[24]: InvokeAction.md
[25]: ../ThreadBase/InvokeAction.md
[26]: ../ThreadBase/InvokeOnComplete.md
[27]: ../Thread/Join.md
[28]: ../Thread/Join_1.md
[29]: http://msdn.microsoft.com/en-us/library/57ctke0a
[30]: ../ThreadBase/ThreadProc.md
[31]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[32]: ../../W/AsExtensions/As__1.md
[33]: ../../W/AsExtensions/README.md
[34]: ../../W/AsExtensions/AsJson__1.md
[35]: ../../W/AsExtensions/AsXml__1.md
[36]: ../ThreadExtensions/CreateThread__1.md
[37]: ../ThreadExtensions/README.md
[38]: ../../W/PropertyHostMethods/InitializeProperties.md
[39]: ../../W/PropertyHostMethods/README.md
[40]: ../../W/PropertyHostMethods/IsDirty.md
[41]: ../../W/PropertyHostMethods/MarkAsClean.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected property]: ../../_icons/protproperty.gif "Protected property"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Static member]: ../../_icons/static.gif "Static member"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"