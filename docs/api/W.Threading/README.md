W.Threading Namespace
=====================
Functionality related to multi-threading


Classes
-------

                | Class                          | Description                                                                                                                                                                
--------------- | ------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public class] | [Gate][1]                      | A thread Gate is a background thread which is initially closed. When a Gate is opened, the Action runs until completion. The Gate can be opened (Run) any number of times. 
![Public class] | [Gate&lt;TParameterType>][2]   | A thread Gate which supports passing in a typed parameter                                                                                                                  
![Public class] | [GateExtensions][3]            | Extension methods on Action to Create a Gate                                                                                                                               
![Public class] | [ParameterizedThread][4]       | A thread class which supports a variable number of arguments                                                                                                               
![Public class] | [Thread][5]                    | A thread class                                                                                                                                                             
![Public class] | [Thread&lt;TParameterType>][6] | A thread class which can pass a typed parameter into the thread action                                                                                                     


Delegates
---------

                   | Delegate                   | Description                                                                 
------------------ | -------------------------- | --------------------------------------------------------------------------- 
![Public delegate] | [GenericThreadDelegate][7] | The delegate, with variable arguments, which is called on a separate thread 


Enumerations
------------

                      | Enumeration         | Description                      
--------------------- | ------------------- | -------------------------------- 
![Public enumeration] | [CPUProfileEnum][8] | The preferred level of CPU usage 

[1]: Gate/README.md
[2]: Gate_1/README.md
[3]: GateExtensions/README.md
[4]: ParameterizedThread/README.md
[5]: Thread/README.md
[6]: Thread_1/README.md
[7]: GenericThreadDelegate/README.md
[8]: CPUProfileEnum/README.md
[Public class]: ../_icons/pubclass.gif "Public class"
[Public delegate]: ../_icons/pubdelegate.gif "Public delegate"
[Public enumeration]: ../_icons/pubenumeration.gif "Public enumeration"