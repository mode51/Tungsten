Thread&lt;T> Constructor
========================
  Starts a new thread

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Thread(
	Action<T, CancellationTokenSource> action,
	Action<bool, Exception> onComplete = null,
	T customData = null
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[T][3], [CancellationTokenSource][4]>  
Action to call on a thread

##### *onComplete* (Optional)
Type: [System.Action][2]&lt;[Boolean][5], [Exception][6]>  
Action to call upon comletion. Executes on the same thread as Action.

##### *customData* (Optional)
Type: [T][3]  
The data to pass to the call to the thread (Action)

#### Return Value
Type:   


See Also
--------

#### Reference
[Thread&lt;T> Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: README.md
[4]: http://msdn.microsoft.com/en-us/library/dd321629
[5]: http://msdn.microsoft.com/en-us/library/a28wyd50
[6]: http://msdn.microsoft.com/en-us/library/c18k6c59
[7]: ../../_icons/Help.png