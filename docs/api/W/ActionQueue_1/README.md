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
public class ActionQueue<T> : IDisposable

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

                    | Name                  | Description                                                   
------------------- | --------------------- | ------------------------------------------------------------- 
![Public method]    | [Cancel][7]           | Cancels processing of the queue                               
![Public method]    | [Dispose][8]          | Releases resources related to the ActionQueue                 
![Public method]    | [Enqueue][9]          | Places an item in the queue                                   
![Public method]    | [Equals][10]          | (Inherited from [Object][1].)                                 
![Protected method] | [Finalize][11]        | Disposes the ActionQueue (Overrides [Object.Finalize()][12].) 
![Public method]    | [GetHashCode][13]     | (Inherited from [Object][1].)                                 
![Public method]    | [GetType][14]         | (Inherited from [Object][1].)                                 
![Protected method] | [MemberwiseClone][15] | (Inherited from [Object][1].)                                 
![Public method]    | [ToString][16]        | (Inherited from [Object][1].)                                 


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][17]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][18].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][19]     | Serializes an object to a Json string (Defined by [AsExtensions][18].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][20]      | Serializes an object to an xml string (Defined by [AsExtensions][18].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][21]   | Starts a new thread (Defined by [ThreadExtensions][22].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][23] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][24].) 
![Public Extension Method]                | [IsDirty][25]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][24].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][26]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][24].)                                                                                                  


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
[8]: Dispose.md
[9]: Enqueue.md
[10]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[11]: Finalize.md
[12]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[13]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[14]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[15]: http://msdn.microsoft.com/en-us/library/57ctke0a
[16]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[17]: ../AsExtensions/As__1.md
[18]: ../AsExtensions/README.md
[19]: ../AsExtensions/AsJson__1.md
[20]: ../AsExtensions/AsXml__1.md
[21]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[22]: ../../W.Threading/ThreadExtensions/README.md
[23]: ../PropertyHostMethods/InitializeProperties.md
[24]: ../PropertyHostMethods/README.md
[25]: ../PropertyHostMethods/IsDirty.md
[26]: ../PropertyHostMethods/MarkAsClean.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"