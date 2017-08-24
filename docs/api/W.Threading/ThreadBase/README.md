ThreadBase Class
================
  A base class for Thread which should work for all compiler Target types


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.ThreadBase**  
    [W.Threading.Thread][2]  

  **Namespace:**  [W.Threading][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public abstract class ThreadBase : IDisposable
```

The **ThreadBase** type exposes the following members.


Constructors
------------

                    | Name            | Description         
------------------- | --------------- | ------------------- 
![Protected method] | [ThreadBase][4] | Starts a new thread 


Properties
----------

                      | Name           | Description                                                                                       
--------------------- | -------------- | ------------------------------------------------------------------------------------------------- 
![Protected property] | [Action][5]    | The Action to execute on the thread                                                               
![Protected property] | [Cts][6]       | The CancellationTokenSource which can be used to cancel the thread                                
![Protected property] | [IsBusy][7]    | Value is True if the thread is currently running, otherwise False                                 
![Public property]    | [IsRunning][8] | True if the thread is running, otherwise false                                                    
![Protected property] | [OnExit][9]    | The Action to execute when the thread completes                                                   
![Protected property] | [Success][10]  | The Value to send to the onExit Action. True if the thread returns successfully, otherwise False. 


Methods
-------

                    | Name                       | Description                                                                                                                                     
------------------- | -------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- 
![Protected method] | [CallInvokeAction][11]     | Must be overridden to provide exception handling                                                                                                
![Protected method] | [CallInvokeOnComplete][12] | Calls the onExit Action when the thread returns                                                                                                 
![Public method]    | [Cancel()][13]             | 
Cancels the thread by calling Cancel on the CancellationTokenSource. The value should be checked in the code in the specified Action parameter.
 
![Public method]    | [Cancel(Int32)][14]        | 
Cancels the thread by calling Cancel on the CancellationTokenSource. The value should be checked in the code in the specified Action parameter.
 
![Public method]    | [Dispose][15]              | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.                                        
![Public method]    | [Equals][16]               | (Inherited from [Object][1].)                                                                                                                   
![Protected method] | [Finalize][17]             | Destructs the ThreadBase object. Calls Dispose. (Overrides [Object.Finalize()][18].)                                                            
![Public method]    | [GetHashCode][19]          | (Inherited from [Object][1].)                                                                                                                   
![Public method]    | [GetType][20]              | (Inherited from [Object][1].)                                                                                                                   
![Protected method] | [InvokeAction][21]         | Invokes the Action. Virtual for customization.                                                                                                  
![Protected method] | [InvokeOnComplete][22]     | Invokes the onExit action. Virtual for customization.                                                                                           
![Public method]    | [Join()][23]               | Blocks the calling thread until the thread terminates                                                                                           
![Public method]    | [Join(Int32)][24]          | Blocks the calling thread until either the thread terminates or the specified milliseconds elapse                                               
![Protected method] | [MemberwiseClone][25]      | (Inherited from [Object][1].)                                                                                                                   
![Protected method] | [ThreadProc][26]           | The host thread procedure. This method calls the Action and subsequent onExit.                                                                  
![Public method]    | [ToString][27]             | (Inherited from [Object][1].)                                                                                                                   


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][28]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][29].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][30]     | Serializes an object to a Json string (Defined by [AsExtensions][29].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][31]      | Serializes an object to an xml string (Defined by [AsExtensions][29].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][32]   | Starts a new thread (Defined by [ThreadExtensions][33].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][34] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][35].) 
![Public Extension Method]                | [IsDirty][36]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][35].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][37]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][35].)                                                                                                  
![Public Extension Method]                | [WaitForValue][38]         | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][39].)                                                                                                         


See Also
--------

#### Reference
[W.Threading Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../Thread/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: Action.md
[6]: Cts.md
[7]: IsBusy.md
[8]: IsRunning.md
[9]: OnExit.md
[10]: Success.md
[11]: CallInvokeAction.md
[12]: CallInvokeOnComplete.md
[13]: Cancel.md
[14]: Cancel_1.md
[15]: Dispose.md
[16]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[17]: Finalize.md
[18]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[19]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[20]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[21]: InvokeAction.md
[22]: InvokeOnComplete.md
[23]: Join.md
[24]: Join_1.md
[25]: http://msdn.microsoft.com/en-us/library/57ctke0a
[26]: ThreadProc.md
[27]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[28]: ../../W/AsExtensions/As__1.md
[29]: ../../W/AsExtensions/README.md
[30]: ../../W/AsExtensions/AsJson__1.md
[31]: ../../W/AsExtensions/AsXml__1.md
[32]: ../ThreadExtensions/CreateThread__1.md
[33]: ../ThreadExtensions/README.md
[34]: ../../W/PropertyHostMethods/InitializeProperties.md
[35]: ../../W/PropertyHostMethods/README.md
[36]: ../../W/PropertyHostMethods/IsDirty.md
[37]: ../../W/PropertyHostMethods/MarkAsClean.md
[38]: ../../W/ExtensionMethods/WaitForValue.md
[39]: ../../W/ExtensionMethods/README.md
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Protected property]: ../../_icons/protproperty.gif "Protected property"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"