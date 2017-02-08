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
-------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][12]   | Starts a new thread (Defined by [ThreadExtensions][13].)                                                                                                                                                                         
![Public Extension Method] | [InitializeProperties][14] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][15].) 
![Public Extension Method] | [IsDirty][16]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][15].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][17]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][15].)                                                                                                  


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
[12]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[13]: ../../W.Threading/ThreadExtensions/README.md
[14]: ../../W/PropertyHostMethods/InitializeProperties.md
[15]: ../../W/PropertyHostMethods/README.md
[16]: ../../W/PropertyHostMethods/IsDirty.md
[17]: ../../W/PropertyHostMethods/MarkAsClean.md
[18]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"