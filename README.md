# Tungsten
A C# library to make Windows Forms, WPF, Windows Universal and Windows Portable application development easier.  See the Wiki page for details and use.

#### Tungsten targets .Net Framework 4.5
#### Tungsten.Portable targets portable-net45+win+wpa81+MonoAndroid10+MonoTouch10+xamarinios10+xamarinmac20
#### Tungsten.Universal targets Windows 10 (10.0; Build 10240)
#### Tungsten.Standard targets .NetStandard 1.4

* Tungsten - A collection of useful classes for Winforms/WPF (some classes are listed below)
* Tungsten.Portable - A Portable version of Tungsten
* Tungsten.Universal - A Universal version of Tungsten
* Tungsten.Standard - A NetStandard version of Tungsten

# Tungsten.Net
* Tungsten.Net - A client/server tcp socket library
* Tungsten.Net.Standard - A NetStandard version of Tungsten.Net

# Tungsten.Net.RPC
* Tungsten.Net.RPC - Encrypted socket classes (client and server) to run code on a Tungsten.RPC server
* Tungsten.Net.RPC.Standard - .NetStandard version of Tungsten.Net.RPC

# Tungsten.Domains
* Tungsten.Domains - Easily implement reloadable AppDomains in your application

# Tungsten.LiteDb
* Tungsten.LiteDb - LiteDb based CRUD for your POCO classes

# Demos
* Tungsten.IO.Pipes.Standard - A NetStandard wrapper for named pipes (client and server)
* Tungsten.Demo.Winforms - Uses tasks, Property and Gate to provide a responsive UI while handling background tasks
* Tungsten.Demo.WPF - Illustrates how to use Tungsten in a WPF application
* Tungsten.Standard.Demo - Demonstrates using some of the features in Tungsten.Standard
* Tungsten.Domains.Demo - Illustrates using Tungsten.Domains to host a reloadable AppDomain
* Tungsten.Domains.Plugin.Demo - A sample plugin
* Tungsten.Domains.Plugin.Interface.Demo - The interface used by the plugin demo
* Tungsten.IO.Pipes.Standard.Demo - Illustrates using the named pipe client and server wrappers
* Tungsten.Net.Demo - Illustrates how to create a tcp server and connect clients
* Tungsten.Net.RPC.Standard.Demo - a client/server RPC demo

# Tungsten (Tungsten, Portable, Universal, Standard)
* [W.Lockable](https://github.com/mode51/Tungsten/wiki/Lockable-TValue)\<TValue\> - A generic class to wrap a value in a thread-safe manner
* [W.Property](https://github.com/mode51/Tungsten/wiki/Property-TValue)\<TValue\> - A generic class providing a number of enhancements to a [W.Lockable](https://github.com/mode51/Tungsten/wiki/Lockable-TValue-)\<TValue\> value
    * Implements INotifyPropertyChanged
    * Adds IsDirty/MarkAsClean functionality
    * Supports a callback handler in the constructor so you don't have to use events
* [W.Property](https://github.com/mode51/Tungsten/wiki/Property-TOwner,-TValue)\<TOwner, TValue\> - Like Property\<TValue\> except that you can specify an Owner
* Events and callback have type-strict sender (which is the Owner you specify)
* [W.PropertyHost](https://github.com/mode51/Tungsten/wiki/PropertyHost) - a base class which automates the IsDirty, MarkAsClean and InitializeProperties so you don't have to.
* [W.PropertyChangedNotifier](https://github.com/mode51/Tungsten/wiki/PropertyChangedNotifier) - provides implementation of INotifyPropertyChanged in a base-class with overridable GetValue and SetValue methods
* [W.PropertyHostNotifier] - aggregates PropertyHost and PropertyChangedNotifier
* [W.InvokeExtensions](https://github.com/mode51/Tungsten/wiki/InvokeExtensions) - A static class exposing InvokeEx extension methods (to ease InvokeRequired handling)
* [W.CallResult](https://github.com/mode51/Tungsten/wiki/CallResult) - A non-generic class which can be used to return true/false and an exception from a function
* [W.CallResult\<TResult\>](https://github.com/mode51/Tungsten/wiki/CallResult-TResult) - Like CallResult except that you can also specify a result
* W.Threading.Thread - automates creating a thread with an Action
* W.Threading.Thread\<T\> - like Thread, except Action is now Action\<T\>
* W.Threading.Gate - similar to Thread, a Gate can be started some time after creation
* W.Threading.Gate\<T\> - like Gate, exception Action is now Action\<T\>
* W.ActionQueue\<T\> - Merges a Thread with a ConcurrentQueue.  Executes the provided Action\<T\> or Func\<T, bool\> whenever an item is added to the ConcurrentQueue

# Tungsten.Net
These two classes are designed to be the primary implementations
* W.Net.GenericServer - a generic TCP server hosts secure client connections
* W.Net.GenericClient - a secure generic TCP client to be used specifically with W.Net.GenericServer; sends and receives messages of any type

These classes can be used if you don't need to transmit objects
* W.Net.SecureStringServer - a secure server which hosts W.Net.SecureStringClient connections
* W.Net.SecureStringClient - a secure client which 
* W.Net.StringServer - a non-secure server which hosts non-secure W.Net.StringClient connections
* W.Net.StringClient - a non-secure string client to be used specifically with W.Net.StringServer; sends and receives string messages

The following classes can be used to create more customized implementations
* W.Net.Sockets.SecureServer - can be used with customized secure client connections
* W.Net.Sockets.Server - a non-secure server; can be used with customized W.Net.Socket.FormattedSocket clients
* W.Net.Sockets.FormattedSocket - can be used to create a custom client with a custom data type (as long as you can convert it to and from a byte array)
* W.Net.Sockets.Socket - can be used to create a custom client which transmits byte arrays

# Tungsten.Net.RPC
* W.Net.RPC.Server - An RPC server (see Tungsten.Net.RPC.Standard.Demo for use)
* W.Net.RPC.Client - An RPC client (see Tungsten.Net.RPC.Standard.Demo for use)

# Tungsten.Domains
* W.Domains.DomainLoader - A handy class to make reloadable AppDomains easy

# Tungsten.LiteDb
* W.LiteDb.LiteDbItem - A base class for your POCO classes (necessary for LiteDbMethods due to needing to know the \_id field)
* W.LiteDb.LiteDbMethods - CRUD methods for your POCO classes which inherit LiteDbItem

# Tungsten.IO.Pipes
* W.IO.Pipes.PipeClient - A named pipe client; designed to be used with W.IO.Pipes.PipeServer
* W.IO.Pipes.PipeServer - A named pipe server; designed to be used with W.IO.Pipes.PipeClient
* W.IO.Pipes.FormattedPipeClient - the base class for PipeClient which handles connecting, disconnecting and cleanup
* W.IO.Pipes.PipeTransceiver - the base class of FormattedPipeClient which handles sending and receiving of data
