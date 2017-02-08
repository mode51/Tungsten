AssemblyLoader Class
====================
  
[Missing &lt;summary> documentation for "T:W.Domains.AssemblyLoader"]



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [System.MarshalByRefObject][2]  
    **W.Domains.AssemblyLoader**  

  **Namespace:**  [W.Domains][3]  
  **Assembly:**  Tungsten.Domains (in Tungsten.Domains.dll)

Syntax
------

```csharp
public class AssemblyLoader : MarshalByRefObject, 
	IAssemblyLoader
```

The **AssemblyLoader** type exposes the following members.


Constructors
------------

                 | Name                | Description                                                
---------------- | ------------------- | ---------------------------------------------------------- 
![Public method] | [AssemblyLoader][4] | Initializes a new instance of the **AssemblyLoader** class 


Methods
-------

                 | Name                                                            | Description                                                                                                                   
---------------- | --------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------- 
![Public method] | [Create(String)][5]                                             | Instantiates a class and returns a handle to it. This handle must be cast to an interface in order to work across AppDomains. 
![Public method] | [Create&lt;TInterfaceType>(String)][6]                          | Instantiates a class and returns a handle to it. This handle must be cast to an interface in order to work across AppDomains. 
![Public method] | [Execute(String, String, Object[])][7]                          | Instantiates a class and calls a method exposed by it.                                                                        
![Public method] | [Execute&lt;TResult>(String, String, Object[])][8]              | Instantiates a class and calls a method exposed by it.                                                                        
![Public method] | [ExecuteStaticMethod(String, String, Object[])][9]              | Executes a static method on the specified type across the AppDomain                                                           
![Public method] | [ExecuteStaticMethod&lt;TResult>(String, String, Object[])][10] | Executes a static method on the specified type across the AppDomain                                                           
![Public method] | [Load(AppDomain, String)][11]                                   | Loads a dll into the new AppDomain                                                                                            
![Public method] | [Load(AppDomain, String, String)][12]                           | Loads dlls into the new AppDomain                                                                                             


Extension Methods
-----------------

                           | Name                       | Description                                                                                                                                                                                                                      
-------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][13]   | Starts a new thread (Defined by [ThreadExtensions][14].)                                                                                                                                                                         
![Public Extension Method] | [InitializeProperties][15] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][16].) 
![Public Extension Method] | [IsDirty][17]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][16].)                                                                                                                 
![Public Extension Method] | [MarkAsClean][18]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][16].)                                                                                                  


See Also
--------

#### Reference
[W.Domains Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: http://msdn.microsoft.com/en-us/library/w4302s1f
[3]: ../README.md
[4]: _ctor.md
[5]: Create.md
[6]: Create__1.md
[7]: Execute.md
[8]: Execute__1.md
[9]: ExecuteStaticMethod.md
[10]: ExecuteStaticMethod__1.md
[11]: Load.md
[12]: Load_1.md
[13]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[14]: ../../W.Threading/ThreadExtensions/README.md
[15]: ../../W/PropertyHostMethods/InitializeProperties.md
[16]: ../../W/PropertyHostMethods/README.md
[17]: ../../W/PropertyHostMethods/IsDirty.md
[18]: ../../W/PropertyHostMethods/MarkAsClean.md
[19]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"