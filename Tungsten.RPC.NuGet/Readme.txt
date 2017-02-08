Tungsten.RPC is a socket library which allows clients to execute functionality (with or without return values) on a server.  Tungsten.RPC uses two-way encryption via public/private key pairs to secure the transmission.

Dependencies:
Tungsten (NuGet)
Newtonsoft.Json (NuGet)


2.8.2017
Updated Tungsten.RPC.Demo with the necessary changes
Added IClient and ISocketClient interfaces to Tungsten.RPC.Interfaces
Moved delegates and constants into Tungsten.RPC.Interfaces
Tungsten.RPC no longer exposes CallResult as a return value.  Instead Client.Connect now returns true/false and exceptions are handled via a callback.
Tungsten.RPC now references Tungsten.RPC.Interfaces.dll

1.27.2017
Rebuilt with Tungsten 1.0.8

1.21.2017 - Initial Release


Thanks for using Tungsten.RPC

Jordan Duerksen