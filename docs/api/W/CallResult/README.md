CallResult Class
================
   A non-generic return value for a function. CallResult encapsulates a success/failure and an exception.


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.CallResult**  
    [W.CallResult&lt;TResult>][2]  

  **Namespace:**  [W][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class CallResult
```

The **CallResult** type exposes the following members.


Constructors
------------

                 | Name                                | Description                                                                               
---------------- | ----------------------------------- | ----------------------------------------------------------------------------------------- 
![Public method] | [CallResult()][4]                   | Default constructor, initializes Success to false                                         
![Public method] | [CallResult(Boolean)][5]            | Constructor which accepts an initial value for Success                                    
![Public method] | [CallResult(Boolean, Exception)][6] | Constructor which accepts an initial value for Success and an initial value for Exception 


Properties
----------

                                   | Name           | Description                                            
---------------------------------- | -------------- | ------------------------------------------------------ 
![Public property]![Static member] | [Empty][7]     | Provides a new instance of an uninitialized CallResult 
![Public property]                 | [Exception][8] | Provide exception data to the caller if desired        
![Public property]                 | [Success][9]   | Set to True if the function succeeds, otherwise False  


Methods
-------

                    | Name                  | Description                   
------------------- | --------------------- | ----------------------------- 
![Public method]    | [Equals][10]          | (Inherited from [Object][1].) 
![Protected method] | [Finalize][11]        | (Inherited from [Object][1].) 
![Public method]    | [GetHashCode][12]     | (Inherited from [Object][1].) 
![Public method]    | [GetType][13]         | (Inherited from [Object][1].) 
![Protected method] | [MemberwiseClone][14] | (Inherited from [Object][1].) 
![Public method]    | [ToString][15]        | (Inherited from [Object][1].) 


See Also
--------

#### Reference
[W Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../CallResult_1/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: _ctor_1.md
[6]: _ctor_2.md
[7]: Empty.md
[8]: Exception.md
[9]: Success.md
[10]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[11]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[12]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[13]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[14]: http://msdn.microsoft.com/en-us/library/57ctke0a
[15]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"