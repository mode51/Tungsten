InvokeExtensions Class
======================
   Extensions to provide code shortcuts to evaluate InvokeRequired and run code appropriately


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.InvokeExtensions.InvokeExtensions**  

  **Namespace:**  [W.InvokeExtensions][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static class InvokeExtensions
```


Methods
-------

                                 | Name                                       | Description                                                                        
-------------------------------- | ------------------------------------------ | ---------------------------------------------------------------------------------- 
![Public method]![Static member] | [InvokeEx&lt;T>(T, Action&lt;T>)][3]       | Runs the provided Action on the UI thread                                          
![Public method]![Static member] | [InvokeEx&lt;T>(T, Func&lt;T, Object>)][4] | Runs the provided Function on the UI thread. Avoids the cross-threaded exceptions. 
![Public method]![Static member] | [InvokeEx&lt;T, U>(T, Func&lt;T, U>)][5]   | Runs the provided Function on the UI thread. Avoids the cross-threaded exceptions. 


See Also
--------

#### Reference
[W.InvokeExtensions Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: InvokeEx__1.md
[4]: InvokeEx__1_1.md
[5]: InvokeEx__2.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"