Thread Constructor (Action&lt;CancellationToken>, Boolean)
==========================================================
   Constructs a new Thread

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Thread(
	Action<CancellationToken> action,
	bool autoStart
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[CancellationToken][3]>  
The action to call on a thread

##### *autoStart*
Type: [System.Boolean][4]  
If True, the thread will immediately start. Otherwise Start will have to be called.


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