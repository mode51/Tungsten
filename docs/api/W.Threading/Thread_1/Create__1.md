Thread&lt;TCustomData>.Create&lt;TCustomDataType> Method
========================================================
  Starts a new thread

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Thread<TCustomDataType> Create<TCustomDataType>(
	Action<TCustomDataType, CancellationTokenSource> action,
	Action<bool, Exception> onExit = null,
	TCustomDataType customData = null,
	CancellationTokenSource cts = null
)

```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;**TCustomDataType**, [CancellationTokenSource][3]>  
Action to call on a thread

##### *onExit* (Optional)
Type: [System.Action][2]&lt;[Boolean][4], [Exception][5]>  
Action to call upon comletion. Executes on the same thread as Action.

##### *customData* (Optional)
Type: **TCustomDataType**  
Custom data to be passed into the thread

##### *cts* (Optional)
Type: [System.Threading.CancellationTokenSource][3]  
A CancellationTokenSource which can be used to cancel the operation

#### Type Parameters

##### *TCustomDataType*

[Missing &lt;typeparam name="TCustomDataType"/> documentation for "M:W.Threading.Thread`1.Create``1(System.Action{``0,System.Threading.CancellationTokenSource},System.Action{System.Boolean,System.Exception},``0,System.Threading.CancellationTokenSource)"]


#### Return Value
Type: [Thread][6]&lt;**TCustomDataType**>  
A new long-running Thread object

See Also
--------

#### Reference
[Thread&lt;TCustomData> Class][6]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: http://msdn.microsoft.com/en-us/library/dd321629
[4]: http://msdn.microsoft.com/en-us/library/a28wyd50
[5]: http://msdn.microsoft.com/en-us/library/c18k6c59
[6]: README.md