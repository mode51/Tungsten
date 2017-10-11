W Namespace
===========
The root namespace for Tungsten. W is used because it is the symbol for Tungsten on the periodic table of elements.


Classes
-------

                | Class                                                                              | Description                                                                                                                                                                                                                                             
--------------- | ---------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public class] | [ActionQueue&lt;T>][1]                                                             | 
Allows the programmer to enqueue items for processing on a separate thread. The ActionQueue will process items sequentially whenever an item is added.
                                                                                              
![Public class] | [AsExtensions][2]                                                                  | Extensions which convert objects of one type to another                                                                                                                                                                                                 
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
![Public class] | [ExtensionMethods][12]                                                             | Tungsten extension methods                                                                                                                                                                                                                              
![Public class] | [FromExtensions][13]                                                               | Extensions which convert objects of one type to another                                                                                                                                                                                                 
![Public class] | [InvokeExtensions][14]                                                             | Extension methods to provide code shortcuts to evaluate InvokeRequired and run code appropriately                                                                                                                                                       
![Public class] | [Lockable&lt;TValue>][15]                                                          | 
Provides thread safety via locking
                                                                                                                                                                                                                  
![Public class] | [Property&lt;TValue>][16]                                                          | A generic Property with no owner (self-owned)                                                                                                                                                                                                           
![Public class] | [Property&lt;TOwner, TValue>][17]                                                  | A generic Property with an owner                                                                                                                                                                                                                        
![Public class] | [PropertyBase&lt;TOwner, TValue>][18]                                              | Provides the functionality for the Property classes                                                                                                                                                                                                     
![Public class] | [PropertyChangedNotifier][19]                                                      | 
This is a base class for supporting INotifyPropertyChanged
                                                                                                                                                                                          
![Public class] | [PropertyHost][20]                                                                 | 
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class does not support INotifyPropertyChanged and is not intented to host owned properties (though nothing prevents you from doing so)
 
![Public class] | [PropertyHostMethods][21]                                                          | Exposes static PropertyHost extension methods                                                                                                                                                                                                           
![Public class] | [PropertyHostNotifier][22]                                                         | 
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class inherits PropertyChangedNotifier for INotifyPropertyChanged support
                                                          
![Public class] | [Singleton&lt;TSingletonType>][23]                                                 | Thread-safe Singleton implementation                                                                                                                                                                                                                    
![Public class] | [StringExtensions][24]                                                             | Some useful string extensions                                                                                                                                                                                                                           


Interfaces
----------

                    | Interface                  | Description                                                                                                                                         
------------------- | -------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public interface] | [IOwnedProperty][25]       | 
Used by PropertyHostMethods.InitializeProperties to find properties on which to set the owner. This interface is not used by self-owned properties.
 
![Public interface] | [IProperty][26]            | The base interface which Property must support                                                                                                      
![Public interface] | [IProperty&lt;TValue>][27] | The base interface which Property must support                                                                                                      


Delegates
---------

                   | Delegate                                                                                         | Description                                                                                     
------------------ | ------------------------------------------------------------------------------------------------ | ----------------------------------------------------------------------------------------------- 
![Public delegate] | [EventTemplate.EventDelegate][28]                                                                | The template delegate                                                                           
![Public delegate] | [EventTemplate&lt;TEventArg>.EventDelegate][29]                                                  | The template delegate                                                                           
![Public delegate] | [EventTemplate&lt;TEventArg1, TEventArg2>.EventDelegate][30]                                     | The template delegate                                                                           
![Public delegate] | [EventTemplate&lt;TEventArg1, TEventArg2, TEventArg3>.EventDelegate][31]                         | The template delegate                                                                           
![Public delegate] | [EventTemplate&lt;TEventArg1, TEventArg2, TEventArg3, TEventArg4>.EventDelegate][32]             | The template delegate                                                                           
![Public delegate] | [EventTemplate&lt;TEventArg1, TEventArg2, TEventArg3, TEventArg4, TEventArg5>.EventDelegate][33] | The template delegate                                                                           
![Public delegate] | [PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate][34]                                     | Used by the constructor to handle the property change via a callback rather than the events     
![Public delegate] | [PropertyBase&lt;TOwner, TValue>.PropertyValueChangedDelegate][35]                               | Raised when the value of the property changes                                                   
![Public delegate] | [PropertyBase&lt;TOwner, TValue>.PropertyValueChangingDelegate][36]                              | Raised prior to the value of the property changing. Allows the programmer to cancel the change. 

[1]: ActionQueue_1/README.md
[2]: AsExtensions/README.md
[3]: CallResult/README.md
[4]: CallResult_1/README.md
[5]: Disposable/README.md
[6]: EventTemplate/README.md
[7]: EventTemplate_1/README.md
[8]: EventTemplate_2/README.md
[9]: EventTemplate_3/README.md
[10]: EventTemplate_4/README.md
[11]: EventTemplate_5/README.md
[12]: ExtensionMethods/README.md
[13]: FromExtensions/README.md
[14]: InvokeExtensions/README.md
[15]: Lockable_1/README.md
[16]: Property_1/README.md
[17]: Property_2/README.md
[18]: PropertyBase_2/README.md
[19]: PropertyChangedNotifier/README.md
[20]: PropertyHost/README.md
[21]: PropertyHostMethods/README.md
[22]: PropertyHostNotifier/README.md
[23]: Singleton_1/README.md
[24]: StringExtensions/README.md
[25]: IOwnedProperty/README.md
[26]: IProperty/README.md
[27]: IProperty_1/README.md
[28]: EventTemplate_EventDelegate/README.md
[29]: EventTemplate_1_EventDelegate/README.md
[30]: EventTemplate_2_EventDelegate/README.md
[31]: EventTemplate_3_EventDelegate/README.md
[32]: EventTemplate_4_EventDelegate/README.md
[33]: EventTemplate_5_EventDelegate/README.md
[34]: PropertyBase_2_OnValueChangedDelegate/README.md
[35]: PropertyBase_2_PropertyValueChangedDelegate/README.md
[36]: PropertyBase_2_PropertyValueChangingDelegate/README.md
[Public class]: ../_icons/pubclass.gif "Public class"
[Public interface]: ../_icons/pubinterface.gif "Public interface"
[Public delegate]: ../_icons/pubdelegate.gif "Public delegate"