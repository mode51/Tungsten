ActionQueue&lt;T> Constructor (Func&lt;T, Boolean>, String)
===========================================================
  Creates a new ActionQueue

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public ActionQueue(
	Func<T, bool> onItemAdded,
	string caller = ""
)
```

#### Parameters

##### *onItemAdded*
Type: [System.Func][2]&lt;[T][3], [Boolean][4]>  
A callback which is called whenever an item has been enqueued

##### *caller* (Optional)
Type: [System.String][5]  

[Missing &lt;param name="caller"/> documentation for "M:W.ActionQueue`1.#ctor(System.Func{`0,System.Boolean},System.String)"]



See Also
--------

#### Reference
[ActionQueue&lt;T> Class][3]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549151
[3]: README.md
[4]: http://msdn.microsoft.com/en-us/library/a28wyd50
[5]: http://msdn.microsoft.com/en-us/library/s1wwdcbf