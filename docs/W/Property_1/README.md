Property&lt;TValue> Class
=========================
  
[Missing &lt;summary> documentation for "T:W.Property`1"]



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

[Missing &lt;typeparam name="TValue"/> documentation for "T:W.Property`1"]


The **Property<TValue>** type exposes the following members.


Constructors
------------

                 | Name                                                                                                  | Description                                                  
---------------- | ----------------------------------------------------------------------------------------------------- | ------------------------------------------------------------ 
![Public method] | [Property&lt;TValue>()][5]                                                                            | Initializes a new instance of the **Property<TValue>** class 
![Public method] | [Property&lt;TValue>(TValue)][6]                                                                      | Initializes a new instance of the **Property<TValue>** class 
![Public method] | [Property&lt;TValue>(PropertyBase&lt;Property&lt;TValue>, TValue>.OnValueChangedDelegate)][7]         | Initializes a new instance of the **Property<TValue>** class 
![Public method] | [Property&lt;TValue>(TValue, PropertyBase&lt;Property&lt;TValue>, TValue>.OnValueChangedDelegate)][8] | Initializes a new instance of the **Property<TValue>** class 


Extension Methods
-----------------

                           | Name                       | Description                                                                                                                                                                                                                      
-------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][9]    | Starts a new thread (Defined by [ThreadExtensions][10].)                                                                                                                                                                         
![Public Extension Method] | [InitializeProperties][11] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][12].) 
![Public Extension Method] | [IsDirty][13]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][12].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][14]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][12].)                                                                                                  


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
[9]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[10]: ../../W.Threading/ThreadExtensions/README.md
[11]: ../PropertyHostMethods/InitializeProperties.md
[12]: ../PropertyHostMethods/README.md
[13]: ../PropertyHostMethods/IsDirty.md
[14]: ../PropertyHostMethods/MarkAsClean.md
[15]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"