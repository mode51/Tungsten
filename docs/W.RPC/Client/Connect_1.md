Client.Connect Method (String, Int32, Int32, Action&lt;Client, IPAddress>)
==========================================================================
  Attempts to connect to a local or remote Tungsten RPC Server

  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public CallResult Connect(
	string remoteAddress,
	int remotePort,
	int msTimeout = 10000,
	Action<Client, IPAddress> onConnection = null
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
Type: [System.Action][4]&lt;[Client][5], [IPAddress][6]>  

[Missing &lt;param name="onConnection"/> documentation for "M:W.RPC.Client.Connect(System.String,System.Int32,System.Int32,System.Action{W.RPC.Client,System.Net.IPAddress})"]


#### Return Value
Type: [CallResult][7]  
A CallResult specifying success/failure and an Exception if one ocurred

See Also
--------

#### Reference
[Client Class][5]  
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: http://msdn.microsoft.com/en-us/library/bb549311
[5]: README.md
[6]: http://msdn.microsoft.com/en-us/library/s128tyf6
[7]: ../../W/CallResult/README.md
[8]: ../../_icons/Help.png