Client.Connect Method (String, Int32, Int32, Action&lt;ISocketClient, IPAddress>, Action&lt;Exception>)
=======================================================================================================
  Attempts to connect to a local or remote Tungsten RPC Server

  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public bool Connect(
	string remoteAddress,
	int remotePort,
	int msTimeout = 10000,
	Action<ISocketClient, IPAddress> onConnection = null,
	Action<Exception> onException = null
)
```

#### Parameters

##### *remoteAddress*
Type: [System.String][2]  
The IP address of the Tungsten RPC Server

##### *remotePort*
Type: [System.Int32][3]  
The port on which the Tungsten RPC Server is listening

##### *msTimeout* (Optional)
Type: [System.Int32][3]  
The call will fail if the client can't connect within the specified elapsed time (in milliseconds)

##### *onConnection* (Optional)
Type: [System.Action][4]&lt;[ISocketClient][5], [IPAddress][6]>  

[Missing &lt;param name="onConnection"/> documentation for "M:W.RPC.Client.Connect(System.String,System.Int32,System.Int32,System.Action{W.RPC.ISocketClient,System.Net.IPAddress},System.Action{System.Exception})"]


##### *onException* (Optional)
Type: [System.Action][7]&lt;[Exception][8]>  

[Missing &lt;param name="onException"/> documentation for "M:W.RPC.Client.Connect(System.String,System.Int32,System.Int32,System.Action{W.RPC.ISocketClient,System.Net.IPAddress},System.Action{System.Exception})"]


#### Return Value
Type: [Boolean][9]  
A CallResult specifying success/failure and an Exception if one ocurred
#### Implements
[ISocketClient.Connect(String, Int32, Int32, Action&lt;ISocketClient, IPAddress>, Action&lt;Exception>)][10]  


See Also
--------

#### Reference
[Client Class][11]  
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: http://msdn.microsoft.com/en-us/library/bb549311
[5]: ../ISocketClient/README.md
[6]: http://msdn.microsoft.com/en-us/library/s128tyf6
[7]: http://msdn.microsoft.com/en-us/library/018hxwa8
[8]: http://msdn.microsoft.com/en-us/library/c18k6c59
[9]: http://msdn.microsoft.com/en-us/library/a28wyd50
[10]: ../ISocketClient/Connect_1.md
[11]: README.md
[12]: ../../_icons/Help.png