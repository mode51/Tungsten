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
![Protected method]              | [CallInvokeAction][7] | (Overrides [ThreadBase.CallInvokeAction()][8].)                                                                                                                                        
![Public method]                 | [Cancel()][9]         | 
Cancels the thread by calling Cancel on the CancellationTokenSource. The value should be checked in the code in the specified Action parameter.
 (Overrides [ThreadBase.Cancel()][10].) 
![Public method]                 | [Cancel(Int32)][11]   | 
Cancels the thread by calling Cancel on the CancellationTokenSource. The value should be checked in the code in the specified Action parameter.
                                    
![Public method]![Static member] | [Create][12]          | Starts a new thread                                                                                                                                                                    
![Public method]                 | [Join()][13]          | Blocks the calling thread until the thread terminates (Overrides [ThreadBase.Join()][14].)                                                                                             
![Public method]                 | [Join(Int32)][15]     | Blocks the calling thread until either the thread terminates or the specified milliseconds elapse (Overrides [ThreadBase.Join(Int32)][16].)                                            


Extension Methods
-----------------

                           | Name                     | Description                                              
-------------------------- | ------------------------ | -------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][17] | Starts a new thread (Defined by [ThreadExtensions][18].) 


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
[13]: Join.md
[14]: ../ThreadBase/Join.md
[15]: Join_1.md
[16]: ../ThreadBase/Join_1.md
[17]: ../ThreadExtensions/CreateThread__1.md
[18]: ../ThreadExtensions/README.md
[19]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Static member]: ../../_icons/static.gif "Static member"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"