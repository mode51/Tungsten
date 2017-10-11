Tungsten.Net is a client/server tcp socket library. Client and server classes are provided, with and without assymetric encryption.

9.5.2017 v1.3.0
Rewrite to stabalize SecureClient
Added CPUProfile to allow the programmer to customize how the cpu is utilized (mostly only useful for servers)
Now using ref and out to minimize memory usage
Added SendAndWaitForResponse and SendRaw/SendRawAndWaitForResponse

5.30.2017 v1.2.5.3
This update largely breaks backward compatability.  I'm hoping this is the last major change.
Refactored, simplified and corrected clients, servers and interfaces.
Renamed MessageSent multi-cast delegate to DataSent
Removed SecureClient<TClientType>.MessageSent multi-cast delegate as tracking this would be too much overhead for it's worth.  DataSent still exists.
Removed StringClient, it's related server, and the secure versions thereof because they can be declared easily with existing classes.
Fixed a bug with server-side clients
Added SerializationSettings which exposes the JsonSerializationSettings for Newtonsoft.Json

5.11.2017 v1.2.5
Near rewrite of clients and servers (secure clients and servers should be more stable now)
Added StringClientLogger and SecureStringClientLogger to make it easy to add logging over TCP
GenericClient is now Client (and SecureClient) and Client is now the base of all other clients
SecureServer now inherits Server (instead of duplicating the same functionality)
Removed W.Net.Sockets.FormattedClient - this is now built into W.Net.Client

5.2.2017 v1.2.4.1
Readded a constructor to W.Net.SecureStringClient and W.Net.GenericClient

5.2.2017 v1.2.4
All target dlls are now named Tungsten.Net.dll - this should help with cross-framework compatibility
Updated Tungsten reference to v1.2.4

4.28.2017 - v1.2.3.4
W.Net.Sockets.Socket.Name will now be the remoteAddress + ":" + remotePort (ie: "127.0.0.1:5150")
W.Net.Sockets.FormattedSocket has been renamed to FormattedClient
W.Net.Sockets.FormattedClient.Connected now exposes the IPEndPoint instead of the IPAddress
W.Net.Sockets.FormattedClient.Disconnected now also exposes the IPEndPoint

4.12.2017 - v1.2.3.2
Changed netstandard target from 1.4 to 1.3

4.6.2017 - v1.2.3
Now references Newtonsoft
GenericClient now uses Newtonsoft for serialization

3.30.2017 - v1.2.2
Both Tungsten.Net and Tungsten.Net.Standard now share the same code-base
Updated Tungsten reference to 1.2.2

3.15.2017 - v1.2.0
Initial Release (replaces Tungsten.Net.Core)

Thanks for using Tungsten.Net,

Jordan Duerksen
