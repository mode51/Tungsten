### Tungsten.Suite is currently under development.  This page is likely to change.

The help files are built with [Sandcastle Help File Builder](https://github.com/EWSoftware/SHFB)

| Package | Description |
|---------|-------------|
| Tungsten.ArrayMethods | Provides additional functionality for arrays (Peek, Take, Trim, Append, Insert)
| Tungsten.As | A class which can be used as a return value to specify a success/fail, an exception if on ocurred, and a result |
| Tungsten.CallResult | A class which can be used as a return value to specify a success/fail, an exception if on ocurred, and a result |
| Tungsten.Console | Provides several string extension methods to send strings to the console |
| Tungsten.EventTemplate | A class which makes exposing and raising an event somewhat easier |
| Tungsten.Encryption | Provides several utility classes for RSA encryption and MD5 password hashes |
| Tungsten.From | Complements Tungsten.As by providing FromBase64 and FromCompressed |
| Tungsten.IO.Pipes | Client and server named pipes |
| Tungsten.Lockable | A class which supports thread-safe access to an underlying value |
| Tungsten.Logging | Provides basic support for logging.  Handle the W.Logging.Log.LogTheMessage event to provide additional logging options.  W.Logging.LogMessageHistory class can be instantiated to maintain an in-memory history of messages. |
| Tungsten.Property | A class which implements IPropertyChangedNotifier.  PropertySlim is a lightweight version of Property and can be used in simpler scenarios (where ownership isn't necessary). |
| Tungsten.Threading | Provides a Thread.Sleep method and ThreadMethod which makes creating a background thread (long running task) easy |  |
| Tungsten.Threading.Lockers | Provides classes which support resource locking (MonitorLocker, ReaderWriterLocker, SemaphoreLocker and SpinLocker) |

#### Framework Compatability
<sub>

| Package | net20 | net35 | net45 | net461 | netstandard1.0 | netstandard1.3 | netstandard1.4 | netstandard1.5 |
|---------|-------|-------|-------|--------|----------------|----------------|----------------|----------------|
| Tungsten.ArrayMethods | | | * | | * | | | | |
| Tungsten.As | | | * | | | * | | |
| Tungsten.CallResult | | | * | | * | | | |
| Tungsten.Console | | | * | | | * | | |
| Tungsten.EventTemplate | | | * | | * | | | |
| Tungsten.Encryption | | | * | | | * | | |
| Tungsten.From | | | * | | | * | | |
| Tungsten.IO.Pipes | | | * | | | | * | |
| Tungsten.Lockable | | | * | | * | | | |
| Tungsten.Logging | | | * | | * | | | |
| Tungsten.Property | | | * | | * | | | |
| Tungsten.Threading | | | * | | * | | | |
| Tungsten.Threading.Lockers | | | * | | * | | | |

</sub>

## Projects not included in the Tungsten.Suite NuGet package
| Package | Description |
|---------|-------------|
| Tungsten.AppBar | Converts a Winforms form to an AppBar |
| Tungsten.Domains | Easily implement reloadable AppDomains in your application |
| Tungsten.Firewall | Wraps NetFwTypeLib to add and remove Windows firewall rules |
| Tungsten.InterProcess | InterProcess communication via the WM_COPYDATA message |

#### Framework Compatability
<sub>

| Package | net20 | net35 | net45 | net461 | netstandard1.0 | netstandard1.3 | netstandard1.4 | netstandard1.5 |
|---------|-------|-------|-------|--------|----------------|----------------|----------------|----------------|
| Tungsten.AppBar | * | * | * |  |  |  |  |  |
| Tungsten.Domains | * | * | * |  |  |  |  |  |
| Tungsten.Firewall | * | * | * |  |  |  |  |  |
| Tungsten.InterProcess | * | * | * |  |  |  |  |  |

</sub>
 
## Conceptual projects which may go away
| Package | Description |
|---------|-------------|
| Tungsten.LiteDb | Provides LiteDB based CRUD for your POCO classes |
| Tungsten.Theater | Provides an asynchronous execution pipeline |

#### Framework Compatability
<sub>

| Package | net20 | net35 | net45 | net461 | netstandard1.0 | netstandard1.3 | netstandard1.4 | netstandard1.5 |
|---------|-------|-------|-------|--------|----------------|----------------|----------------|----------------|
| Tungsten.LiteDb |||*||||*||
| Tungsten.Theater |||*|*||*|||

</sub>
