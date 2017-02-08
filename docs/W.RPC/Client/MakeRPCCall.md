Client.MakeRPCCall Method (String, Action, Object[])
====================================================
  Not sure I should keep this method. Shouldn't all RPC calls have a result? Otherwise, the client wouldn't know if it succeeded.

  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public ManualResetEvent MakeRPCCall(
	string methodName,
	Action onResponse,
	params Object[] args
)
```

#### Parameters

##### *methodName*
Type: [System.String][2]  

[Missing &lt;param name="methodName"/> documentation for "M:W.RPC.Client.MakeRPCCall(System.String,System.Action,System.Object[])"]


##### *onResponse*
Type: [System.Action][3]  

[Missing &lt;param name="onResponse"/> documentation for "M:W.RPC.Client.MakeRPCCall(System.String,System.Action,System.Object[])"]


##### *args*
Type: [System.Object][4][]  

[Missing &lt;param name="args"/> documentation for "M:W.RPC.Client.MakeRPCCall(System.String,System.Action,System.Object[])"]


#### Return Value
Type: [ManualResetEvent][5]  

[Missing &lt;returns> documentation for "M:W.RPC.Client.MakeRPCCall(System.String,System.Action,System.Object[])"]


See Also
--------

#### Reference
[Client Class][6]  
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/bb534741
[4]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[5]: http://msdn.microsoft.com/en-us/library/2ssskfws
[6]: README.md
[7]: ../../_icons/Help.png