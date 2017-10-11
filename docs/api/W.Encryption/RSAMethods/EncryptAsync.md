RSAMethods.EncryptAsync Method
==============================
   Asynchronously encrypts a string using the specified keysize and public key

  **Namespace:**  [W.Encryption][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Task<string> EncryptAsync(
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
Type: [Task][4]&lt;[String][2]>  
A string containing the encrypted data

See Also
--------

#### Reference
[RSAMethods Class][5]  
[W.Encryption Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/ke2te33h
[4]: http://msdn.microsoft.com/en-us/library/dd321424
[5]: README.md