RSAMethods.Decrypt Method (String, String, Int32)
=================================================
  Decrypts a string previously encrypted with RSA encryption

  **Namespace:**  [W.Encryption][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static string Decrypt(
	string text,
	string key,
	int keySize = 2048
)
```

#### Parameters

##### *text*
Type: [System.String][2]  
The RSA encrypted string

##### *key*
Type: [System.String][2]  
The private key to use for decrypting

##### *keySize* (Optional)
Type: [System.Int32][3]  
The keysize, in bits, of the private key

#### Return Value
Type: [String][2]  
The decrypted string

See Also
--------

#### Reference
[RSAMethods Class][4]  
[W.Encryption Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/td2s409d
[4]: README.md