### Tungsten.Suite is currently under development.  This page is likely to change.

The help files are built with [Sandcastle Help File Builder](https://github.com/EWSoftware/SHFB)

## Projects

### Tungsten.ArrayMethods (net45, netstandard1.0)
* Provides additional functionality for arrays (Peek, Take, Trim, Append, Insert)

### Tungsten.CallResult (net45, netstandard1.0)
* A class which can be used as a return value to specify a success/fail, an exception if on ocurred, and a result

### Tungsten.EventTemplate (net45, netstandard1.0)
* A class which makes exposing and raising an event somewhat easier

### Tungsten.Lockable (net45, netstandard1.0)
* A class which supports thread-safe access to an underlying value

### Tungsten.Logging (net45, netstandard1.0)
* Provides basic support for logging.  Handle the W.Logging.Log.LogTheMessage event to provide additional logging options.  W.Logging.LogMessageHistory class can be instantiated to maintain an in-memory history of messages.

### Tungsten.Property (net45, netstandard1.0)
* A class which implements IPropertyChangedNotifier.  PropertySlim is a lightweight version of Property and can be used in simpler scenarios (where ownership isn't necessary).

### Tungsten.Threading (net45, netstandard1.0)
* Provides a Thread.Sleep method and ThreadMethod which makes creating a background thread (long running task) easy

### Tungsten.Threading.Lockers (net45, netstandard1.0)

### Tungsten.As (net45, netstandard1.3)
* Extension methods for quick conversions (As<T>, AsBase64, AsBytes, AsCompressed, AsString)

### Tungsten.Console (net45, netstandard1.3)
* Provides several string extension methods to send strings to the console

### Tungsten.Encryption (net45, netstandard1.3)
* Provides several utility classes for RSA encryption and MD5 password hashes

### Tungsten.From (net45, netstandard1.3)
* Complements Tungsten.As by providing FromBase64 and FromCompressed

### Tungsten.IO.Pipes (net45, netstandard1.4)
* Client and server named pipes

### Tungsten.Net (net45, netstandard1.5
* Client/server tcp/udp socket library, supports Generics, unsecured or with RSA encryption

## The following projects are separate projects and are not included in the Tungsten.Suite NuGet package
### Tungsten.AppBar (net45)
* Converts a Winforms form to an AppBar

### Tungsten.Domains (net20, net35, net45)
* Easily implement reloadable AppDomains in your application

### Tungsten.Firewall (net20, net35, net45)
* Wraps NetFwTypeLib to add and remove Windows firewall rules

### Tungsten.InterProcess (net20, net35, net45)
* InterProcess communication via the WM_COPYDATA message

## The following projects are conceptual and may go away
### Tungsten.LiteDb (net45, netstandard1.4)
* Provides LiteDB based CRUD for your POCO classes
### Tungsten.Theater (net45, net461, netstandard1.3)
* Provides an asynchronous execution pipeline
