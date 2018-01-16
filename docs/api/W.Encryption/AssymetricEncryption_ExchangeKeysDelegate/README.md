AssymetricEncryption.ExchangeKeysDelegate Delegate
==================================================
   A delegate used by ExchangeKeys to facilitate the exchange of the public keys

  **Namespace:**  [W.Encryption][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate Nullable<RSAParameters> ExchangeKeysDelegate(
	RSAParameters localPublicKey
)
```

#### Parameters

##### *localPublicKey*
Type: [System.Security.Cryptography.RSAParameters][2]  
The local public key

#### Return Value
Type: [Nullable][3]&lt;[RSAParameters][2]>  
Return the remote public key

See Also
--------

#### Reference
[W.Encryption Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/ke2te33h
[3]: http://msdn.microsoft.com/en-us/library/b3h38hb0