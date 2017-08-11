ThreadExtensions Class
======================
  Contains a generic extension method to quickly start a new thread


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.ThreadExtensions**  

  **Namespace:**  [W.Threading][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static class ThreadExtensions
```


Methods
-------

                                 | Name                                                                                                     | Description         
-------------------------------- | -------------------------------------------------------------------------------------------------------- | ------------------- 
![Public method]![Static member] | [CreateThread&lt;T>(T, Action&lt;T, CancellationTokenSource>, Action&lt;Boolean, Exception>)][3]         | Starts a new thread 
![Public method]![Static member] | [CreateThread&lt;T>(Object, Action&lt;T, CancellationTokenSource>, Action&lt;Boolean, Exception>, T)][4] | Starts a new thread 


See Also
--------

#### Reference
[W.Threading Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: CreateThread__1_1.md
[4]: CreateThread__1.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"