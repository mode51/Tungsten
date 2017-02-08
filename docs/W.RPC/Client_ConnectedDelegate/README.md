Client.ConnectedDelegate Delegate
=================================
  Delegate to notify the programmer when the Client has connected to the Server

  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public delegate void ConnectedDelegate(
	Client client,
	IPAddress remoteAddress
)
```

#### Parameters

##### *client*
Type: [W.RPC.Client][2]  
A reference to the Client which has connected

##### *remoteAddress*
Type: [System.Net.IPAddress][3]  
The IP Address of the Server


See Also
--------

#### Reference
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: ../Client/README.md
[3]: http://msdn.microsoft.com/en-us/library/s128tyf6
[4]: ../../_icons/Help.png