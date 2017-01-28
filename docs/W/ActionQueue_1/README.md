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

                           | Name                    | Description                                              
-------------------------- | ----------------------- | -------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][9] | Starts a new thread (Defined by [ThreadExtensions][10].) 


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
[11]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"