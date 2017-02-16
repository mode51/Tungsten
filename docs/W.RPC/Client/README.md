Client Class
============
  Provides simple access to a Tungsten RPC Server


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.RPC.Client**  

  **Namespace:**  [W.RPC][2]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public class Client : IDisposable, IClient, 
	ISocketClient
```

The **Client** type exposes the following members.


Constructors
------------

                 | Name                                 | Description                                                                                    
---------------- | ------------------------------------ | ---------------------------------------------------------------------------------------------- 
![Public method] | [Client()][3]                        | Constructs a new Tungsten RPC Client                                                           
![Public method] | [Client(IPAddress, Int32, Int32)][4] | Constructs a new Tungsten RPC Client and automatically connects to the specified remote server 
![Public method] | [Client(String, Int32, Int32)][5]    | Constructs a new Tungsten RPC Client and automatically connects to the specified remote server 


Properties
----------

                   | Name             | Description                                                                         
------------------ | ---------------- | ----------------------------------------------------------------------------------- 
![Public property] | [IsConnected][6] | True if the Client is currently connected to a Tungsten RPC Server, otherwise False 


Methods
-------

                    | Name                                                                                             | Description                                                                                                                                                   
------------------- | ------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Connect(IPAddress, Int32, Int32, Action&lt;ISocketClient, IPAddress>, Action&lt;Exception>)][7] | Attempts to connect to a local or remote Tungsten RPC Server                                                                                                  
![Public method]    | [Connect(String, Int32, Int32, Action&lt;ISocketClient, IPAddress>, Action&lt;Exception>)][8]    | Attempts to connect to a local or remote Tungsten RPC Server                                                                                                  
![Public method]    | [ConnectAsync(IPAddress, Int32, Int32)][9]                                                       | Attempts to connect to a local or remote Tungsten RPC Server asynchronously                                                                                   
![Public method]    | [ConnectAsync(String, Int32, Int32)][10]                                                         | Attempts to connect to a local or remote Tungsten RPC Server asynchronously                                                                                   
![Public method]    | [Disconnect][11]                                                                                 | Disconnects from the Server                                                                                                                                   
![Public method]    | [Dispose][12]                                                                                    | Disconnects from a Tungsten RPC Server if connected. Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. 
![Protected method] | [Finalize][13]                                                                                   | Destructs the Tungsten RPC Client. Calls Dispose. (Overrides [Object.Finalize()][14].)                                                                        
![Public method]    | [MakeRPCCall(String, Action, Object[])][15]                                                      | Not sure I should keep this method. Shouldn't all RPC calls have a result? Otherwise, the client wouldn't know if it succeeded.                               
![Public method]    | [MakeRPCCall&lt;T>(String, Action&lt;T>)][16]                                                    | Calls a method on the Tungsten RPC Server                                                                                                                     
![Public method]    | [MakeRPCCall&lt;T>(String, Action&lt;T>, Object[])][17]                                          | Calls a method on the Tungsten RPC Server                                                                                                                     


Events
------

                | Name               | Description                                             
--------------- | ------------------ | ------------------------------------------------------- 
![Public event] | [Connected][18]    | Raised when the Client has connected to the Server      
![Public event] | [Disconnected][19] | Raised when the Client has disconnected from the Server 


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][20]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][21].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][22]     | Serializes an object to a Json string (Defined by [AsExtensions][21].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][23]   | Starts a new thread (Defined by [ThreadExtensions][24].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][25] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][26].) 
![Public Extension Method]                | [IsDirty][27]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][26].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][28]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][26].)                                                                                                  


See Also
--------

#### Reference
[W.RPC Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: _ctor_1.md
[5]: _ctor_2.md
[6]: IsConnected.md
[7]: Connect.md
[8]: Connect_1.md
[9]: ConnectAsync.md
[10]: ConnectAsync_1.md
[11]: Disconnect.md
[12]: Dispose.md
[13]: Finalize.md
[14]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[15]: MakeRPCCall.md
[16]: MakeRPCCall__1.md
[17]: MakeRPCCall__1_1.md
[18]: Connected.md
[19]: Disconnected.md
[20]: ../../W/AsExtensions/As__1.md
[21]: ../../W/AsExtensions/README.md
[22]: ../../W/AsExtensions/AsJson__1.md
[23]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[24]: ../../W.Threading/ThreadExtensions/README.md
[25]: ../../W/PropertyHostMethods/InitializeProperties.md
[26]: ../../W/PropertyHostMethods/README.md
[27]: ../../W/PropertyHostMethods/IsDirty.md
[28]: ../../W/PropertyHostMethods/MarkAsClean.md
[29]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"