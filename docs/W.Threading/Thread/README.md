Thread Class
============
  A thread wrapper which makes multi-threading easier


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.ThreadBase][2]  
    **W.Threading.Thread**  
      [W.Threading.Gate][3]  
      [W.Threading.Thread&lt;T>][4]  

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
---------------- | ----------- | ------------------- 
![Public method] | [Thread][6] | Starts a new thread 


Methods
-------

                                 | Name                  | Description                                                                                                                                                                            
-------------------------------- | --------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Protected method]              | [CallInvokeAction][7] | Wraps the call to InvokeAction with try/catch block to catch exceptions (Overrides [ThreadBase.CallInvokeAction()][8].)                                                                
![Public method]                 | [Cancel()][9]         | 
Cancels the thread by calling Cancel on the CancellationTokenSource. The value should be checked in the code in the specified Action parameter.
 (Overrides [ThreadBase.Cancel()][10].) 
![Public method]                 | [Cancel(Int32)][11]   | 
Cancels the thread by calling Cancel on the CancellationTokenSource. The value should be checked in the code in the specified Action parameter.
                                    
![Public method]![Static member] | [Create][12]          | Starts a new thread                                                                                                                                                                    
![Public method]                 | [Dispose][13]         | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. (Overrides [ThreadBase.Dispose()][14].)                                       
![Public method]                 | [Join()][15]          | Blocks the calling thread until the thread terminates (Overrides [ThreadBase.Join()][16].)                                                                                             
![Public method]                 | [Join(Int32)][17]     | Blocks the calling thread until either the thread terminates or the specified milliseconds elapse (Overrides [ThreadBase.Join(Int32)][18].)                                            


Extension Methods
-----------------

                           | Name                       | Description                                                                                                                                                                                                                      
-------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][19]   | Starts a new thread (Defined by [ThreadExtensions][20].)                                                                                                                                                                         
![Public Extension Method] | [InitializeProperties][21] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][22].) 
![Public Extension Method] | [IsDirty][23]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][22].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][24]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][22].)                                                                                                  


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
[7]: CallInvokeAction.md
[8]: ../ThreadBase/CallInvokeAction.md
[9]: Cancel.md
[10]: ../ThreadBase/Cancel.md
[11]: Cancel_1.md
[12]: Create.md
[13]: Dispose.md
[14]: ../ThreadBase/Dispose.md
[15]: Join.md
[16]: ../ThreadBase/Join.md
[17]: Join_1.md
[18]: ../ThreadBase/Join_1.md
[19]: ../ThreadExtensions/CreateThread__1.md
[20]: ../ThreadExtensions/README.md
[21]: ../../W/PropertyHostMethods/InitializeProperties.md
[22]: ../../W/PropertyHostMethods/README.md
[23]: ../../W/PropertyHostMethods/IsDirty.md
[24]: ../../W/PropertyHostMethods/MarkAsClean.md
[25]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Static member]: ../../_icons/static.gif "Static member"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"