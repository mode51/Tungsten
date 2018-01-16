AssymetricEncryption Class
==========================
   Facilitates two way (assymetric) encryption via RSA cryptography


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Encryption.AssymetricEncryption**  

  **Namespace:**  [W.Encryption][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class AssymetricEncryption
```

The **AssymetricEncryption** type exposes the following members.


Constructors
------------

                 | Name                      | Description                                
---------------- | ------------------------- | ------------------------------------------ 
![Public method] | [AssymetricEncryption][3] | Constructs a new TwoWayEncryption instance 


Properties
----------

                   | Name                 | Description                                       
------------------ | -------------------- | ------------------------------------------------- 
![Public property] | [LegalKeySizes][4]   | The legal RSA key sizes supported by the platform 
![Public property] | [PublicKey][5]       | The local public key                              
![Public property] | [RemotePublicKey][6] | The remote's public key                           


Methods
-------

                    | Name                  | Description                                                                                                                                                                                                                                                                                           
------------------- | --------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Decrypt][7]          | Decrypts data with the local private key                                                                                                                                                                                                                                                              
![Public method]    | [Encrypt][8]          | Encrypts data with the remote public key                                                                                                                                                                                                                                                              
![Public method]    | [Equals][9]           | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                         
![Public method]    | [ExchangeKeys][10]    | 
Calls the function which completes the exchange and sets RemotePublicKey to the result. This function must be implemented by the developer and is contextual to his or her scenario. In all cases however, the return value must be the remote public key upon success, or null to specify a failure.
 
![Protected method] | [Finalize][11]        | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                         
![Public method]    | [GetHashCode][12]     | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                         
![Public method]    | [GetType][13]         | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                         
![Protected method] | [MemberwiseClone][14] | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                         
![Public method]    | [ToString][15]        | (Inherited from [Object][1].)                                                                                                                                                                                                                                                                         


See Also
--------

#### Reference
[W.Encryption Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: LegalKeySizes.md
[5]: PublicKey.md
[6]: RemotePublicKey.md
[7]: Decrypt.md
[8]: Encrypt.md
[9]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[10]: ExchangeKeys.md
[11]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[12]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[13]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[14]: http://msdn.microsoft.com/en-us/library/57ctke0a
[15]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"