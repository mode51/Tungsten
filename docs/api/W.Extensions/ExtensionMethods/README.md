ExtensionMethods Class
======================
   Tungsten extension methods


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Extensions.ExtensionMethods**  

  **Namespace:**  [W.Extensions][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static class ExtensionMethods
```

The **ExtensionMethods** type exposes the following members.


Methods
-------

                                 | Name                                                                            | Description                                                                          
-------------------------------- | ------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------ 
![Public method]![Static member] | [Lock&lt;TItemType>][3]                                                         | Performs the given action in a lock statement using the provided object for the lock 
![Public method]![Static member] | [WaitForValue&lt;TItemType>][4]                                                 | Initiates a Task which will wait for the specified condition to be met               
![Public method]![Static member] | [WaitForValueAsync(Object, Object, Int32)][5]                                   | Initiates a Task which will wait for the given variable to have the specified value  
![Public method]![Static member] | [WaitForValueAsync&lt;TItemType>(TItemType, Predicate&lt;TItemType>, Int32)][6] | Initiates a Task which will wait for the specified condition to be met               


See Also
--------

#### Reference
[W.Extensions Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: Lock__1.md
[4]: WaitForValue__1.md
[5]: WaitForValueAsync.md
[6]: WaitForValueAsync__1.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"