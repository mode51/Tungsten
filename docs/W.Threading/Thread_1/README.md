Thread&lt;T> Class
==================
  A thread wrapper which makes multi-threading easier


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.ThreadBase][2]  
    [W.Threading.Thread][3]  
      **W.Threading.Thread<T>**  
        [W.Threading.Gate&lt;T>][4]  

  **Namespace:**  [W.Threading][5]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Thread<T> : Thread

```

#### Type Parameters

##### *T*

[Missing &lt;typeparam name="T"/> documentation for "T:W.Threading.Thread`1"]


The **Thread<T>** type exposes the following members.


Constructors
------------

                 | Name              | Description         
---------------- | ----------------- | ------------------- 
![Public method] | [Thread&lt;T>][6] | Starts a new thread 


Properties
----------

                      | Name        | Description 
--------------------- | ----------- | ----------- 
![Protected property] | [Action][7] |             


Methods
-------

                                 | Name              | Description                                                                                               
-------------------------------- | ----------------- | --------------------------------------------------------------------------------------------------------- 
![Public method]![Static member] | [Create][8]       | Starts a new thread                                                                                       
![Protected method]              | [InvokeAction][9] | Overridden implementation which calls Action with CustomData (Overrides [ThreadBase.InvokeAction()][10].) 


Extension Methods
-----------------

                           | Name                     | Description                                              
-------------------------- | ------------------------ | -------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][11] | Starts a new thread (Defined by [ThreadExtensions][12].) 


See Also
--------

#### Reference
[W.Threading Namespace][5]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../ThreadBase/README.md
[3]: ../Thread/README.md
[4]: ../Gate_1/README.md
[5]: ../README.md
[6]: _ctor.md
[7]: Action.md
[8]: Create.md
[9]: InvokeAction.md
[10]: ../ThreadBase/InvokeAction.md
[11]: ../ThreadExtensions/CreateThread__1.md
[12]: ../ThreadExtensions/README.md
[13]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected property]: ../../_icons/protproperty.gif "Protected property"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"