W Namespace
===========
The root namespace for Tungsten. W is used because it is the symbol for Tungsten on the periodic table of elements.


Classes
-------

                | Class                                                                              | Description                                                                                                                                                                                                                                             
--------------- | ---------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public class] | [ActionQueue&lt;T>][1]                                                             | 
Allows the programmer to enqueue items for processing on a separate thread. The ActionQueue will process items sequentially whenever an item is added.
                                                                                              
![Public class] | [AtomicMethod][2]                                                                  | Wraps an AtomicMethodDelegate with functionality to prevent re-entrancy and to know when the method is running and completed                                                                                                                            
![Public class] | [CallResult][3]                                                                    | A non-generic return value for a function. CallResult encapsulates a success/failure and an exception.                                                                                                                                                  
![Public class] | [CallResult&lt;TResult>][4]                                                        | 
Generic class to be used as a return value. CallResult encapsulates a success/failure, an exception and a return value.
                                                                                                                             
![Public class] | [Disposable][5]                                                                    | Provides the Disposable pattern as a base class                                                                                                                                                                                                         
![Public class] | [EventTemplate][6]                                                                 | Wraps the functionality of delegate, event and RaiseXXX into a single class                                                                                                                                                                             
![Public class] | [EventTemplate&lt;TEventArg>][7]                                                   | Wraps the functionality of delegate, event and RaiseXXX into a single class                                                                                                                                                                             
![Public class] | [EventTemplate&lt;TEventArg1, TEventArg2>][8]                                      | Wraps the functionality of delegate, event and RaiseXXX into a single class                                                                                                                                                                             
![Public class] | [EventTemplate&lt;TEventArg1, TEventArg2, TEventArg3>][9]                          | Wraps the functionality of delegate, event and RaiseXXX into a single class                                                                                                                                                                             
![Public class] | [EventTemplate&lt;TEventArg1, TEventArg2, TEventArg3, TEventArg4>][10]             | Wraps the functionality of delegate, event and RaiseXXX into a single class                                                                                                                                                                             
![Public class] | [EventTemplate&lt;TEventArg1, TEventArg2, TEventArg3, TEventArg4, TEventArg5>][11] | Wraps the functionality of delegate, event and RaiseXXX into a single class                                                                                                                                                                             
![Public class] | [Lockable&lt;TValue>][12]                                                          | 
Provides thread safety via locking
                                                                                                                                                                                                                  
![Public class] | [LockableSlim&lt;TValue>][13]                                                      | 
Provides thread safety via ReaderWriterLockSlim locking. This is more efficient than Lockable&lt;TValue>.
                                                                                                                                           
![Public class] | [Property&lt;TValue>][14]                                                          | A generic Property with no owner (self-owned)                                                                                                                                                                                                           
![Public class] | [Property&lt;TOwner, TValue>][15]                                                  | A generic Property with an owner                                                                                                                                                                                                                        
![Public class] | [PropertyBase&lt;TOwner, TValue>][16]                                              | Provides the functionality for the Property classes                                                                                                                                                                                                     
![Public class] | [PropertyChangedNotifier][17]                                                      | 
This is a base class for supporting INotifyPropertyChanged
                                                                                                                                                                                          
![Public class] | [PropertyHost][18]                                                                 | 
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class does not support INotifyPropertyChanged and is not intented to host owned properties (though nothing prevents you from doing so)
 
![Public class] | [PropertyHostMethods][19]                                                          | Exposes static PropertyHost extension methods                                                                                                                                                                                                           
![Public class] | [PropertyHostNotifier][20]                                                         | 
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class inherits PropertyChangedNotifier for INotifyPropertyChanged support
                                                          
![Public class] | [Singleton&lt;TSingletonType>][21]                                                 | Thread-safe Singleton implementation                                                                                                                                                                                                                    


Interfaces
----------

                    | Interface                  | Description                                                                                                                                         
------------------- | -------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public interface] | [IOwnedProperty][22]       | 
Used by PropertyHostMethods.InitializeProperties to find properties on which to set the owner. This interface is not used by self-owned properties.
 
![Public interface] | [IProperty][23]            | The base interface which Property must support                                                                                                      
![Public interface] | [IProperty&lt;TValue>][24] | The base interface which Property must support                                                                                                      


Delegates
---------

                   | Delegate                                                                                   | Description                                                                                     
------------------ | ------------------------------------------------------------------------------------------ | ----------------------------------------------------------------------------------------------- 
![Public delegate] | [AtomicMethodDelegate][25]                                                                 | The delegate Type used by AtomicMethod                                                          
![Public delegate] | [EventTemplateDelegate][26]                                                                | The template delegate                                                                           
![Public delegate] | [EventTemplateDelegate&lt;TEventArg>][27]                                                  | The template delegate                                                                           
![Public delegate] | [EventTemplateDelegate&lt;TEventArg1, TEventArg2>][28]                                     | The template delegate                                                                           
![Public delegate] | [EventTemplateDelegate&lt;TEventArg1, TEventArg2, TEventArg3>][29]                         | The template delegate                                                                           
![Public delegate] | [EventTemplateDelegate&lt;TEventArg1, TEventArg2, TEventArg3, TEventArg4>][30]             | The template delegate                                                                           
![Public delegate] | [EventTemplateDelegate&lt;TEventArg1, TEventArg2, TEventArg3, TEventArg4, TEventArg5>][31] | The template delegate                                                                           
![Public delegate] | [OnValueChangedDelegate&lt;TOwner, TValue>][32]                                            | Used by the constructor to handle the property change via a callback rather than the events     
![Public delegate] | [PropertyValueChangedDelegate&lt;TOwner, TValue>][33]                                      | Raised when the value of the property changes                                                   
![Public delegate] | [PropertyValueChangingDelegate&lt;TOwner, TValue>][34]                                     | Raised prior to the value of the property changing. Allows the programmer to cancel the change. 

[1]: ActionQueue_1/README.md
[2]: AtomicMethod/README.md
[3]: CallResult/README.md
[4]: CallResult_1/README.md
[5]: Disposable/README.md
[6]: EventTemplate/README.md
[7]: EventTemplate_1/README.md
[8]: EventTemplate_2/README.md
[9]: EventTemplate_3/README.md
[10]: EventTemplate_4/README.md
[11]: EventTemplate_5/README.md
[12]: Lockable_1/README.md
[13]: LockableSlim_1/README.md
[14]: Property_1/README.md
[15]: Property_2/README.md
[16]: PropertyBase_2/README.md
[17]: PropertyChangedNotifier/README.md
[18]: PropertyHost/README.md
[19]: PropertyHostMethods/README.md
[20]: PropertyHostNotifier/README.md
[21]: Singleton_1/README.md
[22]: IOwnedProperty/README.md
[23]: IProperty/README.md
[24]: IProperty_1/README.md
[25]: AtomicMethodDelegate/README.md
[26]: EventTemplateDelegate/README.md
[27]: EventTemplateDelegate_1/README.md
[28]: EventTemplateDelegate_2/README.md
[29]: EventTemplateDelegate_3/README.md
[30]: EventTemplateDelegate_4/README.md
[31]: EventTemplateDelegate_5/README.md
[32]: OnValueChangedDelegate_2/README.md
[33]: PropertyValueChangedDelegate_2/README.md
[34]: PropertyValueChangingDelegate_2/README.md
[Public class]: ../_icons/pubclass.gif "Public class"
[Public interface]: ../_icons/pubinterface.gif "Public interface"
[Public delegate]: ../_icons/pubdelegate.gif "Public delegate"