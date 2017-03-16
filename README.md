#Tungsten

Platforms: WinForms, WPF, Windows Universal, Windows Portable (PCL), NetStandard1.4

A C# library to make Windows Forms, WPF, Windows Universal and Windows Portable application development easier.  See the Wiki page for details and use.

Tungsten is built with .Net Framework 4.5
Tungsten.Universal is built on Windows 10 (10.0; Build 10240)

To build Tungsten.Documentation, you will need to install the SHFB Visual Studio extension from NuGet.

# Projects
* Tungsten - A collection of useful classes for Winforms/WPF
* Tungsten.Portable - A Portable version of Tungsten
* Tungsten.Universal - A Universal version of Tungsten
* Tungsten.Standard - A NetStandard version of Tungsten
* Tungsten.RPC - Encrypted socket classes (client and server) to run code on a Tungsten.RPC server
* Tungsten.Domains - Easily implement reloadable AppDomains in your application
* Tungsten.LiteDb - LiteDb based CRUD for your POCO classes
* Tungsten.IO.Pipes.Standard - A NetStandard wrapper for named pipes (client and server)

# Obsolete Projects
* Tungsten.Core - Obsolete - replaced by Tungsten.Standard
* Tungsten.Net.Core - Obsolete - replaced by Tungsten.Net.Standard

# Demos
* Tungsten.Demo.Winforms - Uses tasks, Property and Gate to provide a responsive UI while handling background tasks
* Tungsten.Demo.WPF - Illustrates how to use Tungsten in a WPF application
* Tungsten.RPC.ServerDemo - Illustrates how to create an RPC server with Tungsten.RPC
* Tungsten.RPC.ClientDemo - Illustrates how to create an RPC client with Tungsten.RPC
* Tungsten.RPC.Host.Demo - Hosts the RPC server in a reloadable AppDomain
* Tungsten.Domains.Demo - Illustrates using Tungsten.Domains to host a reloadable AppDomain
* Tungsten.Standard.Demo - Demonstrates using some of the features in Tungsten.Standard
* Tungsten.IO.Pipes.Standard.Demo - Illustrates using the named pipe client and server wrappers

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

#Tungsten.RPC Classes
* W.RPC.Server - An RPC server (see Tungsten.RPC.ServerDemo for use)
* W.RPC.Client - An RPC client (see Tungsten.RPC.ClientDemo for use)

#Tungsten.Domains
* W.Domains.DomainLoader - A handy class to make reloadable AppDomains easy

#Tungsten.LiteDb
* W.LiteDb.LiteDbItem - A base class for your POCO classes (necessary for LiteDbMethods due to needing to know the \_id field)
* W.LiteDb.LiteDbMethods - CRUD methods for your POCO classes which inherit LiteDbItem

#Tungsten.IO.Pipes
* W.IO.Pipes.PipeClient - A named pipe client; designed to be used with W.IO.Pipes.PipeServer
* W.IO.Pipes.PipeServer - A named pipe server; designed to be used with W.IO.Pipes.PipeClient
* W.IO.Pipes.FormattedPipeClient - the base class for PipeClient which handles connecting, disconnecting and cleanup
* W.IO.Pipes.PipeTransceiver - the base class of FormattedPipeClient which handles sending and receiving of data
