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


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][9]          | Use Generic syntax for the as operator. (Defined by [AsExtensions][10].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][11]     | Serializes an object to a Json string (Defined by [AsExtensions][10].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][12]      | Serializes an object to an xml string (Defined by [AsExtensions][10].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][13]   | Starts a new thread (Defined by [ThreadExtensions][14].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][15] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][16].) 
![Public Extension Method]                | [IsDirty][17]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][16].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][18]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][16].)                                                                                                  


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
[9]: ../AsExtensions/As__1.md
[10]: ../AsExtensions/README.md
[11]: ../AsExtensions/AsJson__1.md
[12]: ../AsExtensions/AsXml__1.md
[13]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[14]: ../../W.Threading/ThreadExtensions/README.md
[15]: ../PropertyHostMethods/InitializeProperties.md
[16]: ../PropertyHostMethods/README.md
[17]: ../PropertyHostMethods/IsDirty.md
[18]: ../PropertyHostMethods/MarkAsClean.md
[19]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"