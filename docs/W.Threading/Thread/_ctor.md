Thread Constructor
==================
  Starts a new thread

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Thread(
	Action<CancellationTokenSource> action,
	Action<bool, Exception> onComplete = null
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[CancellationTokenSource][3]>  
Action to call on a thread

##### *onComplete* (Optional)
Type: [System.Action][4]&lt;[Boolean][5], [Exception][6]>  
Action to call upon comletion. Executes on the same thread as Action.

#### Return Value
Type:   


See Also
--------

#### Reference
[Thread Class][7]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/018hxwa8
[3]: http://msdn.microsoft.com/en-us/library/dd321629
[4]: http://msdn.microsoft.com/en-us/library/bb549311
[5]: http://msdn.microsoft.com/en-us/library/a28wyd50
[6]: http://msdn.microsoft.com/en-us/library/c18k6c59
[7]: README.md
[8]: ../../_icons/Help.png