RSAMethods Class
================
  Static Encrypt and Decrypt methods


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Encryption.RSAMethods**  

  **Namespace:**  [W.Encryption][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class RSAMethods
```

The **RSAMethods** type exposes the following members.


Constructors
------------

                 | Name            | Description                                            
---------------- | --------------- | ------------------------------------------------------ 
![Public method] | [RSAMethods][3] | Initializes a new instance of the **RSAMethods** class 


Methods
-------

                                 | Name                                       | Description                                                
-------------------------------- | ------------------------------------------ | ---------------------------------------------------------- 
![Public method]![Static member] | [Decrypt(String, RSAParameters, Int32)][4] | Decrypts a string previously encrypted with RSA encryption 
![Public method]![Static member] | [Decrypt(String, String, Int32)][5]        | Decrypts a string previously encrypted with RSA encryption 
![Public method]![Static member] | [Encrypt(Byte[], RSAParameters, Int32)][6] | Encrypts a byte array with RSA encryption                  
![Public method]![Static member] | [Encrypt(Byte[], String, Int32)][7]        | Encrypts a byte array with RSA encryption                  
![Public method]![Static member] | [Encrypt(String, RSAParameters, Int32)][8] | Encrypts a string with RSA encryption                      
![Public method]![Static member] | [Encrypt(String, String, Int32)][9]        | Encrypts a string with RSA encryption                      
![Public method]                 | [Equals][10]                               | (Inherited from [Object][1].)                              
![Protected method]              | [Finalize][11]                             | (Inherited from [Object][1].)                              
![Public method]                 | [GetHashCode][12]                          | (Inherited from [Object][1].)                              
![Public method]                 | [GetType][13]                              | (Inherited from [Object][1].)                              
![Protected method]              | [MemberwiseClone][14]                      | (Inherited from [Object][1].)                              
![Public method]                 | [ToString][15]                             | (Inherited from [Object][1].)                              


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][16]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][17].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][18]     | Serializes an object to a Json string (Defined by [AsExtensions][17].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][19]      | Serializes an object to an xml string (Defined by [AsExtensions][17].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][20]   | Starts a new thread (Defined by [ThreadExtensions][21].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][22] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][23].) 
![Public Extension Method]                | [IsDirty][24]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][23].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][25]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][23].)                                                                                                  


See Also
--------

#### Reference
[W.Encryption Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: Decrypt.md
[5]: Decrypt_1.md
[6]: Encrypt.md
[7]: Encrypt_1.md
[8]: Encrypt_2.md
[9]: Encrypt_3.md
[10]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[11]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[12]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[13]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[14]: http://msdn.microsoft.com/en-us/library/57ctke0a
[15]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[16]: ../../W/AsExtensions/As__1.md
[17]: ../../W/AsExtensions/README.md
[18]: ../../W/AsExtensions/AsJson__1.md
[19]: ../../W/AsExtensions/AsXml__1.md
[20]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[21]: ../../W.Threading/ThreadExtensions/README.md
[22]: ../../W/PropertyHostMethods/InitializeProperties.md
[23]: ../../W/PropertyHostMethods/README.md
[24]: ../../W/PropertyHostMethods/IsDirty.md
[25]: ../../W/PropertyHostMethods/MarkAsClean.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"