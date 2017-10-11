Thread.Create Method (Action&lt;CancellationToken>)
===================================================
   Starts a new thread

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Thread Create(
	Action<CancellationToken> action
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[CancellationToken][3]>  
Action to call on a thread

#### Return Value
Type: [Thread][4]  
A new Thread

See Also
--------

#### Reference
[Thread Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/018hxwa8
[3]: http://msdn.microsoft.com/en-us/library/dd384802
[4]: README.md