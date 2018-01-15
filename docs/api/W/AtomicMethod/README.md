AtomicMethod Class
==================
   Wraps an AtomicMethodDelegate with functionality to prevent re-entrancy and to know when the method is running and completed


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.AtomicMethod**  

  **Namespace:**  [W][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class AtomicMethod
```

The **AtomicMethod** type exposes the following members.


Constructors
------------

                 | Name                                    | Description                   
---------------- | --------------------------------------- | ----------------------------- 
![Public method] | [AtomicMethod(Action)][3]               | Constructs a new AtomicMethod 
![Public method] | [AtomicMethod(AtomicMethodDelegate)][4] | Constructs a new AtomicMethod 


Properties
----------

                   | Name            | Description                                                                                                            
------------------ | --------------- | ---------------------------------------------------------------------------------------------------------------------- 
![Public property] | [IsComplete][5] | True if the method has completed, otherwise False. The value is False initially and reset to False when Run is called. 
![Public property] | [IsRunning][6]  | True if the method is currently running, otherwise False                                                               


Methods
-------

                                 | Name                                           | Description                                                                                                                                                       
-------------------------------- | ---------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]                 | [Equals][7]                                    | (Inherited from [Object][1].)                                                                                                                                     
![Protected method]              | [Finalize][8]                                  | Destructs the AtomicMethod (Overrides [Object.Finalize()][9].)                                                                                                    
![Public method]                 | [GetHashCode][10]                              | (Inherited from [Object][1].)                                                                                                                                     
![Public method]                 | [GetType][11]                                  | (Inherited from [Object][1].)                                                                                                                                     
![Protected method]              | [MemberwiseClone][12]                          | (Inherited from [Object][1].)                                                                                                                                     
![Public method]![Static member] | [Run(Action)][13]                              | Creates a new AtomicMethod, immediately calls Run and returns the AtomicMethod instance                                                                           
![Public method]                 | [Run(Object[])][14]                            | Synchronously calls the delegate with the specified arguments. These arguments must be accurate in number and in type to the arguments expected by the delegate.  
![Public method]![Static member] | [Run(AtomicMethodDelegate)][15]                | Creates a new AtomicMethod, immediately calls Run and returns the AtomicMethod instance                                                                           
![Public method]![Static member] | [Run(AtomicMethodDelegate, Object[])][16]      | Creates a new AtomicMethod, immediately calls Run and returns the AtomicMethod instance                                                                           
![Public method]![Static member] | [RunAsync(Action)][17]                         | Creates a new AtomicMethod, immediately calls RunAsync and returns the AtomicMethod instance                                                                      
![Public method]                 | [RunAsync(Object[])][18]                       | Asynchronously calls the delegate with the specified arguments. These arguments must be accurate in number and in type to the arguments expected by the delegate. 
![Public method]![Static member] | [RunAsync(AtomicMethodDelegate)][19]           | Creates a new AtomicMethod, immediately calls RunAsync and returns the AtomicMethod instance                                                                      
![Public method]                 | [RunAsync(CancellationToken, Object[])][20]    | Calls the delegate with the specified arguments. These arguments must be accurate in number and in type to the arguments expected by the delegate.                
![Public method]![Static member] | [RunAsync(AtomicMethodDelegate, Object[])][21] | Creates a new AtomicMethod, immediately calls RunAsync and returns the AtomicMethod instance                                                                      
![Public method]                 | [ToString][22]                                 | (Inherited from [Object][1].)                                                                                                                                     
![Public method]                 | [Wait()][23]                                   | Block the calling thread until the method completes                                                                                                               
![Public method]                 | [Wait(Int32)][24]                              | Block the calling thread until the method completes                                                                                                               
![Public method]                 | [Wait(CancellationToken)][25]                  | Block the calling thread until the method completes                                                                                                               


See Also
--------

#### Reference
[W Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: _ctor_1.md
[5]: IsComplete.md
[6]: IsRunning.md
[7]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[8]: Finalize.md
[9]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[10]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[11]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[12]: http://msdn.microsoft.com/en-us/library/57ctke0a
[13]: Run.md
[14]: Run_1.md
[15]: Run_2.md
[16]: Run_3.md
[17]: RunAsync.md
[18]: RunAsync_1.md
[19]: RunAsync_3.md
[20]: RunAsync_2.md
[21]: RunAsync_4.md
[22]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[23]: Wait.md
[24]: Wait_1.md
[25]: Wait_2.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Static member]: ../../_icons/static.gif "Static member"