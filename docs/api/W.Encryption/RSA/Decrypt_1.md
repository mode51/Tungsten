RSA.Decrypt Method (String, RSAParameters)
==========================================
  Decrypts a string (previously encrypted with the Encrypt method)

  **Namespace:**  [W.Encryption][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public string Decrypt(
	string base64String,
	RSAParameters privateKey
)
```

#### Parameters

##### *base64String*
Type: [System.String][2]  
The encrypted string

##### *privateKey*
Type: [System.Security.Cryptography.RSAParameters][3]  
The private key used to decrypt the string

#### Return Value
Type: [String][2]  
A string containing the decrypted value

See Also
--------

#### Reference
[RSA Class][4]  
[W.Encryption Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/ke2te33h
[4]: README.md