Client.ConnectAsync Method (String, Int32, Int32)
=================================================
  Attempts to connect to a local or remote Tungsten RPC Server asynchronously

  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public Task ConnectAsync(
	string remoteAddress,
	int remotePort,
	int msTimeout = 10000
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

#### Return Value
Type: [Task][4]  
A Task which can be awaited

See Also
--------

#### Reference
[Client Class][5]  
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: http://msdn.microsoft.com/en-us/library/dd235678
[5]: README.md
[6]: ../../_icons/Help.png