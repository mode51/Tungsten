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
	IProperty<TValue>, IProperty
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
![Protected method] | [ExecuteOnValueChanged][11]      | Calls the OnValueChanged callback                                                                       
![Protected method] | [GetValue][12]                   | Gets the property value (Overrides [PropertyChangedNotifier.GetValue()][13].)                           
![Public method]    | [LoadValue][14]                  | Loads Value without raising events or calling the OnValueChanged callback                               
![Protected method] | [OnPropertyChanged][15]          | Raises the OnPropertyChanged event (Overrides [PropertyChangedNotifier.OnPropertyChanged(String)][16].) 
![Protected method] | [RaisePropertyValueChanged][17]  | Raises the PropertyValueChanged event                                                                   
![Protected method] | [RaisePropertyValueChanging][18] | Raises the ValueChanging event                                                                          
![Public method]    | [ResetToDefaultValue][19]        | Resets the Value to the value provided by DefaultValue                                                  
![Protected method] | [SetValue][20]                   | Sets the property value (Overrides [PropertyChangedNotifier.SetValue(Object, String)][21].)             
![Public method]    | [WaitForChanged][22]             | Allows the caller to suspend it's thread until Value changes                                            


Events
------

                | Name                | Description                                                                         
--------------- | ------------------- | ----------------------------------------------------------------------------------- 
![Public event] | [ValueChanged][23]  | Raised after Value has changed                                                      
![Public event] | [ValueChanging][24] | Raised before Value has changed. To prevent Value from changing set cancel to true. 


Fields
------

                   | Name                 | Description                                                                      
------------------ | -------------------- | -------------------------------------------------------------------------------- 
![Protected field] | [OnValueChanged][25] | Callback type for use in the constructor (if one wants to avoid using the event) 


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][26]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][27].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][28]     | Serializes an object to a Json string (Defined by [AsExtensions][27].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][29]   | Starts a new thread (Defined by [ThreadExtensions][30].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][31] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][32].) 
![Public Extension Method]                | [IsDirty][33]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][32].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][34]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][32].)                                                                                                  


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
[11]: ExecuteOnValueChanged.md
[12]: GetValue.md
[13]: ../PropertyChangedNotifier/GetValue.md
[14]: LoadValue.md
[15]: OnPropertyChanged.md
[16]: ../PropertyChangedNotifier/OnPropertyChanged.md
[17]: RaisePropertyValueChanged.md
[18]: RaisePropertyValueChanging.md
[19]: ResetToDefaultValue.md
[20]: SetValue.md
[21]: ../PropertyChangedNotifier/SetValue.md
[22]: WaitForChanged.md
[23]: ValueChanged.md
[24]: ValueChanging.md
[25]: OnValueChanged.md
[26]: ../AsExtensions/As__1.md
[27]: ../AsExtensions/README.md
[28]: ../AsExtensions/AsJson__1.md
[29]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[30]: ../../W.Threading/ThreadExtensions/README.md
[31]: ../PropertyHostMethods/InitializeProperties.md
[32]: ../PropertyHostMethods/README.md
[33]: ../PropertyHostMethods/IsDirty.md
[34]: ../PropertyHostMethods/MarkAsClean.md
[35]: ../../_icons/Help.png
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Protected field]: ../../_icons/protfield.gif "Protected field"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"