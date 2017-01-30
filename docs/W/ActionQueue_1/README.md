ActionQueue&lt;T> Class
=======================
  
[Missing &lt;summary> documentation for "T:W.ActionQueue`1"]



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.ActionQueue<T>**  

  **Namespace:**  [W][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class ActionQueue<T>

```

#### Type Parameters

##### *T*

[Missing &lt;typeparam name="T"/> documentation for "T:W.ActionQueue`1"]


The **ActionQueue<T>** type exposes the following members.


Constructors
------------

                 | Name                                                | Description               
---------------- | --------------------------------------------------- | ------------------------- 
![Public method] | [ActionQueue&lt;T>(Action&lt;T>, String)][3]        | Creates a new ActionQueue 
![Public method] | [ActionQueue&lt;T>(Func&lt;T, Boolean>, String)][4] | Creates a new ActionQueue 


Properties
----------

                   | Name       | Description                                        
------------------ | ---------- | -------------------------------------------------- 
![Public property] | [Count][5] | Returns the number of items currently in the queue 
![Public property] | [Queue][6] | The reference to the ConcurrentQueue being used    


Methods
-------

                 | Name         | Description                     
---------------- | ------------ | ------------------------------- 
![Public method] | [Cancel][7]  | Cancels processing of the queue 
![Public method] | [Enqueue][8] | Places an item in the queue     


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
[W Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: _ctor_1.md
[5]: Count.md
[6]: Queue.md
[7]: Cancel.md
[8]: Enqueue.md
[9]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[10]: ../../W.Threading/ThreadExtensions/README.md
[11]: ../PropertyHostMethods/InitializeProperties.md
[12]: ../PropertyHostMethods/README.md
[13]: ../PropertyHostMethods/IsDirty.md
[14]: ../PropertyHostMethods/MarkAsClean.md
[15]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"