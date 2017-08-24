RSAMethods.Encrypt Method (Byte[], RSAParameters, Int32)
========================================================
  Encrypts a byte array with RSA encryption

  **Namespace:**  [W.Encryption][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static string Encrypt(
	byte[] byteData,
	RSAParameters key,
	int keySize = 2048
)
```

#### Parameters

##### *byteData*
Type: [System.Byte][2][]  
The byte array to encrypt

##### *key*
Type: [System.Security.Cryptography.RSAParameters][3]  
The public key to use to encrypt the byte array

##### *keySize* (Optional)
Type: [System.Int32][4]  
The keysize, in bits, of the public key

#### Return Value
Type: [String][5]  
The encrypted byte array of the specified data

See Also
--------

#### Reference
[RSAMethods Class][6]  
[W.Encryption Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/yyb1w04y
[3]: http://msdn.microsoft.com/en-us/library/ke2te33h
[4]: http://msdn.microsoft.com/en-us/library/td2s409d
[5]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[6]: README.md