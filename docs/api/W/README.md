W Namespace
===========
The root namespace for Tungsten. W is used because it is the symbol for Tungsten on the periodic table of elements.


Classes
-------

                | Class                                                                              | Description                                                                                                                                                                                                                                             
--------------- | ---------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public class] | [ActionQueue&lt;T>][1]                                                             | 
Allows the programmer to enqueue items for processing on a separate thread. The ActionQueue will process items sequentially when an item is added.
                                                                                                  
![Public class] | [ArrayMethods][2]                                                                  | Methods to peek and modify arrays                                                                                                                                                                                                                       
![Public class] | [CallResult][3]                                                                    | A non-generic return value for a function. CallResult encapsulates a success/failure and an exception.                                                                                                                                                  
![Public class] | [CallResult&lt;TResult>][4]                                                        | 
Generic class to be used as a return value. CallResult encapsulates a success/failure, an exception and a return value.
                                                                                                                             
![Public class] | [Disposable][5]                                                                    | Provides the Disposable pattern as a base class                                                                                                                                                                                                         
![Public class] | [Disposer][6]                                                                      | 
Aids in implementing a clean Dispose method. Supports re-entrancy but only calls the cleanup Action once.
                                                                                                                                           
![Public class] | [EventTemplate][7]                                                                 | Wraps the functionality of delegate, event and RaiseXXX into a single class                                                                                                                                                                             
![Public class] | [EventTemplate&lt;TEventArg>][8]                                                   | Wraps the functionality of delegate, event and RaiseXXX into a single class                                                                                                                                                                             
![Public class] | [EventTemplate&lt;TEventArg1, TEventArg2>][9]                                      | Wraps the functionality of delegate, event and RaiseXXX into a single class                                                                                                                                                                             
![Public class] | [EventTemplate&lt;TEventArg1, TEventArg2, TEventArg3>][10]                         | Wraps the functionality of delegate, event and RaiseXXX into a single class                                                                                                                                                                             
![Public class] | [EventTemplate&lt;TEventArg1, TEventArg2, TEventArg3, TEventArg4>][11]             | Wraps the functionality of delegate, event and RaiseXXX into a single class                                                                                                                                                                             
![Public class] | [EventTemplate&lt;TEventArg1, TEventArg2, TEventArg3, TEventArg4, TEventArg5>][12] | Wraps the functionality of delegate, event and RaiseXXX into a single class                                                                                                                                                                             
![Public class] | [Lockable&lt;TValue>][13]                                                          | 
Extends LockableSlim with ValueChangedDelegate notification
                                                                                                                                                                                         
![Public class] | [LockableSlim&lt;TValue>][14]                                                      | Uses ReaderWriterLock to provide thread-safe access to an underlying value                                                                                                                                                                              
![Public class] | [Property&lt;TValue>][15]                                                          | A Property with no owner (self-owned)                                                                                                                                                                                                                   
![Public class] | [Property&lt;TOwner, TValue>][16]                                                  | A generic Property with an owner                                                                                                                                                                                                                        
![Public class] | [PropertyBase&lt;TOwner, TValue>][17]                                              |                                                                                                                                                                                                                                                         
![Public class] | [PropertyChangedNotifier][18]                                                      | 
This is a base class for supporting INotifyPropertyChanged
                                                                                                                                                                                          
![Public class] | [PropertyHost][19]                                                                 | 
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class does not support INotifyPropertyChanged and is not intented to host owned properties (though nothing prevents you from doing so)
 
![Public class] | [PropertyHostNotifier][20]                                                         | 
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class inherits PropertyChangedNotifier for INotifyPropertyChanged support
                                                          
![Public class] | [PropertySlim&lt;TValue>][21]                                                      |                                                                                                                                                                                                                                                         
![Public class] | [Singleton&lt;TSingletonType>][22]                                                 | Thread-safe Singleton implementation                                                                                                                                                                                                                    


Interfaces
----------

                    | Interface                  | Description                                                                                                                                         
------------------- | -------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public interface] | [IOwnedProperty][23]       | 
Used by PropertyHostMethods.InitializeProperties to find properties on which to set the owner. This interface is not used by self-owned properties.
 
![Public interface] | [IProperty][24]            | The base interface which Property must support                                                                                                      
![Public interface] | [IProperty&lt;TValue>][25] | The base interface which Property must support                                                                                                      


Delegates
---------

                   | Delegate                                                                                   | Description                       
------------------ | ------------------------------------------------------------------------------------------ | --------------------------------- 
![Public delegate] | [EventTemplateDelegate][26]                                                                | The template delegate             
![Public delegate] | [EventTemplateDelegate&lt;TEventArg>][27]                                                  | The template delegate             
![Public delegate] | [EventTemplateDelegate&lt;TEventArg1, TEventArg2>][28]                                     | The template delegate             
![Public delegate] | [EventTemplateDelegate&lt;TEventArg1, TEventArg2, TEventArg3>][29]                         | The template delegate             
![Public delegate] | [EventTemplateDelegate&lt;TEventArg1, TEventArg2, TEventArg3, TEventArg4>][30]             | The template delegate             
![Public delegate] | [EventTemplateDelegate&lt;TEventArg1, TEventArg2, TEventArg3, TEventArg4, TEventArg5>][31] | The template delegate             
![Public delegate] | [ValueChangedDelegate&lt;TValue>][32]                                                      | Raised when the value has changed 

[1]: ActionQueue_1/README.md
[2]: ArrayMethods/README.md
[3]: CallResult/README.md
[4]: CallResult_1/README.md
[5]: Disposable/README.md
[6]: Disposer/README.md
[7]: EventTemplate/README.md
[8]: EventTemplate_1/README.md
[9]: EventTemplate_2/README.md
[10]: EventTemplate_3/README.md
[11]: EventTemplate_4/README.md
[12]: EventTemplate_5/README.md
[13]: Lockable_1/README.md
[14]: LockableSlim_1/README.md
[15]: Property_1/README.md
[16]: Property_2/README.md
[17]: PropertyBase_2/README.md
[18]: PropertyChangedNotifier/README.md
[19]: PropertyHost/README.md
[20]: PropertyHostNotifier/README.md
[21]: PropertySlim_1/README.md
[22]: Singleton_1/README.md
[23]: IOwnedProperty/README.md
[24]: IProperty/README.md
[25]: IProperty_1/README.md
[26]: EventTemplateDelegate/README.md
[27]: EventTemplateDelegate_1/README.md
[28]: EventTemplateDelegate_2/README.md
[29]: EventTemplateDelegate_3/README.md
[30]: EventTemplateDelegate_4/README.md
[31]: EventTemplateDelegate_5/README.md
[32]: ValueChangedDelegate_1/README.md
[Public class]: ../_icons/pubclass.gif "Public class"
[Public interface]: ../_icons/pubinterface.gif "Public interface"
[Public delegate]: ../_icons/pubdelegate.gif "Public delegate"