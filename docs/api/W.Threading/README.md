W.Threading Namespace
=====================
Functionality related to multi-threading


Classes
-------

                | Class                        | Description                                                                                                                                                                             
--------------- | ---------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public class] | [Gate][1]                    | **Obsolete.**A thread Gate is a background thread which is initially closed. When a Gate is opened, the Action runs until completion. The Gate can be opened (Run) any number of times. 
![Public class] | [Gate&lt;TParameterType>][2] | **Obsolete.**A thread Gate which supports passing in a typed parameter                                                                                                                  
![Public class] | [ParameterizedThread][3]     | A thread class which supports a variable number of arguments                                                                                                                            
![Public class] | [Thread][4]                  | A thread class                                                                                                                                                                          
![Public class] | [Thread&lt;TType>][5]        | A thread class which can pass a typed parameter into the thread method                                                                                                                  
![Public class] | [ThreadMethod][6]            | Adds multi-threading and additional functionality to an Action or ThreadMethodDelegate                                                                                                  
![Public class] | [ThreadSlim][7]              | A lighter thread class than W.Threading.Thread                                                                                                                                          


Delegates
---------

                   | Delegate                   | Description                                                                 
------------------ | -------------------------- | --------------------------------------------------------------------------- 
![Public delegate] | [AnyMethodDelegate][8]     | The delegate Type used by ThreadMethod                                      
![Public delegate] | [GenericThreadDelegate][9] | The delegate, with variable arguments, which is called on a separate thread 
![Public delegate] | [ThreadDelegate][10]       | Thread delegate used by ThreadSlim                                          


Enumerations
------------

                      | Enumeration          | Description                      
--------------------- | -------------------- | -------------------------------- 
![Public enumeration] | [CPUProfileEnum][11] | The preferred level of CPU usage 

[1]: Gate/README.md
[2]: Gate_1/README.md
[3]: ParameterizedThread/README.md
[4]: Thread/README.md
[5]: Thread_1/README.md
[6]: ThreadMethod/README.md
[7]: ThreadSlim/README.md
[8]: AnyMethodDelegate/README.md
[9]: GenericThreadDelegate/README.md
[10]: ThreadDelegate/README.md
[11]: CPUProfileEnum/README.md
[Public class]: ../_icons/pubclass.gif "Public class"
[Public delegate]: ../_icons/pubdelegate.gif "Public delegate"
[Public enumeration]: ../_icons/pubenumeration.gif "Public enumeration"