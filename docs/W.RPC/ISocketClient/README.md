ISocketClient Interface
=======================
  
[Missing &lt;summary> documentation for "T:W.RPC.ISocketClient"]


  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC.Interfaces (in Tungsten.RPC.Interfaces.dll)

Syntax
------

```csharp
public interface ISocketClient
```

The **ISocketClient** type exposes the following members.


Properties
----------

                   | Name             | Description 
------------------ | ---------------- | ----------- 
![Public property] | [IsConnected][2] |             


Methods
-------

                 | Name                                                                                             | Description 
---------------- | ------------------------------------------------------------------------------------------------ | ----------- 
![Public method] | [Connect(IPAddress, Int32, Int32, Action&lt;ISocketClient, IPAddress>, Action&lt;Exception>)][3] |             
![Public method] | [Connect(String, Int32, Int32, Action&lt;ISocketClient, IPAddress>, Action&lt;Exception>)][4]    |             
![Public method] | [ConnectAsync(IPAddress, Int32, Int32)][5]                                                       |             
![Public method] | [ConnectAsync(String, Int32, Int32)][6]                                                          |             
![Public method] | [Disconnect][7]                                                                                  |             


Events
------

                | Name              | Description 
--------------- | ----------------- | ----------- 
![Public event] | [Connected][8]    |             
![Public event] | [Disconnected][9] |             


See Also
--------

#### Reference
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: IsConnected.md
[3]: Connect.md
[4]: Connect_1.md
[5]: ConnectAsync.md
[6]: ConnectAsync_1.md
[7]: Disconnect.md
[8]: Connected.md
[9]: Disconnected.md
[10]: ../../_icons/Help.png
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public event]: ../../_icons/pubevent.gif "Public event"