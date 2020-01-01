 ### Notes:
* #### 1/1/2020 - Initial checkin for v3.0.0
* #### The first commit of Tungsten as a single package(version 3.0 of the Tungsten library)
* #### Tungsten NuGet package has NOT been published
___

#### Key features in Tungsten.Suite
|    Feature    | Description                                                  |
| ------------- | ------------------------------------------------------------ |
| W.ArrayMethods | Provides additional functionality for arrays (Peek, Take, Trim, Append, Insert) |
| W.As | Several conversion extension methods AsBytes, AsString, AsCompressed, AsBase64 |
| W.CallResult | A class which can be used as a return value to specify a success/fail, an exception if on occurred, and a result |
| W.Domains | |
| W.EventTemplate | A class which makes exposing and raising an event somewhat easier |
| W.Encryption | Provides several utility classes for RSA encryption and MD5 password hashes |
| W.Firewall | Static methods to add and remove firewall rules |
| W.From | Complements W.As by providing FromBase64 and FromCompressed |
| W.IO.Pipes | Client and server named pipes                                |
| W.InterProcess | Uses WM_COPYDATA to send interprocess messages |
| W.Lockable | A class which supports thread-safe access to an underlying value |
| W.Logging | Provides basic support for logging.  Handle the W.Logging.Log.LogTheMessage event to provide additional logging functionality.  W.Logging.LogMessageHistory class can be instantiated to maintain an in-memory history of log messages. |
| W.Net, W.Net.RPC | Client and server for Tcp and Udp with Generics support and assymetric encryption (public key/private key encryption with no symmetric key; secure transmission, but does not prevent man-in-the-middle attacks) |
| W.Property | A class which implements IPropertyChangedNotifier.  PropertySlim is a lightweight version of Property and can be used in simpler scenarios (where ownership isn't necessary). |
| W.SolitaryApplication | Makes it easy to make an application support only one running instance |
| W.Threading | Provides a Thread.Sleep method and ThreadMethod which makes creating a background thread (long running task) easy |
| W.Threading.Lockers | Provides classes which support resource locking (MonitorLocker, ReaderWriterLocker, SemaphoreLocker and SpinLocker) |

___
#### Framework Compatibility
<sub>

|        Feature        |       net45        |   netstandard1.0   |   netstandard1.3   |   netstandard1.4   |   netstandard2.0   |
| --------------------- | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: |
| W.ArrayMethods        | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
| W.As                  | :heavy_check_mark: |                    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
| W.CallResult          | :heavy_check_mark: | :heavy_check_mark: |                    | :heavy_check_mark: | :heavy_check_mark: |
| W.Domains             | :heavy_check_mark: |                    |                    |                    |                    |
| W.EventTemplate       | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
| W.Encryption          | :heavy_check_mark: |                    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
| W.Firewall            | :heavy_check_mark: |                    |                    |                    |                    |
| W.From                | :heavy_check_mark: |                    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
| W.IO.Pipes            | :heavy_check_mark: |                    |                    | :heavy_check_mark: | :heavy_check_mark: |
| W.InterProcess        | :heavy_check_mark: |                    |                    |                    |                    |
| W.Lockable            | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
| W.Logging             | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
| W.Net                 | :heavy_check_mark: [<sup>2</sup>](#rpcNote2) |                    | :heavy_check_mark: [<sup>1</sup>](#rpcNote1) [<sup>2</sup>](#rpcNote2) | :heavy_check_mark: [<sup>1</sup>](#rpcNote1) [<sup>2</sup>](#rpcNote2) | :heavy_check_mark: [<sup>1</sup>](#rpcNote1) [<sup>2</sup>](#rpcNote2) |
| W.Property            | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
| W.SolitaryApplication | :heavy_check_mark: |                    |                    |                    | :heavy_check_mark: | 
| W.Threading           | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
| W.Threading.Lockers   | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |

<p><a name="rpcNote1"></a>1. Due to limitations with System.Reflection, W.Net.RPC.Server is only available in net45 and netstandard2.0</p>
<p><a name="rpcNote2"></a>2. Due to the way Newtonsoft.Json deserializes integers, do NOT use int (Int32) in your api's as parameters or return types. Use long instead.</p>
<br>

</sub>
