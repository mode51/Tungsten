EventTemplate Class
===================
   Wraps the functionality of delegate, event and RaiseXXX into a single class


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.EventTemplate**  

  **Namespace:**  [W][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class EventTemplate
```

The **EventTemplate** type exposes the following members.


Constructors
------------

                 | Name               | Description                                               
---------------- | ------------------ | --------------------------------------------------------- 
![Public method] | [EventTemplate][3] | Initializes a new instance of the **EventTemplate** class 


Methods
-------

                    | Name                 | Description                   
------------------- | -------------------- | ----------------------------- 
![Public method]    | [Equals][4]          | (Inherited from [Object][1].) 
![Protected method] | [Finalize][5]        | (Inherited from [Object][1].) 
![Public method]    | [GetHashCode][6]     | (Inherited from [Object][1].) 
![Public method]    | [GetType][7]         | (Inherited from [Object][1].) 
![Protected method] | [MemberwiseClone][8] | (Inherited from [Object][1].) 
![Public method]    | [Raise][9]           | Raises the template event     
![Public method]    | [ToString][10]       | (Inherited from [Object][1].) 


Events
------

                | Name           | Description        
--------------- | -------------- | ------------------ 
![Public event] | [OnRaised][11] | The template event 


See Also
--------

#### Reference
[W Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[5]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[6]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[7]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[8]: http://msdn.microsoft.com/en-us/library/57ctke0a
[9]: Raise.md
[10]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[11]: OnRaised.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public event]: ../../_icons/pubevent.gif "Public event"