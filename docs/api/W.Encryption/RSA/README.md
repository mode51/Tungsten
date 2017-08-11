RSA Class
=========
  Provides RSA encryption functionality


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Encryption.RSA**  

  **Namespace:**  [W.Encryption][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class RSA : IDisposable
```

The **RSA** type exposes the following members.


Constructors
------------

                 | Name     | Description                 
---------------- | -------- | --------------------------- 
![Public method] | [RSA][3] | constructs a new RSA object 


Properties
----------

                   | Name               | Description                                                       
------------------ | ------------------ | ----------------------------------------------------------------- 
![Public property] | [LegalKeySizes][4] | Gets the key sizes that are supported by the asymmetric algorithm 
![Public property] | [PrivateKey][5]    | The private key used to decrypt data(do not share)                
![Public property] | [PublicKey][6]     | The public key used to encrypt data (should be shared)            


Methods
-------

                    | Name                                 | Description                                                                                              
------------------- | ------------------------------------ | -------------------------------------------------------------------------------------------------------- 
![Public method]    | [Decrypt(String)][7]                 | Decrypts a string (previously encrypted with the Encrypt method)                                         
![Public method]    | [Decrypt(String, RSAParameters)][8]  | Decrypts a string (previously encrypted with the Encrypt method)                                         
![Public method]    | [Dispose][9]                         | Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. 
![Public method]    | [Encrypt(String)][10]                | Encrypts a string                                                                                        
![Public method]    | [Encrypt(Byte[], RSAParameters)][11] | Encrypts a string                                                                                        
![Public method]    | [Encrypt(String, RSAParameters)][12] | Encrypts a string                                                                                        
![Public method]    | [Equals][13]                         | (Inherited from [Object][1].)                                                                            
![Protected method] | [Finalize][14]                       | Destructs the RSA object and calls Dispose (Overrides [Object.Finalize()][15].)                          
![Public method]    | [GetHashCode][16]                    | (Inherited from [Object][1].)                                                                            
![Public method]    | [GetType][17]                        | (Inherited from [Object][1].)                                                                            
![Protected method] | [MemberwiseClone][18]                | (Inherited from [Object][1].)                                                                            
![Public method]    | [ToString][19]                       | (Inherited from [Object][1].)                                                                            


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][20]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][21].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][22]     | Serializes an object to a Json string (Defined by [AsExtensions][21].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][23]      | Serializes an object to an xml string (Defined by [AsExtensions][21].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][24]   | Starts a new thread (Defined by [ThreadExtensions][25].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][26] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][27].) 
![Public Extension Method]                | [IsDirty][28]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][27].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][29]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][27].)                                                                                                  


See Also
--------

#### Reference
[W.Encryption Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: LegalKeySizes.md
[5]: PrivateKey.md
[6]: PublicKey.md
[7]: Decrypt.md
[8]: Decrypt_1.md
[9]: Dispose.md
[10]: Encrypt_1.md
[11]: Encrypt.md
[12]: Encrypt_2.md
[13]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[14]: Finalize.md
[15]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[16]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[17]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[18]: http://msdn.microsoft.com/en-us/library/57ctke0a
[19]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[20]: ../../W/AsExtensions/As__1.md
[21]: ../../W/AsExtensions/README.md
[22]: ../../W/AsExtensions/AsJson__1.md
[23]: ../../W/AsExtensions/AsXml__1.md
[24]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[25]: ../../W.Threading/ThreadExtensions/README.md
[26]: ../../W/PropertyHostMethods/InitializeProperties.md
[27]: ../../W/PropertyHostMethods/README.md
[28]: ../../W/PropertyHostMethods/IsDirty.md
[29]: ../../W/PropertyHostMethods/MarkAsClean.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"