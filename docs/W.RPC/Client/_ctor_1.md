Client Constructor (IPAddress, Int32, Int32)
============================================
  Constructs a new Tungsten RPC Client and automatically connects to the specified remote server

  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public Client(
	IPAddress remoteAddress,
	int remotePort,
	int msTimeout = 10000
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


See Also
--------

#### Reference
[Client Class][4]  
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s128tyf6
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: README.md
[5]: ../../_icons/Help.png