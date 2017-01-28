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
public abstract class ThreadBase
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
--------------------- | --------------- | ---------------------------------------------- 
![Protected property] | [Action][5]     |                                                
![Protected property] | [Cts][6]        |                                                
![Protected property] | [IsBusy][7]     |                                                
![Public property]    | [IsRunning][8]  | True if the thread is running, otherwise false 
![Protected property] | [OnComplete][9] |                                                
![Protected property] | [Success][10]   |                                                


Methods
-------

                    | Name                       | Description                                                                                                                                     
------------------- | -------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- 
![Protected method] | [CallInvokeAction][11]     |                                                                                                                                                 
![Protected method] | [CallInvokeOnComplete][12] |                                                                                                                                                 
![Public method]    | [Cancel][13]               | 
Cancels the thread by calling Cancel on the CancellationTokenSource. The value should be checked in the code in the specified Action parameter.
 
![Protected method] | [InvokeAction][14]         |                                                                                                                                                 
![Protected method] | [InvokeOnComplete][15]     |                                                                                                                                                 
![Public method]    | [Join()][16]               | Blocks the calling thread until the thread terminates                                                                                           
![Public method]    | [Join(Int32)][17]          | Blocks the calling thread until either the thread terminates or the specified milliseconds elapse                                               
![Protected method] | [ThreadProc][18]           |                                                                                                                                                 


Extension Methods
-----------------

                           | Name                     | Description                                              
-------------------------- | ------------------------ | -------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][19] | Starts a new thread (Defined by [ThreadExtensions][20].) 


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
[14]: InvokeAction.md
[15]: InvokeOnComplete.md
[16]: Join.md
[17]: Join_1.md
[18]: ThreadProc.md
[19]: ../ThreadExtensions/CreateThread__1.md
[20]: ../ThreadExtensions/README.md
[21]: ../../_icons/Help.png
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Protected property]: ../../_icons/protproperty.gif "Protected property"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"