Server Class
============
  Hosts an RPC instance


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [System.MarshalByRefObject][2]  
    **W.RPC.Server**  

  **Namespace:**  [W.RPC][3]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public class Server : MarshalByRefObject, IDisposable, 
	IServer
```

The **Server** type exposes the following members.


Constructors
------------

                 | Name                          | Description                                                                                                
---------------- | ----------------------------- | ---------------------------------------------------------------------------------------------------------- 
![Public method] | [Server()][4]                 | Constructor for the Server class. This constructor does not start listening for clients.                   
![Public method] | [Server(IPAddress, Int32)][5] | Constructor for the Server class which automatically starts listening on the specified IP Address and Port 


Methods
-------

                                 | Name          | Description                                                                                              
-------------------------------- | ------------- | -------------------------------------------------------------------------------------------------------- 
![Public method]![Static member] | [Create][6]   | Creates a new Server instance and starts listening on the specified ipAddress and port                   
![Public method]                 | [Dispose][7]  | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. 
![Protected method]              | [Finalize][8] | Destructor for the Server class. Calls Dispose. (Overrides [Object.Finalize()][9].)                      
![Public method]                 | [Start][10]   | Starts listening for client connections                                                                  
![Public method]                 | [Stop][11]    | Stops listening for client connections                                                                   


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][12]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][13].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][14]     | Serializes an object to a Json string (Defined by [AsExtensions][13].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][15]      | Serializes an object to an xml string (Defined by [AsExtensions][13].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][16]   | Starts a new thread (Defined by [ThreadExtensions][17].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][18] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][19].) 
![Public Extension Method]                | [IsDirty][20]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][19].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][21]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][19].)                                                                                                  


See Also
--------

#### Reference
[W.RPC Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: http://msdn.microsoft.com/en-us/library/w4302s1f
[3]: ../README.md
[4]: _ctor.md
[5]: _ctor_1.md
[6]: Create.md
[7]: Dispose.md
[8]: Finalize.md
[9]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[10]: Start.md
[11]: Stop.md
[12]: ../../W/AsExtensions/As__1.md
[13]: ../../W/AsExtensions/README.md
[14]: ../../W/AsExtensions/AsJson__1.md
[15]: ../../W/AsExtensions/AsXml__1.md
[16]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[17]: ../../W.Threading/ThreadExtensions/README.md
[18]: ../../W/PropertyHostMethods/InitializeProperties.md
[19]: ../../W/PropertyHostMethods/README.md
[20]: ../../W/PropertyHostMethods/IsDirty.md
[21]: ../../W/PropertyHostMethods/MarkAsClean.md
[22]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"