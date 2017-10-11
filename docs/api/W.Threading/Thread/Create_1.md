Thread.Create Method (Action&lt;CancellationToken>, Boolean)
============================================================
   Starts a new thread

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Thread Create(
	Action<CancellationToken> action,
	bool autoStart
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[CancellationToken][3]>  
Action to call on a thread

##### *autoStart*
Type: [System.Boolean][4]  
If True, the thread immediately starts

#### Return Value
Type: [Thread][5]  
A new Thread

See Also
--------

#### Reference
[Thread Class][5]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/018hxwa8
[3]: http://msdn.microsoft.com/en-us/library/dd384802
[4]: http://msdn.microsoft.com/en-us/library/a28wyd50
[5]: README.md