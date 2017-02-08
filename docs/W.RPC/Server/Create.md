Server.Create Method
====================
  Creates a new Server instance and starts listening on the specified ipAddress and port

  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public static Server Create(
	IPAddress ipAddress,
	int port
)
```

#### Parameters

##### *ipAddress*
Type: [System.Net.IPAddress][2]  
The network address on which to listen

##### *port*
Type: [System.Int32][3]  
The port on which to listen

#### Return Value
Type: [Server][4]  
The new Server instance

See Also
--------

#### Reference
[Server Class][4]  
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s128tyf6
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: README.md
[5]: ../../_icons/Help.png