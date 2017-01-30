Property&lt;TOwner, TValue> Class
=================================
  
[Missing &lt;summary> documentation for "T:W.Property`2"]



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

[Missing &lt;typeparam name="TOwner"/> documentation for "T:W.Property`2"]


##### *TValue*

[Missing &lt;typeparam name="TValue"/> documentation for "T:W.Property`2"]


The **Property<TOwner, TValue>** type exposes the following members.


Constructors
------------

                 | Name                                                                                                      | Description                                                          
---------------- | --------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------- 
![Public method] | [Property&lt;TOwner, TValue>()][5]                                                                        | Initializes a new instance of the **Property<TOwner, TValue>** class 
![Public method] | [Property&lt;TOwner, TValue>(TOwner)][6]                                                                  | Initializes a new instance of the **Property<TOwner, TValue>** class 
![Public method] | [Property&lt;TOwner, TValue>(TValue)][7]                                                                  | Initializes a new instance of the **Property<TOwner, TValue>** class 
![Public method] | [Property&lt;TOwner, TValue>(PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate)][8]                  | Initializes a new instance of the **Property<TOwner, TValue>** class 
![Public method] | [Property&lt;TOwner, TValue>(TOwner, TValue)][9]                                                          | Initializes a new instance of the **Property<TOwner, TValue>** class 
![Public method] | [Property&lt;TOwner, TValue>(TOwner, PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate)][10]         | Initializes a new instance of the **Property<TOwner, TValue>** class 
![Public method] | [Property&lt;TOwner, TValue>(TOwner, TValue, PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate)][11] | Initializes a new instance of the **Property<TOwner, TValue>** class 


Extension Methods
-----------------

                           | Name                       | Description                                                                                                                                                                                                                      
-------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][12]   | Starts a new thread (Defined by [ThreadExtensions][13].)                                                                                                                                                                         
![Public Extension Method] | [InitializeProperties][14] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][15].) 
![Public Extension Method] | [IsDirty][16]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][15].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][17]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][15].)                                                                                                  


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
[12]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[13]: ../../W.Threading/ThreadExtensions/README.md
[14]: ../PropertyHostMethods/InitializeProperties.md
[15]: ../PropertyHostMethods/README.md
[16]: ../PropertyHostMethods/IsDirty.md
[17]: ../PropertyHostMethods/MarkAsClean.md
[18]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"