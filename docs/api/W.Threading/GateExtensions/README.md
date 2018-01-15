GateExtensions Class
====================
   Extension methods on Action to Create a Gate


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.GateExtensions**  

  **Namespace:**  [W.Threading][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static class GateExtensions
```


Methods
-------

                                 | Name                                                                         | Description                             
-------------------------------- | ---------------------------------------------------------------------------- | --------------------------------------- 
![Public method]![Static member] | [AsGate(Action&lt;CancellationToken>)][3]                                    | Creates a Gate with the supplied action 
![Public method]![Static member] | [AsGate&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>)][4] | Creates a Gate with the supplied action 


See Also
--------

#### Reference
[W.Threading Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: AsGate.md
[4]: AsGate__1.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"