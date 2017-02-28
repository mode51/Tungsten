ActionQueue&lt;T> Class
=======================
  
Allows the programmer to enqueue items for processing on a separate thread. The ActionQueue will process items sequentially whenever an item is added.



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
The type of data to enqueue and process

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
[W Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: _ctor_1.md
[5]: Count.md
[6]: Queue.md
[7]: Cancel.md
[8]: Enqueue.md
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
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"