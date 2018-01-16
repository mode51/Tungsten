ThreadSlim.Create Method (Action&lt;CancellationToken>)
=======================================================
   Constructs a new ThreadSlim using an Action&lt;CancellationToken> as the thread method

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static ThreadSlim Create(
	Action<CancellationToken> action
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[CancellationToken][3]>  
The action to run on a separate thread

#### Return Value
Type: [ThreadSlim][4]  

[Missing &lt;returns> documentation for "M:W.Threading.ThreadSlim.Create(System.Action{System.Threading.CancellationToken})"]


See Also
--------

#### Reference
[ThreadSlim Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/018hxwa8
[3]: http://msdn.microsoft.com/en-us/library/dd384802
[4]: README.md