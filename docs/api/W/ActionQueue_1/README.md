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
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"