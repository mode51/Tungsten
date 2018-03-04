 ### Notes:
* #### Tungsten.Suite is currently under development.  This page is likely to undergo regular changes.
* #### NuGet packages have not been published yet
___

The help files are built with [Sandcastle Help File Builder](https://github.com/EWSoftware/SHFB)

In itself, Tungsten.Suite is not an actual code project.  Instead it is a single nuspec file which contains references to NuGet packages from most of the Tungsten projects.

#### Projects in Tungsten.Suite
| Project                    | Description                                                  |
| -------------------------- | ------------------------------------------------------------ |
| Tungsten.ArrayMethods      | Provides additional functionality for arrays (Peek, Take, Trim, Append, Insert) |
| Tungsten.As                | A class which can be used as a return value to specify a success/fail, an exception if on occurred, and a result |
| Tungsten.CallResult        | A class which can be used as a return value to specify a success/fail, an exception if on occurred, and a result |
| Tungsten.Console           | Provides several string extension methods to send strings to the console |
| Tungsten.EventTemplate     | A class which makes exposing and raising an event somewhat easier |
| Tungsten.Encryption        | Provides several utility classes for RSA encryption and MD5 password hashes |
| Tungsten.From              | Complements Tungsten.As by providing FromBase64 and FromCompressed |
| Tungsten.IO.Pipes          | Client and server named pipes                                |
| Tungsten.Lockable          | A class which supports thread-safe access to an underlying value |
| Tungsten.Logging           | Provides basic support for logging.  Handle the W.Logging.Log.LogTheMessage event to provide additional logging functionality.  W.Logging.LogMessageHistory class can be instantiated to maintain an in-memory history of log messages. |
| Tungsten.Net | Client and server for Tcp and Udp with Generics support and assymetric encryption (public key/private key encryption with no symmetric key; secure transmission, but does not prevent man-in-the-middle attacks) |
| Tungsten.Property          | A class which implements IPropertyChangedNotifier.  PropertySlim is a lightweight version of Property and can be used in simpler scenarios (where ownership isn't necessary). |
| Tungsten.Threading         | Provides a Thread.Sleep method and ThreadMethod which makes creating a background thread (long running task) easy |
| Tungsten.Threading.Lockers | Provides classes which support resource locking (MonitorLocker, ReaderWriterLocker, SemaphoreLocker and SpinLocker) |

#### Projects not included in the Tungsten.Suite NuGet package
| Project               | Description                                                 |
| --------------------- | ----------------------------------------------------------- |
| Tungsten.Domains      | Easily implement reloadable AppDomains in your application  |
| Tungsten.Firewall     | Wraps NetFwTypeLib to add and remove Windows firewall rules |
| Tungsten.InterProcess | InterProcess communication via the WM_COPYDATA message      |

#### Concept projects
| Project          | Description                                      |
| ---------------- | ------------------------------------------------ |
| Tungsten.LiteDb  | Provides LiteDB based CRUD for your POCO classes |

___
#### Framework Compatibility
<sub>

| Project                    | net20 | net35 |       net45        | net461 |   netstandard1.0   |   netstandard1.3   |   netstandard1.4   | netstandard1.5 |   netstandard2.0   |
| -------------------------- | :---: | :---: | :----------------: | :----: | :----------------: | :----------------: | :----------------: | :------------: | :----------------: |
| Tungsten.ArrayMethods      |       |       | :heavy_check_mark: |        | :heavy_check_mark: |                    |                    |                |                    |
| Tungsten.As                |       |       | :heavy_check_mark: |        |                    | :heavy_check_mark: |                    |                |                    |
| Tungsten.CallResult        |       |       | :heavy_check_mark: |        | :heavy_check_mark: |                    |                    |                |                    |
| Tungsten.Console           |       |       | :heavy_check_mark: |        |                    | :heavy_check_mark: |                    |                |                    |
| Tungsten.EventTemplate     |       |       | :heavy_check_mark: |        | :heavy_check_mark: |                    |                    |                |                    |
| Tungsten.Encryption        |       |       | :heavy_check_mark: |        |                    | :heavy_check_mark: |                    |                |                    |
| Tungsten.From              |       |       | :heavy_check_mark: |        |                    | :heavy_check_mark: |                    |                |                    |
| Tungsten.IO.Pipes          |       |       | :heavy_check_mark: |        |                    |                    | :heavy_check_mark: |                |                    |
| Tungsten.Lockable          |       |       | :heavy_check_mark: |        | :heavy_check_mark: |                    |                    |                |                    |
| Tungsten.Logging           |       |       | :heavy_check_mark: |        | :heavy_check_mark: |                    |                    |                |                    |
| Tungsten.Net               |       |       | :heavy_check_mark: |        |                    | :heavy_check_mark: |                    |                | :heavy_check_mark: [<sup>1</sup>](#rpcNote) |
| Tungsten.Property          |       |       | :heavy_check_mark: |        | :heavy_check_mark: |                    |                    |                |                    |
| Tungsten.Threading         |       |       | :heavy_check_mark: |        | :heavy_check_mark: |                    |                    |                |                    |
| Tungsten.Threading.Lockers |       |       | :heavy_check_mark: |        | :heavy_check_mark: |                    |                    |                |                    |

<a name="rpcNote">1. </a>Due to limitations with System.Reflection, W.Net.RPC.Server is only available in net45 and netstandard2.0
<br>

| Project               |       net20        |       net35        |       net45        | net461 | netstandard1.0 | netstandard1.3 | netstandard1.4 | netstandard1.5 | netstandard2.0 |
| --------------------- | :----------------: | :----------------: | :----------------: | :----: | :------------: | :------------: | :------------: | :------------: | :------------: |
| Tungsten.Domains      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |        |                |                |                |                |                |
| Tungsten.Firewall     | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |        |                |                |                |                |                |
| Tungsten.InterProcess | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |        |                |                |                |                |                |

| Project          | net20 | net35 |       net45        |       net461       | netstandard1.0 |   netstandard1.3   |   netstandard1.4   | netstandard1.5 | netstandard2.0 |
| ---------------- | :---: | :---: | :----------------: | :----------------: | :------------: | :----------------: | :----------------: | :------------: | :------------: |
| Tungsten.LiteDb  |       |       | :heavy_check_mark: |                    |                |                    | :heavy_check_mark: |                |                |

</sub>
