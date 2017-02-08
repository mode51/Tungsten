ISocketClient.Connect Method (IPAddress, Int32, Int32, Action&lt;ISocketClient, IPAddress>, Action&lt;Exception>)
=================================================================================================================
  
[Missing &lt;summary> documentation for "M:W.RPC.ISocketClient.Connect(System.Net.IPAddress,System.Int32,System.Int32,System.Action{W.RPC.ISocketClient,System.Net.IPAddress},System.Action{System.Exception})"]


  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC.Interfaces (in Tungsten.RPC.Interfaces.dll)

Syntax
------

```csharp
bool Connect(
	IPAddress remoteAddress,
	int remotePort,
	int msTimeout = 10000,
	Action<ISocketClient, IPAddress> onConnection = null,
	Action<Exception> onException = null
)
```

#### Parameters

##### *remoteAddress*
Type: [System.Net.IPAddress][2]  

[Missing &lt;param name="remoteAddress"/> documentation for "M:W.RPC.ISocketClient.Connect(System.Net.IPAddress,System.Int32,System.Int32,System.Action{W.RPC.ISocketClient,System.Net.IPAddress},System.Action{System.Exception})"]


##### *remotePort*
Type: [System.Int32][3]  

[Missing &lt;param name="remotePort"/> documentation for "M:W.RPC.ISocketClient.Connect(System.Net.IPAddress,System.Int32,System.Int32,System.Action{W.RPC.ISocketClient,System.Net.IPAddress},System.Action{System.Exception})"]


##### *msTimeout* (Optional)
Type: [System.Int32][3]  

[Missing &lt;param name="msTimeout"/> documentation for "M:W.RPC.ISocketClient.Connect(System.Net.IPAddress,System.Int32,System.Int32,System.Action{W.RPC.ISocketClient,System.Net.IPAddress},System.Action{System.Exception})"]


##### *onConnection* (Optional)
Type: [System.Action][4]&lt;[ISocketClient][5], [IPAddress][2]>  

[Missing &lt;param name="onConnection"/> documentation for "M:W.RPC.ISocketClient.Connect(System.Net.IPAddress,System.Int32,System.Int32,System.Action{W.RPC.ISocketClient,System.Net.IPAddress},System.Action{System.Exception})"]


##### *onException* (Optional)
Type: [System.Action][6]&lt;[Exception][7]>  

[Missing &lt;param name="onException"/> documentation for "M:W.RPC.ISocketClient.Connect(System.Net.IPAddress,System.Int32,System.Int32,System.Action{W.RPC.ISocketClient,System.Net.IPAddress},System.Action{System.Exception})"]


#### Return Value
Type: [Boolean][8]  

[Missing &lt;returns> documentation for "M:W.RPC.ISocketClient.Connect(System.Net.IPAddress,System.Int32,System.Int32,System.Action{W.RPC.ISocketClient,System.Net.IPAddress},System.Action{System.Exception})"]


See Also
--------

#### Reference
[ISocketClient Interface][5]  
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s128tyf6
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: http://msdn.microsoft.com/en-us/library/bb549311
[5]: README.md
[6]: http://msdn.microsoft.com/en-us/library/018hxwa8
[7]: http://msdn.microsoft.com/en-us/library/c18k6c59
[8]: http://msdn.microsoft.com/en-us/library/a28wyd50
[9]: ../../_icons/Help.png