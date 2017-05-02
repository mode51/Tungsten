Tungsten.Net is a client/server tcp socket library. Client and server classes are provided, with and without assymetric encryption.

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
