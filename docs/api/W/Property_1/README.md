Property&lt;TValue> Class
=========================
  A generic Property with no owner (self-owned)


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.PropertyChangedNotifier][2]  
    [W.PropertyBase][3]&lt;**Property**&lt;**TValue**>, **TValue**>  
      **W.Property<TValue>**  

  **Namespace:**  [W][4]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Property<TValue> : PropertyBase<Property<TValue>, TValue>

```

#### Type Parameters

##### *TValue*
The type of the property value

The **Property<TValue>** type exposes the following members.


Constructors
------------

                 | Name                                                                                                  | Description               
---------------- | ----------------------------------------------------------------------------------------------------- | ------------------------- 
![Public method] | [Property&lt;TValue>()][5]                                                                            | Constructs a new Property 
![Public method] | [Property&lt;TValue>(TValue)][6]                                                                      | Constructs a new Property 
![Public method] | [Property&lt;TValue>(PropertyBase&lt;Property&lt;TValue>, TValue>.OnValueChangedDelegate)][7]         | Constructs a new Property 
![Public method] | [Property&lt;TValue>(TValue, PropertyBase&lt;Property&lt;TValue>, TValue>.OnValueChangedDelegate)][8] | Constructs a new Property 


Properties
----------

                   | Name              | Description                                                                                                                                                                                                     
------------------ | ----------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public property] | [DefaultValue][9] | 
Allows the programmer to assign a default value which can be reset via the ResetToDefaultValue method. This value does not have to be the initial value.
 (Inherited from [PropertyBase&lt;TOwner, TValue>][3].) 
![Public property] | [IsDirty][10]     | True if Value has changed since initialization or since the last call to MarkAsClean (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                                     
![Public property] | [Owner][11]       | The property owner (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                                                                                                       
![Public property] | [Value][12]       | Get/Set the actual value of the Property (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                                                                                 


Methods
-------

                    | Name                             | Description                                                                                                                      
------------------- | -------------------------------- | -------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Dispose][13]                    | Disposes the object and releases resources (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                
![Public method]    | [Equals][14]                     | (Inherited from [Object][1].)                                                                                                    
![Protected method] | [ExecuteOnValueChanged][15]      | Calls the OnValueChanged callback (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                         
![Protected method] | [Finalize][16]                   | Disposes the PropertyBase (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                 
![Public method]    | [GetHashCode][17]                | (Inherited from [Object][1].)                                                                                                    
![Public method]    | [GetType][18]                    | (Inherited from [Object][1].)                                                                                                    
![Protected method] | [GetValue][19]                   | Gets the property value (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                   
![Public method]    | [LoadValue][20]                  | Loads Value without raising events or calling the OnValueChanged callback (Inherited from [PropertyBase&lt;TOwner, TValue>][3].) 
![Protected method] | [MemberwiseClone][21]            | (Inherited from [Object][1].)                                                                                                    
![Protected method] | [OnPropertyChanged][22]          | Raises the OnPropertyChanged event (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                        
![Protected method] | [RaiseOnPropertyChanged][23]     | 
Raises the PropertyChanged event
 (Inherited from [PropertyChangedNotifier][2].)                                              
![Protected method] | [RaisePropertyValueChanged][24]  | Raises the PropertyValueChanged event (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                     
![Protected method] | [RaisePropertyValueChanging][25] | Raises the ValueChanging event (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                            
![Public method]    | [ResetToDefaultValue][26]        | Resets the Value to the value provided by DefaultValue (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                    
![Protected method] | [SetValue][27]                   | Sets the property value (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                   
![Public method]    | [ToString][28]                   | (Inherited from [Object][1].)                                                                                                    
![Public method]    | [WaitForChanged][29]             | Allows the caller to suspend it's thread until Value changes (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)              


Events
------

                | Name                  | Description                                                                                                                                
--------------- | --------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ 
![Public event] | [PropertyChanged][30] | Raised when a property changes (Inherited from [PropertyChangedNotifier][2].)                                                              
![Public event] | [ValueChanged][31]    | Raised after Value has changed (Inherited from [PropertyBase&lt;TOwner, TValue>][3].)                                                      
![Public event] | [ValueChanging][32]   | Raised before Value has changed. To prevent Value from changing set cancel to true. (Inherited from [PropertyBase&lt;TOwner, TValue>][3].) 


Fields
------

                   | Name                 | Description                                                                                                                             
------------------ | -------------------- | --------------------------------------------------------------------------------------------------------------------------------------- 
![Protected field] | [OnValueChanged][33] | Callback type for use in the constructor (if one wants to avoid using the event) (Inherited from [PropertyBase&lt;TOwner, TValue>][3].) 


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][34]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][35].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][36]     | Serializes an object to a Json string (Defined by [AsExtensions][35].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][37]      | Serializes an object to an xml string (Defined by [AsExtensions][35].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][38]   | Starts a new thread (Defined by [ThreadExtensions][39].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][40] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][41].) 
![Public Extension Method]                | [IsDirty][42]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][41].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][43]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][41].)                                                                                                  


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
[7]: _ctor_3.md
[8]: _ctor_2.md
[9]: ../PropertyBase_2/DefaultValue.md
[10]: ../PropertyBase_2/IsDirty.md
[11]: ../PropertyBase_2/Owner.md
[12]: ../PropertyBase_2/Value.md
[13]: ../PropertyBase_2/Dispose.md
[14]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[15]: ../PropertyBase_2/ExecuteOnValueChanged.md
[16]: ../PropertyBase_2/Finalize.md
[17]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[18]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[19]: ../PropertyBase_2/GetValue.md
[20]: ../PropertyBase_2/LoadValue.md
[21]: http://msdn.microsoft.com/en-us/library/57ctke0a
[22]: ../PropertyBase_2/OnPropertyChanged.md
[23]: ../PropertyChangedNotifier/RaiseOnPropertyChanged.md
[24]: ../PropertyBase_2/RaisePropertyValueChanged.md
[25]: ../PropertyBase_2/RaisePropertyValueChanging.md
[26]: ../PropertyBase_2/ResetToDefaultValue.md
[27]: ../PropertyBase_2/SetValue.md
[28]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[29]: ../PropertyBase_2/WaitForChanged.md
[30]: ../PropertyChangedNotifier/PropertyChanged.md
[31]: ../PropertyBase_2/ValueChanged.md
[32]: ../PropertyBase_2/ValueChanging.md
[33]: ../PropertyBase_2/OnValueChanged.md
[34]: ../AsExtensions/As__1.md
[35]: ../AsExtensions/README.md
[36]: ../AsExtensions/AsJson__1.md
[37]: ../AsExtensions/AsXml__1.md
[38]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[39]: ../../W.Threading/ThreadExtensions/README.md
[40]: ../PropertyHostMethods/InitializeProperties.md
[41]: ../PropertyHostMethods/README.md
[42]: ../PropertyHostMethods/IsDirty.md
[43]: ../PropertyHostMethods/MarkAsClean.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Protected field]: ../../_icons/protfield.gif "Protected field"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"