Client.Connect Method (IPAddress, Int32, Int32, Action&lt;ISocketClient, IPAddress>, Action&lt;Exception>)
==========================================================================================================
  Attempts to connect to a local or remote Tungsten RPC Server

  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public bool Connect(
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
The IP address of the Tungsten RPC Server

##### *remotePort*
Type: [System.Int32][3]  
The port on which the Tungsten RPC Server is listening

##### *msTimeout* (Optional)
Type: [System.Int32][3]  
The call will fail if the client can't connect within the specified elapsed time (in milliseconds)

##### *onConnection* (Optional)
Type: [System.Action][4]&lt;[ISocketClient][5], [IPAddress][2]>  

[Missing &lt;param name="onConnection"/> documentation for "M:W.RPC.Client.Connect(System.Net.IPAddress,System.Int32,System.Int32,System.Action{W.RPC.ISocketClient,System.Net.IPAddress},System.Action{System.Exception})"]


##### *onException* (Optional)
Type: [System.Action][6]&lt;[Exception][7]>  

[Missing &lt;param name="onException"/> documentation for "M:W.RPC.Client.Connect(System.Net.IPAddress,System.Int32,System.Int32,System.Action{W.RPC.ISocketClient,System.Net.IPAddress},System.Action{System.Exception})"]


#### Return Value
Type: [Boolean][8]  
A CallResult specifying success/failure and an Exception if one ocurred
#### Implements
[ISocketClient.Connect(IPAddress, Int32, Int32, Action&lt;ISocketClient, IPAddress>, Action&lt;Exception>)][9]  


See Also
--------

#### Reference
[Client Class][10]  
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s128tyf6
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: http://msdn.microsoft.com/en-us/library/bb549311
[5]: ../ISocketClient/README.md
[6]: http://msdn.microsoft.com/en-us/library/018hxwa8
[7]: http://msdn.microsoft.com/en-us/library/c18k6c59
[8]: http://msdn.microsoft.com/en-us/library/a28wyd50
[9]: ../ISocketClient/Connect.md
[10]: README.md
[11]: ../../_icons/Help.png