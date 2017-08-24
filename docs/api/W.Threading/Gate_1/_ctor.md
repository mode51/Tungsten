Gate&lt;T> Constructor
======================
  Construct a Gate

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Gate(
	Action<T, CancellationTokenSource> action,
	Action<bool, Exception> onExit = null,
	T args = null
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[T][3], [CancellationTokenSource][4]>  
The action to execute in a background task

##### *onExit* (Optional)
Type: [System.Action][2]&lt;[Boolean][5], [Exception][6]>  
Called when the task completes

##### *args* (Optional)
Type: [T][3]  

[Missing &lt;param name="args"/> documentation for "M:W.Threading.Gate`1.#ctor(System.Action{`0,System.Threading.CancellationTokenSource},System.Action{System.Boolean,System.Exception},`0)"]



See Also
--------

#### Reference
[Gate&lt;T> Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: README.md
[4]: http://msdn.microsoft.com/en-us/library/dd321629
[5]: http://msdn.microsoft.com/en-us/library/a28wyd50
[6]: http://msdn.microsoft.com/en-us/library/c18k6c59