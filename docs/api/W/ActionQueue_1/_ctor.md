ActionQueue&lt;T> Constructor (Action&lt;T>, String)
====================================================
  Creates a new ActionQueue

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public ActionQueue(
	Action<T> onItemAdded,
	string caller = ""
)
```

#### Parameters

##### *onItemAdded*
Type: [System.Action][2]&lt;[T][3]>  
A callback which is called whenever an item has been enqueued

##### *caller* (Optional)
Type: [System.String][4]  

[Missing &lt;param name="caller"/> documentation for "M:W.ActionQueue`1.#ctor(System.Action{`0},System.String)"]



See Also
--------

#### Reference
[ActionQueue&lt;T> Class][3]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/018hxwa8
[3]: README.md
[4]: http://msdn.microsoft.com/en-us/library/s1wwdcbf