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


Methods
-------

                    | Name                  | Description                                                                                                                                      
------------------- | --------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------ 
![Protected method] | [CallInvokeAction][6] | 
Used to wrap the call to InvokeAction with try/catch handlers. This method should call InvokeAction.
 (Overrides [Thread.CallInvokeAction()][7].) 
![Public method]    | [Run][8]              | Allows the Action to be called                                                                                                                   


Extension Methods
-----------------

                           | Name                    | Description                                              
-------------------------- | ----------------------- | -------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][9] | Starts a new thread (Defined by [ThreadExtensions][10].) 


See Also
--------

#### Reference
[W.Threading Namespace][4]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../ThreadBase/README.md
[3]: ../Thread/README.md
[4]: ../README.md
[5]: _ctor.md
[6]: CallInvokeAction.md
[7]: ../Thread/CallInvokeAction.md
[8]: Run.md
[9]: ../ThreadExtensions/CreateThread__1.md
[10]: ../ThreadExtensions/README.md
[11]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"