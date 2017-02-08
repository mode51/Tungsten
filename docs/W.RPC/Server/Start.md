Server.Start Method
===================
  Starts listening for client connections

  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public void Start(
	IPAddress ipAddress,
	int port
)
```

#### Parameters

##### *ipAddress*
Type: [System.Net.IPAddress][2]  

[Missing &lt;param name="ipAddress"/> documentation for "M:W.RPC.Server.Start(System.Net.IPAddress,System.Int32)"]


##### *port*
Type: [System.Int32][3]  

[Missing &lt;param name="port"/> documentation for "M:W.RPC.Server.Start(System.Net.IPAddress,System.Int32)"]


#### Implements
[IServer.Start(IPAddress, Int32)][4]  


Remarks
-------

This method will use reflection to inspect all loaded dlls for classes supporting the RPCClass and RPCMethod attributes


See Also
--------

#### Reference
[Server Class][5]  
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s128tyf6
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: ../../W.RPC.Interfaces/IServer/Start.md
[5]: README.md
[6]: ../../_icons/Help.png