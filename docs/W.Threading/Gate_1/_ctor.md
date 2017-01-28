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
	Action<bool, Exception> onComplete = null
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[T][3], [CancellationTokenSource][4]>  

[Missing &lt;param name="action"/> documentation for "M:W.Threading.Gate`1.#ctor(System.Action{`0,System.Threading.CancellationTokenSource},System.Action{System.Boolean,System.Exception})"]


##### *onComplete* (Optional)
Type: [System.Action][2]&lt;[Boolean][5], [Exception][6]>  

[Missing &lt;param name="onComplete"/> documentation for "M:W.Threading.Gate`1.#ctor(System.Action{`0,System.Threading.CancellationTokenSource},System.Action{System.Boolean,System.Exception})"]



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
[7]: ../../_icons/Help.png