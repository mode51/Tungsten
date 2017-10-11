RSAMethods.Decrypt Method
=========================
   Decrypts a string which was previously encrypted with the Encrypt method

  **Namespace:**  [W.Encryption][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static string Decrypt(
	string cipher,
	RSAParameters privateKey
)
```

#### Parameters

##### *cipher*
Type: [System.String][2]  
The encrypted byte array

##### *privateKey*
Type: [System.Security.Cryptography.RSAParameters][3]  
The private key used to decrypt the data

#### Return Value
Type: [String][2]  
A byte array containing the decrypted value

See Also
--------

#### Reference
[RSAMethods Class][4]  
[W.Encryption Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/ke2te33h
[4]: README.md