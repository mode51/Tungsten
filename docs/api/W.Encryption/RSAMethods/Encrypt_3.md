RSAMethods.Encrypt Method (String, String, Int32)
=================================================
  Encrypts a string with RSA encryption

  **Namespace:**  [W.Encryption][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static string Encrypt(
	string text,
	string key,
	int keySize = 2048
)
```

#### Parameters

##### *text*
Type: [System.String][2]  
The string to encrypt

##### *key*
Type: [System.String][2]  
The public key to use to encrypt the string

##### *keySize* (Optional)
Type: [System.Int32][3]  
The keysize, in bits, of the public key

#### Return Value
Type: [String][2]  
The encrypted value of the specified data

See Also
--------

#### Reference
[RSAMethods Class][4]  
[W.Encryption Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: README.md