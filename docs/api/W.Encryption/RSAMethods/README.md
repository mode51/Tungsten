RSAMethods Class
================
   
Replaces RSA. This code was adaptd for NetStandard from an article published on CodeProject by Mathew John Schlabaugh in 2007. It is less complicated but works more often than my initial RSA implementation. See: https://www.codeproject.com/Articles/10877/Public-Key-RSA-Encryption-in-C-NET



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Encryption.RSAMethods**  

  **Namespace:**  [W.Encryption][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static class RSAMethods
```

The **RSAMethods** type exposes the following members.


Methods
-------

                                 | Name               | Description                                                                             
-------------------------------- | ------------------ | --------------------------------------------------------------------------------------- 
![Public method]![Static member] | [CreateKeyPair][3] | Generates a public/private key pair                                                     
![Public method]![Static member] | [Decrypt][4]       | Decrypts a string which was previously encrypted with the Encrypt method                
![Public method]![Static member] | [DecryptAsync][5]  | Asynchronously decrypts a string which was previously encrypted with the Encrypt method 
![Public method]![Static member] | [Encrypt][6]       | Encrypts a string using the specified keysize and public key                            
![Public method]![Static member] | [EncryptAsync][7]  | Asynchronously encrypts a string using the specified keysize and public key             
![Public method]![Static member] | [LegalKeySizes][8] | Returns an arrary containing the supported key sizes                                    


See Also
--------

#### Reference
[W.Encryption Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: CreateKeyPair.md
[4]: Decrypt.md
[5]: DecryptAsync.md
[6]: Encrypt.md
[7]: EncryptAsync.md
[8]: LegalKeySizes.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"