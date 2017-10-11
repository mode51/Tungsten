RSAMethods.Encrypt Method
=========================
   Encrypts a string using the specified keysize and public key

  **Namespace:**  [W.Encryption][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static string Encrypt(
	string inputString,
	RSAParameters publicKey
)
```

#### Parameters

##### *inputString*
Type: [System.String][2]  
The data to encrypt

##### *publicKey*
Type: [System.Security.Cryptography.RSAParameters][3]  
The public key used to encrypt the data

#### Return Value
Type: [String][2]  
A string containing the encrypted data

See Also
--------

#### Reference
[RSAMethods Class][4]  
[W.Encryption Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/ke2te33h
[4]: README.md