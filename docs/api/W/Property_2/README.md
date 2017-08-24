Property&lt;TOwner, TValue> Class
=================================
  A generic Property with an owner


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.PropertyChangedNotifier][2]  
    [W.PropertyBase][3]&lt;**TOwner**, **TValue**>  
      **W.Property<TOwner, TValue>**  

  **Namespace:**  [W][4]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Property<TOwner, TValue> : PropertyBase<TOwner, TValue>, 
	IOwnedProperty
where TOwner : class

```

#### Type Parameters

##### *TOwner*
The type of owner

##### *TValue*
The type of the property value

The **Property<TOwner, TValue>** type exposes the following members.


Constructors
------------

                 | Name                                                                                                      | Description               
---------------- | --------------------------------------------------------------------------------------------------------- | ------------------------- 
![Public method] | [Property&lt;TOwner, TValue>()][5]                                                                        | Constructs a new Property 
![Public method] | [Property&lt;TOwner, TValue>(TOwner)][6]                                                                  | Constructs a new Property 
![Public method] | [Property&lt;TOwner, TValue>(TValue)][7]                                                                  | Constructs a new Property 
![Public method] | [Property&lt;TOwner, TValue>(PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate)][8]                  | Constructs a new Property 
![Public method] | [Property&lt;TOwner, TValue>(TOwner, TValue)][9]                                                          | Constructs a new Property 
![Public method] | [Property&lt;TOwner, TValue>(TOwner, PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate)][10]         | Constructs a new Property 
![Public method] | [Property&lt;TOwner, TValue>(TValue, PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate)][11]         | Constructs a new Property 
![Public method] | [Property&lt;TOwner, TValue>(TOwner, TValue, PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate)][12] | Constructs a new Property 


Properties
----------

                   | Name               | Description                                                                                                                                                                                                     
------------------ | ------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public property] | [DefaultValue][13] | 
Allows the programmer to assign a default value which can be reset via the ResetToDefaultValue method. This value does not have to be the initial value.
 (Inherited from [PropertyBase&lt;TOwner, TValue>][3].) 
![Public property] | [IsDirty][14]      | True if Value has changed since initialization or since the last call to MarkAsClean (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                                     
![Public property] | [Owner][15]        | The property owner (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                                                                                                       
![Public property] | [Value][16]        | Get/Set the actual value of the Property (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                                                                                 


Methods
-------

                    | Name                             | Description                                                                                                                      
------------------- | -------------------------------- | -------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Dispose][17]                    | Disposes the object and releases resources (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                
![Public method]    | [Equals][18]                     | (Inherited from [Object][1].)                                                                                                    
![Protected method] | [ExecuteOnValueChanged][19]      | Calls the OnValueChanged callback (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                         
![Protected method] | [Finalize][20]                   | Disposes the PropertyBase (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                 
![Public method]    | [GetHashCode][21]                | (Inherited from [Object][1].)                                                                                                    
![Public method]    | [GetType][22]                    | (Inherited from [Object][1].)                                                                                                    
![Protected method] | [GetValue][23]                   | Gets the property value (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                   
![Public method]    | [LoadValue][24]                  | Loads Value without raising events or calling the OnValueChanged callback (Inherited from [PropertyBase&lt;TOwner, TValue>][3].) 
![Protected method] | [MemberwiseClone][25]            | (Inherited from [Object][1].)                                                                                                    
![Protected method] | [OnPropertyChanged][26]          | Raises the OnPropertyChanged event (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                        
![Protected method] | [RaiseOnPropertyChanged][27]     | 
Raises the PropertyChanged event
 (Inherited from [PropertyChangedNotifier][2].)                                              
![Protected method] | [RaisePropertyValueChanged][28]  | Raises the PropertyValueChanged event (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                     
![Protected method] | [RaisePropertyValueChanging][29] | Raises the ValueChanging event (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                            
![Public method]    | [ResetToDefaultValue][30]        | Resets the Value to the value provided by DefaultValue (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                    
![Protected method] | [SetValue][31]                   | Sets the property value (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                   
![Public method]    | [ToString][32]                   | (Inherited from [Object][1].)                                                                                                    
![Public method]    | [WaitForChanged][33]             | Allows the caller to suspend it's thread until Value changes (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)              


Events
------

                | Name                  | Description                                                                                                                                
--------------- | --------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ 
![Public event] | [PropertyChanged][34] | Raised when a property changes (Inherited from [PropertyChangedNotifier][2].)                                                              
![Public event] | [ValueChanged][35]    | Raised after Value has changed (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                      
![Public event] | [ValueChanging][36]   | Raised before Value has changed. To prevent Value from changing set cancel to true. (Inherited from [PropertyBase&lt;TOwner, TValue>][3].) 


Fields
------

                   | Name                 | Description                                                                                                                             
------------------ | -------------------- | --------------------------------------------------------------------------------------------------------------------------------------- 
![Protected field] | [OnValueChanged][37] | Callback type for use in the constructor (if one wants to avoid using the event) (Inherited from [PropertyBase&lt;TOwner, TValue>][3].) 


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][38]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][39].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][40]     | Serializes an object to a Json string (Defined by [AsExtensions][39].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][41]      | Serializes an object to an xml string (Defined by [AsExtensions][39].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][42]   | Starts a new thread (Defined by [ThreadExtensions][43].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][44] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][45].) 
![Public Extension Method]                | [IsDirty][46]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][45].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][47]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][45].)                                                                                                  
![Public Extension Method]                | [WaitForValue][48]         | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][49].)                                                                                                         


See Also
--------

#### Reference
[W Namespace][4]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../PropertyChangedNotifier/README.md
[3]: ../PropertyBase_2/README.md
[4]: ../README.md
[5]: _ctor.md
[6]: _ctor_1.md
[7]: _ctor_5.md
[8]: _ctor_7.md
[9]: _ctor_2.md
[10]: _ctor_4.md
[11]: _ctor_6.md
[12]: _ctor_3.md
[13]: ../PropertyBase_2/DefaultValue.md
[14]: ../PropertyBase_2/IsDirty.md
[15]: ../PropertyBase_2/Owner.md
[16]: ../PropertyBase_2/Value.md
[17]: ../PropertyBase_2/Dispose.md
[18]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[19]: ../PropertyBase_2/ExecuteOnValueChanged.md
[20]: ../PropertyBase_2/Finalize.md
[21]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[22]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[23]: ../PropertyBase_2/GetValue.md
[24]: ../PropertyBase_2/LoadValue.md
[25]: http://msdn.microsoft.com/en-us/library/57ctke0a
[26]: ../PropertyBase_2/OnPropertyChanged.md
[27]: ../PropertyChangedNotifier/RaiseOnPropertyChanged.md
[28]: ../PropertyBase_2/RaisePropertyValueChanged.md
[29]: ../PropertyBase_2/RaisePropertyValueChanging.md
[30]: ../PropertyBase_2/ResetToDefaultValue.md
[31]: ../PropertyBase_2/SetValue.md
[32]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[33]: ../PropertyBase_2/WaitForChanged.md
[34]: ../PropertyChangedNotifier/PropertyChanged.md
[35]: ../PropertyBase_2/ValueChanged.md
[36]: ../PropertyBase_2/ValueChanging.md
[37]: ../PropertyBase_2/OnValueChanged.md
[38]: ../AsExtensions/As__1.md
[39]: ../AsExtensions/README.md
[40]: ../AsExtensions/AsJson__1.md
[41]: ../AsExtensions/AsXml__1.md
[42]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[43]: ../../W.Threading/ThreadExtensions/README.md
[44]: ../PropertyHostMethods/InitializeProperties.md
[45]: ../PropertyHostMethods/README.md
[46]: ../PropertyHostMethods/IsDirty.md
[47]: ../PropertyHostMethods/MarkAsClean.md
[48]: ../ExtensionMethods/WaitForValue.md
[49]: ../ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Protected field]: ../../_icons/protfield.gif "Protected field"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"