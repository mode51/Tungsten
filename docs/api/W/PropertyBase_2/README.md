PropertyBase&lt;TOwner, TValue> Class
=====================================
   Provides the functionality for the Property classes


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.PropertyChangedNotifier][2]  
    **W.PropertyBase<TOwner, TValue>**  
      [W.Property&lt;TValue>][3]  
      [W.Property&lt;TOwner, TValue>][4]  

  **Namespace:**  [W][5]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public abstract class PropertyBase<TOwner, TValue> : PropertyChangedNotifier, 
	IDisposable, IProperty<TValue>, IProperty
where TOwner : class

```

#### Type Parameters

##### *TOwner*
The type of the property owner

##### *TValue*
The type of the property value

The **PropertyBase<TOwner, TValue>** type exposes the following members.


Constructors
------------

                    | Name                                 | Description                                                              
------------------- | ------------------------------------ | ------------------------------------------------------------------------ 
![Protected method] | [PropertyBase&lt;TOwner, TValue>][6] | Initializes a new instance of the **PropertyBase<TOwner, TValue>** class 


Properties
----------

                   | Name              | Description                                                                                                                                              
------------------ | ----------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public property] | [DefaultValue][7] | 
Allows the programmer to assign a default value which can be reset via the ResetToDefaultValue method. This value does not have to be the initial value.
 
![Public property] | [IsDirty][8]      | True if Value has changed since initialization or since the last call to MarkAsClean                                                                     
![Public property] | [Owner][9]        | The property owner                                                                                                                                       
![Public property] | [Value][10]       | Get/Set the actual value of the Property                                                                                                                 


Methods
-------

                    | Name                             | Description                                                                                             
------------------- | -------------------------------- | ------------------------------------------------------------------------------------------------------- 
![Public method]    | [Dispose][11]                    | Disposes the object and releases resources                                                              
![Public method]    | [Equals][12]                     | (Inherited from [Object][1].)                                                                           
![Protected method] | [ExecuteOnValueChanged][13]      | Calls the OnValueChanged callback                                                                       
![Protected method] | [Finalize][14]                   | Disposes the PropertyBase (Overrides [Object.Finalize()][15].)                                          
![Public method]    | [GetHashCode][16]                | (Inherited from [Object][1].)                                                                           
![Public method]    | [GetType][17]                    | (Inherited from [Object][1].)                                                                           
![Protected method] | [GetValue][18]                   | Gets the property value (Overrides [PropertyChangedNotifier.GetValue()][19].)                           
![Public method]    | [LoadValue][20]                  | Loads Value without raising events or calling the OnValueChanged callback                               
![Protected method] | [MemberwiseClone][21]            | (Inherited from [Object][1].)                                                                           
![Protected method] | [OnPropertyChanged][22]          | Raises the OnPropertyChanged event (Overrides [PropertyChangedNotifier.OnPropertyChanged(String)][23].) 
![Protected method] | [RaiseOnPropertyChanged][24]     | 
Raises the PropertyChanged event
 (Inherited from [PropertyChangedNotifier][2].)                     
![Protected method] | [RaisePropertyValueChanged][25]  | Raises the PropertyValueChanged event                                                                   
![Protected method] | [RaisePropertyValueChanging][26] | Raises the ValueChanging event                                                                          
![Public method]    | [ResetToDefaultValue][27]        | Resets the Value to the value provided by DefaultValue                                                  
![Protected method] | [SetValue][28]                   | Sets the property value (Overrides [PropertyChangedNotifier.SetValue(Object, String)][29].)             
![Public method]    | [ToString][30]                   | (Inherited from [Object][1].)                                                                           
![Public method]    | [WaitForChanged][31]             | Allows the caller to suspend it's thread until Value changes                                            


Events
------

                | Name                  | Description                                                                         
--------------- | --------------------- | ----------------------------------------------------------------------------------- 
![Public event] | [PropertyChanged][32] | Raised when a property changes (Inherited from [PropertyChangedNotifier][2].)       
![Public event] | [ValueChanged][33]    | Raised after Value has changed                                                      
![Public event] | [ValueChanging][34]   | Raised before Value has changed. To prevent Value from changing set cancel to true. 


Fields
------

                   | Name                 | Description                                                                      
------------------ | -------------------- | -------------------------------------------------------------------------------- 
![Protected field] | [OnValueChanged][35] | Callback type for use in the constructor (if one wants to avoid using the event) 


See Also
--------

#### Reference
[W Namespace][5]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../PropertyChangedNotifier/README.md
[3]: ../Property_1/README.md
[4]: ../Property_2/README.md
[5]: ../README.md
[6]: _ctor.md
[7]: DefaultValue.md
[8]: IsDirty.md
[9]: Owner.md
[10]: Value.md
[11]: Dispose.md
[12]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[13]: ExecuteOnValueChanged.md
[14]: Finalize.md
[15]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[16]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[17]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[18]: GetValue.md
[19]: ../PropertyChangedNotifier/GetValue.md
[20]: LoadValue.md
[21]: http://msdn.microsoft.com/en-us/library/57ctke0a
[22]: OnPropertyChanged.md
[23]: ../PropertyChangedNotifier/OnPropertyChanged.md
[24]: ../PropertyChangedNotifier/RaiseOnPropertyChanged.md
[25]: RaisePropertyValueChanged.md
[26]: RaisePropertyValueChanging.md
[27]: ResetToDefaultValue.md
[28]: SetValue.md
[29]: ../PropertyChangedNotifier/SetValue.md
[30]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[31]: WaitForChanged.md
[32]: ../PropertyChangedNotifier/PropertyChanged.md
[33]: ValueChanged.md
[34]: ValueChanging.md
[35]: OnValueChanged.md
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Protected field]: ../../_icons/protfield.gif "Protected field"