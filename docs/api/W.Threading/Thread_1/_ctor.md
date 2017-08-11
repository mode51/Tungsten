Thread&lt;TCustomData> Constructor
==================================
  Constructs a new Thread object

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Thread(
	Action<TCustomData, CancellationTokenSource> action,
	Action<bool, Exception> onExit = null,
	TCustomData customData = null,
	CancellationTokenSource cts = null
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[TCustomData][3], [CancellationTokenSource][4]>  
The Action to be called in the thread

##### *onExit* (Optional)
Type: [System.Action][2]&lt;[Boolean][5], [Exception][6]>  
The Action to be called when the thread completes

##### *customData* (Optional)
Type: [TCustomData][3]  
The custom data to be passed into the thread

##### *cts* (Optional)
Type: [System.Threading.CancellationTokenSource][4]  
A CancellationTokenSource which can be used to cancel the thread


See Also
--------

#### Reference
[Thread&lt;TCustomData> Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: README.md
[4]: http://msdn.microsoft.com/en-us/library/dd321629
[5]: http://msdn.microsoft.com/en-us/library/a28wyd50
[6]: http://msdn.microsoft.com/en-us/library/c18k6c59