Tungsten.Net.RPC is a socket library which allows clients to execute functionality (with or without return values) on a server (the server must be a Tungsten.Net.RPC server). Tungsten.Net.RPC uses two-way encryption via public/private key pairs to secure the transmission.

4.17.2017 - v1.2.4
Added more overrides to W.Net.RPC.Client.Call
Added W.Net.RPC.Client.CallAsync asynchronous methods
Default timeout for RPC calls is now 60 seconds, but can be modified through the new W.Net.RPC.Client.SetMaxTimeout method
Fixed several bugs in Tungsten.Net.RPC.Waiter.cs

4.11.2017 - v1.2.3
Initial release under new namespace (replaces Tungsten.Net.Core)

Thanks for using Tungsten.Net.RPC,

Jordan Duerksen
