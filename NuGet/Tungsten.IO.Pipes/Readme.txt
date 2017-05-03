Named pipe wrappers to easily add asynchronous named pipes to your application.  Server and client are provided.

5.3.2017 - v1.2.5
Added static Write(string pipeName, byte[] message) method to W.IO.Pipes.FormattedClient and W.IO.Pipes.PipeClient
Fixed a bug where the pipe server would stop listening for messages

5.2.2017 - v1.2.4
Now using Newtonsoft for serialization instead of basic xml (this will be more robust out of the box)
Updated Tungsten reference to v1.2.4
Renamed output dll to just Tungsten.IO.Pipes

3.15.2017 - v1.2.0
Initial Release (replaces Tungsten.IO.Pipes.Standard)

Thanks for using Tungsten.IP.Pipes,

Jordan Duerksen
