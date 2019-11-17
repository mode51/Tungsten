 ### Notes:
* #### 11/16/2019 - Added Tungsten.SolitaryApplication
* #### The first release of Tungsten.Suite (version 2.0 of the Tungsten library) is complete
* #### All NuGet packages have been published
___

The help files are built with [Sandcastle Help File Builder](https://github.com/EWSoftware/SHFB)

Tungsten.Suite is not an actual code project.  Instead, it is a single nuspec file which contains references to each of the Tungsten NuGet packages according to their target framework. It's the easy way to reference all the Tungsten packages at once.  Of course, you can always reference each package individually as desired.

#### Projects in Tungsten.Suite
| Project                    | Namespace | Description                                                  |
| -------------------------- | --------- | ------------------------------------------------------------ |
| Tungsten.ArrayMethods      | W | Provides additional functionality for arrays (Peek, Take, Trim, Append, Insert) |
| Tungsten.As                | W | Several conversion extension methods AsBytes, AsString, AsCompressed, AsBase64 |
| Tungsten.CallResult        | W | A class which can be used as a return value to specify a success/fail, an exception if on occurred, and a result |
| Tungsten.Console           | W | Provides several string extension methods to send strings to the console |
| Tungsten.EventTemplate     | W | A class which makes exposing and raising an event somewhat easier |
| Tungsten.Encryption        | W.Encryption | Provides several utility classes for RSA encryption and MD5 password hashes |
| Tungsten.From              | W | Complements Tungsten.As by providing FromBase64 and FromCompressed |
| Tungsten.IO.Pipes          | W.IO.Pipes | Client and server named pipes                                |
| Tungsten.Lockable          | W | A class which supports thread-safe access to an underlying value |
| Tungsten.Logging           | W.Logging | Provides basic support for logging.  Handle the W.Logging.Log.LogTheMessage event to provide additional logging functionality.  W.Logging.LogMessageHistory class can be instantiated to maintain an in-memory history of log messages. |
| Tungsten.Net | W.Net, W.Net.RPC | Client and server for Tcp and Udp with Generics support and assymetric encryption (public key/private key encryption with no symmetric key; secure transmission, but does not prevent man-in-the-middle attacks) |
| Tungsten.Property          | W | A class which implements IPropertyChangedNotifier.  PropertySlim is a lightweight version of Property and can be used in simpler scenarios (where ownership isn't necessary). |
| Tungsten.Threading         | W.Threading | Provides a Thread.Sleep method and ThreadMethod which makes creating a background thread (long running task) easy |
| Tungsten.Threading.Lockers | W.Threading.Lockers | Provides classes which support resource locking (MonitorLocker, ReaderWriterLocker, SemaphoreLocker and SpinLocker) |

#### Projects not included in the Tungsten.Suite NuGet package
| Project               | Namespace | Description                                                 |
| --------------------- | --------- | ----------------------------------------------------------- |
| Tungsten.Domains      | W.Domains | Easily implement reloadable AppDomains in your application  |
| Tungsten.Firewall     | W.Firewall | Wraps NetFwTypeLib to add and remove Windows firewall rules |
| Tungsten.InterProcess | W.InterProcess | InterProcess communication via the WM_COPYDATA message      |
| Tungsten.SolitaryApplication | W | A helper class which makes it easy to disallow concurrent application instances |

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
| Tungsten.Net               |       |       | :heavy_check_mark: |        |                    | :heavy_check_mark: [<sup>1</sup>](#rpcNote1) [<sup>2</sup>](#rpcNote2) |                    |                | :heavy_check_mark: [<sup>1</sup>](#rpcNote1) [<sup>2</sup>](#rpcNote2) |
| Tungsten.Property          |       |       | :heavy_check_mark: |        | :heavy_check_mark: |                    |                    |                |                    |
| Tungsten.Threading         |       |       | :heavy_check_mark: |        | :heavy_check_mark: |                    |                    |                |                    |
| Tungsten.Threading.Lockers |       |       | :heavy_check_mark: |        | :heavy_check_mark: |                    |                    |                |                    |

<p><a name="rpcNote1"></a>1. Due to limitations with System.Reflection, W.Net.RPC.Server is only available in net45 and netstandard2.0</p>
<p><a name="rpcNote2"></a>2. Due to the way Newtonsoft.Json deserializes integers, do NOT use int (Int32) in your api's as parameters or return types. Use long instead.</p>
<br>

| Project               |       net20        |       net35        |       net45        | net461 | netstandard1.0 | netstandard1.3 | netstandard1.4 | netstandard1.5 | netstandard2.0 |
| --------------------- | :----------------: | :----------------: | :----------------: | :----: | :------------: | :------------: | :------------: | :------------: | :------------: |
| Tungsten.Domains      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |        |                |                |                |                |                |
| Tungsten.Firewall     | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |        |                |                |                |                |                |
| Tungsten.InterProcess | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |        |                |                |                |                |                |
| Tungsten.SolitaryApplication | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |        |                |                |                |                | :heavy_check_mark: |


</sub>
