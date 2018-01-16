AssymetricEncryption.ExchangeKeys Method
========================================
   
Calls the function which completes the exchange and sets RemotePublicKey to the result. This function must be implemented by the developer and is contextual to his or her scenario. In all cases however, the return value must be the remote public key upon success, or null to specify a failure.


  **Namespace:**  [W.Encryption][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public bool ExchangeKeys(
	AssymetricEncryption.ExchangeKeysDelegate del
)
```

#### Parameters

##### *del*
Type: [W.Encryption.AssymetricEncryption.ExchangeKeysDelegate][2]  
The function to call

#### Return Value
Type: [Boolean][3]  
True if RemotePublicKey was assigned a non-null value, otherwise False

See Also
--------

#### Reference
[AssymetricEncryption Class][4]  
[W.Encryption Namespace][1]  

[1]: ../README.md
[2]: ../AssymetricEncryption_ExchangeKeysDelegate/README.md
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: README.md