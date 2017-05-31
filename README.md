# Projects

### Tungsten
A C# library to make Windows Forms, WPF, Windows Universal and Windows Portable application development easier.  See the Wiki page for details and use.

### Tungsten ###
* **Tungsten** - A collection of useful classes for Winforms/WPF (some classes are listed below)
  * Targets .Net Framework 4.5
* **Tungsten.Portable** - A Portable version of Tungsten
  * Targets portable-net45+win+wpa81+MonoAndroid10+MonoTouch10+xamarinios10+xamarinmac20
* **Tungsten.Universal** - A Universal version of Tungsten
  * Targets Windows 10 (10.0; Build 10240)
* **Tungsten.Standard** - A NetStandard version of Tungsten
  * Targets .NetStandard 1.4

### Tungsten.Net
* Tungsten.Net - A client/server tcp socket library
* Tungsten.Net.Standard - A NetStandard version of Tungsten.Net

### Tungsten.Net.RPC
* Tungsten.Net.RPC - Encrypted socket classes (client and server) to run code on a Tungsten.RPC server
* Tungsten.Net.RPC.Standard - .NetStandard version of Tungsten.Net.RPC

### Tungsten.Domains
* Tungsten.Domains - Easily implement reloadable AppDomains in your application

### Tungsten.LiteDb
* Tungsten.LiteDb - LiteDb based CRUD for your POCO classes

### Tungsten.Firewall
* Tungsten.Firewall - A minimal library, using NetFwTypeLib, to add and remove Windows firewall rules

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

# Overview Of Classes By Project

### Tungsten (Tungsten, Portable, Universal, Standard)
* [W.Lockable](https://github.com/mode51/Tungsten/wiki/Lockable) - A generic class to wrap a value in a thread-safe manner
* [W.Property](https://github.com/mode51/Tungsten/wiki/Property) - A generic class providing a number of enhancements to a [W.Lockable](https://github.com/mode51/Tungsten/wiki/Lockable) value
    * Implements INotifyPropertyChanged
    * Adds IsDirty/MarkAsClean functionality
    * Supports a callback handler in the constructor so you don't have to use events
* [W.PropertyHost](https://github.com/mode51/Tungsten/wiki/PropertyHost) - a base class which automates the IsDirty, MarkAsClean and InitializeProperties so you don't have to.
* [W.PropertyChangedNotifier](https://github.com/mode51/Tungsten/wiki/PropertyChangedNotifier) - provides implementation of INotifyPropertyChanged in a base-class with overridable GetValue and SetValue methods
* [W.PropertyHostNotifier] - aggregates PropertyHost and PropertyChangedNotifier
* [W.InvokeExtensions](https://github.com/mode51/Tungsten/wiki/InvokeExtensions) - A static class exposing InvokeEx extension methods (to ease InvokeRequired handling)
* [W.CallResult](https://github.com/mode51/Tungsten/wiki/CallResult) - A non-generic class which can be used to return true/false and an exception from a function
* W.Threading.Thread - automates creating a thread with an Action
* W.Threading.Thread\<T\> - like Thread, except Action is now Action\<T\>
* W.Threading.Gate - similar to Thread, a Gate can be started some time after creation
* W.Threading.Gate\<T\> - like Gate, exception Action is now Action\<T\>
* W.ActionQueue\<T\> - Merges a Thread with a ConcurrentQueue.  Executes the provided Action\<T\> or Func\<T, bool\>, on a background thread, whenever an item is added to the ConcurrentQueue.

### Tungsten.Net
#### Clients
Clients are designed to be used with their server counterpart (Client/Server and SecureClient/SecureServer)
* W.Net.Client - a non-secure client which sends and receives byte arrays
```
    var client = new W.Net.Client();
```
* W.Net.Client<TMessageType> - a non-secure generic client which sends and receives messages of any type
```
    var client = new W.Net.Client<string>();
```
* W.Net.SecureClient<TMessageType> - like, Client<TMessageType>, except the messages are encrypted
```
    var client = new W.Net.SecureClient<string>();
```

#### Servers
* W.Net.Server<TClientType> - a server which hosts non-secure client connections
```
    var server = new W.Net.Server<W.Net.Client<string>>();
    //or
    public class MyMessage
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
    }
    var messageServer = new W.Net.Server<W.Net.Client<MyMessage>>();
```

* W.Net.SecureServer<TClientType> - a server which hosts encrypted clients (SecureClient)
```
    public class MyMessage
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
    }
    var server = new W.Net.SecureServer<W.Net.SecureClient<MyMessage>>();
```

#### Utility Classes (not really meant to be used directly)
* W.Net.Sockets.Socket - provides root functionality of reading from and writing to a NetworkStream
* W.Net.StringClientLogger - when instantiated, W.Logging.Log.X calls will also log via Client<string>
* W.Net.SecureStringClientLogger - when instantiated, W.Logging.Log.X calls will also log via SecureClient<string>
* W.Net.TcpHelpers - some static helper methods used by Tungsten.Net
* W.Net.TcpClientReader - reads data from a NetworkStream
* W.Net.TcpClientWriter - writes data to a NetworkStream

### Tungsten.Net.RPC
The server must be running in an application which contains classes and methods which support the [RPCClass] and [RPCMethod] attributes.  The dll must be in-memory for the server to find the class methods.  Remote clients can then call these methods.  See the Tungsten.Net.RPC.Standard.Demo for more detailed use.
* W.Net.RPC.Server - Allows remote applications to call local methods
* W.Net.RPC.Client - Call methods exposed by a Tungsten.Net.RPC.Server

### Tungsten.Domains
* W.Domains.DomainLoader - A handy class to make reloadable AppDomains easy

### Tungsten.IO.Pipes
* W.IO.Pipes.PipeClient - A named pipe client; designed to be used with W.IO.Pipes.PipeServer
* W.IO.Pipes.PipeServer - A named pipe server; designed to be used with W.IO.Pipes.PipeClient
* W.IO.Pipes.FormattedPipeClient - the base class for PipeClient which handles connecting, disconnecting and cleanup
* W.IO.Pipes.PipeTransceiver - the base class of FormattedPipeClient which handles sending and receiving of data

### Tungsten.LiteDb
* W.LiteDb.LiteDbItem - A base class for your POCO classes (necessary for LiteDbMethods due to needing to know the \_id field)
* W.LiteDb.LiteDbMethods - CRUD methods for your POCO classes which inherit LiteDbItem

### Tungsten.Firewall
* W.Firewall - Provides static methods to add, remove and check the existance of, Windows firewall rules
