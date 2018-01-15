ThreadExtensions Class
======================
   Contains a generic extension method to quickly start a new thread


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.ThreadExtensions.ThreadExtensions**  

  **Namespace:**  [W.Threading.ThreadExtensions][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static class ThreadExtensions
```


Methods
-------

                                 | Name                                                                                                        | Description                         
-------------------------------- | ----------------------------------------------------------------------------------------------------------- | ----------------------------------- 
![Public method]![Static member] | [CreateThread&lt;TParameterType>(Object, Action&lt;TParameterType, CancellationToken>)][3]                  | Creates and starts a new thread and 
![Public method]![Static member] | [CreateThread&lt;TParameterType>(TParameterType, Action&lt;TParameterType, CancellationToken>)][4]          | Creates and starts a new thread     
![Public method]![Static member] | [CreateThread&lt;TParameterType>(Object, Action&lt;TParameterType, CancellationToken>, Boolean)][5]         | Creates a new thread                
![Public method]![Static member] | [CreateThread&lt;TParameterType>(TParameterType, Action&lt;TParameterType, CancellationToken>, Boolean)][6] | Creates a new thread                


See Also
--------

#### Reference
[W.Threading.ThreadExtensions Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: CreateThread__1.md
[4]: CreateThread__1_2.md
[5]: CreateThread__1_1.md
[6]: CreateThread__1_3.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"