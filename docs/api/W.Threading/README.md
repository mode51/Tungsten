W.Threading Namespace
=====================
Functionality related to multi-threading


Classes
-------

                | Class                          | Description                                                                                                                                                                
--------------- | ------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public class] | [Gate][1]                      | A thread Gate is a background thread which is initially closed. When a Gate is opened, the Action runs until completion. The Gate can be opened (Run) any number of times. 
![Public class] | [Gate&lt;TParameterType>][2]   | A thread Gate which supports passing in a typed parameter                                                                                                                  
![Public class] | [GateMethods][3]               | Extension methods on Action to Create a Gate                                                                                                                               
![Public class] | [Thread][4]                    | A thread class                                                                                                                                                             
![Public class] | [Thread&lt;TParameterType>][5] | A thread class which can pass a typed parameter into the thread action                                                                                                     
![Public class] | [ThreadExtensions][6]          | Contains a generic extension method to quickly start a new thread                                                                                                          


Enumerations
------------

                      | Enumeration         | Description                      
--------------------- | ------------------- | -------------------------------- 
![Public enumeration] | [CPUProfileEnum][7] | The preferred level of CPU usage 

[1]: Gate/README.md
[2]: Gate_1/README.md
[3]: GateMethods/README.md
[4]: Thread/README.md
[5]: Thread_1/README.md
[6]: ThreadExtensions/README.md
[7]: CPUProfileEnum/README.md
[Public class]: ../_icons/pubclass.gif "Public class"
[Public enumeration]: ../_icons/pubenumeration.gif "Public enumeration"