Disposable Class
================
   Provides the Disposable pattern as a base class


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Disposable**  

  **Namespace:**  [W][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Disposable : IDisposable
```

The **Disposable** type exposes the following members.


Constructors
------------

                 | Name            | Description                                            
---------------- | --------------- | ------------------------------------------------------ 
![Public method] | [Disposable][3] | Initializes a new instance of the **Disposable** class 


Methods
-------

                    | Name                     | Description                                                                                                                          
------------------- | ------------------------ | ------------------------------------------------------------------------------------------------------------------------------------ 
![Public method]    | [Dispose][4]             | This code added to correctly implement the disposable pattern.                                                                       
![Public method]    | [Equals][5]              | (Inherited from [Object][1].)                                                                                                        
![Protected method] | [Finalize][6]            | Override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources. (Overrides [Object.Finalize()][7].) 
![Public method]    | [GetHashCode][8]         | (Inherited from [Object][1].)                                                                                                        
![Public method]    | [GetType][9]             | (Inherited from [Object][1].)                                                                                                        
![Protected method] | [MemberwiseClone][10]    | (Inherited from [Object][1].)                                                                                                        
![Protected method] | [OnDispose][11]          | Overload to dispose managed objects                                                                                                  
![Protected method] | [OnDisposeUnmanaged][12] | Override to release unmanaged objects                                                                                                
![Public method]    | [ToString][13]           | (Inherited from [Object][1].)                                                                                                        


Fields
------

                   | Name              | Description                                        
------------------ | ----------------- | -------------------------------------------------- 
![Protected field] | [IsDisposed][14]  | If True, the object has been disposed              
![Protected field] | [IsDisposing][15] | If True, the object is in the process of disposing 


Extension Methods
-----------------

                                          | Name                                                                                         | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][16]                                                                           | Use Generic syntax for the as operator. (Defined by [AsExtensions][17].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][18]                                                                       | Serializes an object to a Json string (Defined by [AsExtensions][17].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][19]                                                                        | Serializes an object to an xml string (Defined by [AsExtensions][17].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>)][20]          | Overloaded. Creates and starts a new thread and (Defined by [ThreadExtensions][21].)                                                                                                                                             
![Public Extension Method]                | [CreateThread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>, Boolean)][22] | Overloaded. Creates a new thread (Defined by [ThreadExtensions][21].)                                                                                                                                                            
![Public Extension Method]                | [InitializeProperties][23]                                                                   | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][24].) 
![Public Extension Method]                | [IsDirty][25]                                                                                | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][24].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][26]                                                                            | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][24].)                                                                                                  
![Public Extension Method]                | [WaitForValueAsync][27]                                                                      | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][28].)                                                                                                         


See Also
--------

#### Reference
[W Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: Dispose.md
[5]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[6]: Finalize.md
[7]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[8]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[9]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[10]: http://msdn.microsoft.com/en-us/library/57ctke0a
[11]: OnDispose.md
[12]: OnDisposeUnmanaged.md
[13]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[14]: IsDisposed.md
[15]: IsDisposing.md
[16]: ../AsExtensions/As__1.md
[17]: ../AsExtensions/README.md
[18]: ../AsExtensions/AsJson__1.md
[19]: ../AsExtensions/AsXml__1.md
[20]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[21]: ../../W.Threading/ThreadExtensions/README.md
[22]: ../../W.Threading/ThreadExtensions/CreateThread__1_1.md
[23]: ../PropertyHostMethods/InitializeProperties.md
[24]: ../PropertyHostMethods/README.md
[25]: ../PropertyHostMethods/IsDirty.md
[26]: ../PropertyHostMethods/MarkAsClean.md
[27]: ../ExtensionMethods/WaitForValueAsync.md
[28]: ../ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Protected field]: ../../_icons/protfield.gif "Protected field"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"