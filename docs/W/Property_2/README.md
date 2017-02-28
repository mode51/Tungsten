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
![Public method] | [Property&lt;TOwner, TValue>(TOwner, TValue, PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate)][11] | Constructs a new Property 


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][12]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][13].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][14]     | Serializes an object to a Json string (Defined by [AsExtensions][13].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][15]      | Serializes an object to an xml string (Defined by [AsExtensions][13].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][16]   | Starts a new thread (Defined by [ThreadExtensions][17].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][18] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][19].) 
![Public Extension Method]                | [IsDirty][20]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][19].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][21]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][19].)                                                                                                  


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
[8]: _ctor_6.md
[9]: _ctor_2.md
[10]: _ctor_4.md
[11]: _ctor_3.md
[12]: ../AsExtensions/As__1.md
[13]: ../AsExtensions/README.md
[14]: ../AsExtensions/AsJson__1.md
[15]: ../AsExtensions/AsXml__1.md
[16]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[17]: ../../W.Threading/ThreadExtensions/README.md
[18]: ../PropertyHostMethods/InitializeProperties.md
[19]: ../PropertyHostMethods/README.md
[20]: ../PropertyHostMethods/IsDirty.md
[21]: ../PropertyHostMethods/MarkAsClean.md
[22]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"