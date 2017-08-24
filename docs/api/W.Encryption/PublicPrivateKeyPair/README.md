PublicPrivateKeyPair Class
==========================
  The public and private RSA keys


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Encryption.PublicPrivateKeyPair**  

  **Namespace:**  [W.Encryption][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class PublicPrivateKeyPair
```

The **PublicPrivateKeyPair** type exposes the following members.


Constructors
------------

                 | Name                      | Description                                                      
---------------- | ------------------------- | ---------------------------------------------------------------- 
![Public method] | [PublicPrivateKeyPair][3] | Initializes a new instance of the **PublicPrivateKeyPair** class 


Properties
----------

                   | Name            | Description                                  
------------------ | --------------- | -------------------------------------------- 
![Public property] | [PrivateKey][4] | The private RSA key. Should never be shared. 
![Public property] | [PublicKey][5]  | the public RSA key. Should be shared.        


Methods
-------

                    | Name                  | Description                   
------------------- | --------------------- | ----------------------------- 
![Public method]    | [Equals][6]           | (Inherited from [Object][1].) 
![Protected method] | [Finalize][7]         | (Inherited from [Object][1].) 
![Public method]    | [GetHashCode][8]      | (Inherited from [Object][1].) 
![Public method]    | [GetType][9]          | (Inherited from [Object][1].) 
![Protected method] | [MemberwiseClone][10] | (Inherited from [Object][1].) 
![Public method]    | [ToString][11]        | (Inherited from [Object][1].) 


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
![Public Extension Method]                | [WaitForValue][22]         | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][23].)                                                                                                         


See Also
--------

#### Reference
[W.Encryption Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: PrivateKey.md
[5]: PublicKey.md
[6]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[7]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[8]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[9]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[10]: http://msdn.microsoft.com/en-us/library/57ctke0a
[11]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
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
[22]: ../../W/ExtensionMethods/WaitForValue.md
[23]: ../../W/ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"