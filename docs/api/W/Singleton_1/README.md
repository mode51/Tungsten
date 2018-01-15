Singleton&lt;TSingletonType> Class
==================================
   Thread-safe Singleton implementation


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Singleton<TSingletonType>**  

  **Namespace:**  [W][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Singleton<TSingletonType>
where TSingletonType : class, new()

```

#### Type Parameters

##### *TSingletonType*
The singleton Type

The **Singleton<TSingletonType>** type exposes the following members.


Constructors
------------

                 | Name                              | Description                                                           
---------------- | --------------------------------- | --------------------------------------------------------------------- 
![Public method] | [Singleton&lt;TSingletonType>][3] | Initializes a new instance of the **Singleton<TSingletonType>** class 


Properties
----------

                                   | Name          | Description           
---------------------------------- | ------------- | --------------------- 
![Public property]![Static member] | [Instance][4] | Returns the singleton 


Methods
-------

                    | Name                 | Description                   
------------------- | -------------------- | ----------------------------- 
![Public method]    | [Equals][5]          | (Inherited from [Object][1].) 
![Protected method] | [Finalize][6]        | (Inherited from [Object][1].) 
![Public method]    | [GetHashCode][7]     | (Inherited from [Object][1].) 
![Public method]    | [GetType][8]         | (Inherited from [Object][1].) 
![Protected method] | [MemberwiseClone][9] | (Inherited from [Object][1].) 
![Public method]    | [ToString][10]       | (Inherited from [Object][1].) 


Remarks
-------
Adapted from MSDN article "Implementing Singleton in C#"

See Also
--------

#### Reference
[W Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: Instance.md
[5]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[6]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[7]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[8]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[9]: http://msdn.microsoft.com/en-us/library/57ctke0a
[10]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"