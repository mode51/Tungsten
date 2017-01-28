CallResult&lt;TResult> Class
============================
  
Generic class to be used as a return value. CallResult encapsulates a success/failure, an exception and a return value.



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.CallResult][2]  
    **W.CallResult<TResult>**  

  **Namespace:**  [W][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class CallResult<TResult> : CallResult

```

#### Type Parameters

##### *TResult*
The type to be used for the Result member (the return value of the function)

The **CallResult<TResult>** type exposes the following members.


Constructors
------------

                 | Name                                                     | Description                                                                                            
---------------- | -------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ 
![Public method] | [CallResult&lt;TResult>()][4]                            | Default constructor                                                                                    
![Public method] | [CallResult&lt;TResult>(Boolean)][5]                     | Constructor accepting an initial Success value                                                         
![Public method] | [CallResult&lt;TResult>(Boolean, TResult)][6]            | Constructor accepting an initial Success value and an initial Result value                             
![Public method] | [CallResult&lt;TResult>(Boolean, TResult, Exception)][7] | Constructor accepting an initial Success value, an initial Result value and an initial Exception value 


Properties
----------

                                   | Name        | Description                                                        
---------------------------------- | ----------- | ------------------------------------------------------------------ 
![Public property]![Static member] | [Empty][8]  | Provides a new instance of an uninitialized CallResult&lt;TResult> 
![Public property]                 | [Result][9] | The return value                                                   


Extension Methods
-----------------

                           | Name                     | Description                                              
-------------------------- | ------------------------ | -------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][10] | Starts a new thread (Defined by [ThreadExtensions][11].) 


See Also
--------

#### Reference
[W Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../CallResult/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: _ctor_1.md
[6]: _ctor_2.md
[7]: _ctor_3.md
[8]: Empty.md
[9]: Result.md
[10]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[11]: ../../W.Threading/ThreadExtensions/README.md
[12]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"