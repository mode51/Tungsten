Client.DisconnectedDelegate Delegate
====================================
  Delegate to notify the programmer when the Client has disconnected from the Server

  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public delegate void DisconnectedDelegate(
	Client client,
	Exception exception
)
```

#### Parameters

##### *client*
Type: [W.RPC.Client][2]  
A reference to the Client which has disconnected

##### *exception*
Type: [System.Exception][3]  
Specifies the exception which caused the disconnection. If no exception ocurred, this value is null.


See Also
--------

#### Reference
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: ../Client/README.md
[3]: http://msdn.microsoft.com/en-us/library/c18k6c59
[4]: ../../_icons/Help.png