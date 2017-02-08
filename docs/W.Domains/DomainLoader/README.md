DomainLoader Class
==================
  
[Missing &lt;summary> documentation for "T:W.Domains.DomainLoader"]



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Domains.DomainLoader**  

  **Namespace:**  [W.Domains][2]  
  **Assembly:**  Tungsten.Domains (in Tungsten.Domains.dll)

Syntax
------

```csharp
public class DomainLoader : IDomainLoader, 
	IDisposable
```

The **DomainLoader** type exposes the following members.


Constructors
------------

                 | Name                                       | Description                                      
---------------- | ------------------------------------------ | ------------------------------------------------ 
![Public method] | [DomainLoader(String, Boolean)][3]         | Creates an AppDomain under the current AppDomain 
![Public method] | [DomainLoader(String, String, Boolean)][4] | Creates an AppDomain under the current AppDomain 


Properties
----------

                   | Name            | Description 
------------------ | --------------- | ----------- 
![Public property] | [DomainName][5] |             


Methods
-------

                    | Name                                                            | Description                                                                                                                                 
------------------- | --------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Create(String)][6]                                             | Instantiates a class and returns a handle to it. This handle must be cast to an interface in order to work across AppDomains.               
![Public method]    | [Create&lt;TInterfaceType>(String)][7]                          | Instantiates a class and returns a handle to it. This handle must be cast to an interface in order to work across AppDomains.               
![Public method]    | [Dispose][8]                                                    | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.                                    
![Public method]    | [DoCallback][9]                                                 |                                                                                                                                             
![Public method]    | [Execute(String, String, Object[])][10]                         | Instantiates a class and calls a method exposed by it.                                                                                      
![Public method]    | [Execute&lt;TResult>(String, String, Object[])][11]             | Instantiates a class and calls a method exposed by it.                                                                                      
![Public method]    | [ExecuteStaticMethod(String, String, Object[])][12]             | Executes a static method on the specified type across the AppDomain                                                                         
![Public method]    | [ExecuteStaticMethod&lt;TResult>(String, String, Object[])][13] | Executes a static method on the specified type across the AppDomain                                                                         
![Protected method] | [Finalize][14]                                                  | (Overrides [Object.Finalize()][15].)                                                                                                        
![Public method]    | [GetData&lt;TData>][16]                                         | Gets the value stored in the current application domain for the specified name                                                              
![Public method]    | [Load][17]                                                      | Loads the dlls into the new AppDomain                                                                                                       
![Public method]    | [SetData][18]                                                   |                                                                                                                                             
![Public method]    | [Unload][19]                                                    | Unloads the AppDomain and deletes files in the cache folder. The cache folder is where dlls are copied, and run, when using shadow copying. 


Extension Methods
-----------------

                           | Name                       | Description                                                                                                                                                                                                                      
-------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][20]   | Starts a new thread (Defined by [ThreadExtensions][21].)                                                                                                                                                                         
![Public Extension Method] | [InitializeProperties][22] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][23].) 
![Public Extension Method] | [IsDirty][24]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][23].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][25]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][23].)                                                                                                  


See Also
--------

#### Reference
[W.Domains Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: _ctor_1.md
[5]: DomainName.md
[6]: Create.md
[7]: Create__1.md
[8]: Dispose.md
[9]: DoCallback.md
[10]: Execute.md
[11]: Execute__1.md
[12]: ExecuteStaticMethod.md
[13]: ExecuteStaticMethod__1.md
[14]: Finalize.md
[15]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[16]: GetData__1.md
[17]: Load.md
[18]: SetData.md
[19]: Unload.md
[20]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[21]: ../../W.Threading/ThreadExtensions/README.md
[22]: ../../W/PropertyHostMethods/InitializeProperties.md
[23]: ../../W/PropertyHostMethods/README.md
[24]: ../../W/PropertyHostMethods/IsDirty.md
[25]: ../../W/PropertyHostMethods/MarkAsClean.md
[26]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"