RSA.Encrypt Method (Byte[], RSAParameters)
==========================================
  Encrypts a string

  **Namespace:**  [W.Encryption][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public string Encrypt(
	byte[] byteData,
	RSAParameters publicKey
)
```

#### Parameters

##### *byteData*
Type: [System.Byte][2][]  
The data to encrypt

##### *publicKey*
Type: [System.Security.Cryptography.RSAParameters][3]  
The public key used to encrypt the data

#### Return Value
Type: [String][4]  
A string containing the encrypted data

See Also
--------

#### Reference
[RSA Class][5]  
[W.Encryption Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/yyb1w04y
[3]: http://msdn.microsoft.com/en-us/library/ke2te33h
[4]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[5]: README.md