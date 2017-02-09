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

                      | Name            | Description                                                                                           
--------------------- | --------------- | ----------------------------------------------------------------------------------------------------- 
![Protected property] | [Action][5]     | The Action to execute on the thread                                                                   
![Protected property] | [Cts][6]        | The CancellationTokenSource which can be used to cancel the thread                                    
![Protected property] | [IsBusy][7]     | Value is True if the thread is currently running, otherwise False                                     
![Public property]    | [IsRunning][8]  | True if the thread is running, otherwise false                                                        
![Protected property] | [OnComplete][9] | The Action to execute when the thread completes                                                       
![Protected property] | [Success][10]   | The Value to send to the OnComplete Action. True if the thread returns successfully, otherwise False. 


Methods
-------

                    | Name                       | Description                                                                                                                                     
------------------- | -------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- 
![Protected method] | [CallInvokeAction][11]     | Must be overridden to provide exception handling                                                                                                
![Protected method] | [CallInvokeOnComplete][12] | Calls the OnComplete Action when the thread returns                                                                                             
![Public method]    | [Cancel][13]               | 
Cancels the thread by calling Cancel on the CancellationTokenSource. The value should be checked in the code in the specified Action parameter.
 
![Public method]    | [Dispose][14]              | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.                                        
![Protected method] | [Finalize][15]             | Destructs the ThreadBase object. Calls Dispose. (Overrides [Object.Finalize()][16].)                                                            
![Protected method] | [InvokeAction][17]         | Invokes the Action. Virtual for customization.                                                                                                  
![Protected method] | [InvokeOnComplete][18]     | Invokes the OnComplete action. Virtual for customization.                                                                                       
![Public method]    | [Join()][19]               | Blocks the calling thread until the thread terminates                                                                                           
![Public method]    | [Join(Int32)][20]          | Blocks the calling thread until either the thread terminates or the specified milliseconds elapse                                               
![Protected method] | [ThreadProc][21]           | The host thread procedure. This method calls the Action and subsequent OnComplete.                                                              


Extension Methods
-----------------

                           | Name                       | Description                                                                                                                                                                                                                      
-------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][22]   | Starts a new thread (Defined by [ThreadExtensions][23].)                                                                                                                                                                         
![Public Extension Method] | [InitializeProperties][24] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][25].) 
![Public Extension Method] | [IsDirty][26]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][25].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][27]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][25].)                                                                                                  


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
[9]: OnComplete.md
[10]: Success.md
[11]: CallInvokeAction.md
[12]: CallInvokeOnComplete.md
[13]: Cancel.md
[14]: Dispose.md
[15]: Finalize.md
[16]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[17]: InvokeAction.md
[18]: InvokeOnComplete.md
[19]: Join.md
[20]: Join_1.md
[21]: ThreadProc.md
[22]: ../ThreadExtensions/CreateThread__1.md
[23]: ../ThreadExtensions/README.md
[24]: ../../W/PropertyHostMethods/InitializeProperties.md
[25]: ../../W/PropertyHostMethods/README.md
[26]: ../../W/PropertyHostMethods/IsDirty.md
[27]: ../../W/PropertyHostMethods/MarkAsClean.md
[28]: ../../_icons/Help.png
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Protected property]: ../../_icons/protproperty.gif "Protected property"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"