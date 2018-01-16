W.Encryption Namespace
======================
Functionality related to encryption


Classes
-------

                | Class                     | Description                                                                                                                                                                                                                                                                                       
--------------- | ------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public class] | [AssymetricEncryption][1] | Facilitates two way (assymetric) encryption via RSA cryptography                                                                                                                                                                                                                                  
![Public class] | [MD5][2]                  | Used to generate MD5 hashes and verify input strings against them                                                                                                                                                                                                                                 
![Public class] | [PublicPrivateKeyPair][3] | The public and private RSA keys                                                                                                                                                                                                                                                                   
![Public class] | [RSAMethods][4]           | 
Replaces RSA. This code was adaptd for NetStandard from an article published on CodeProject by Mathew John Schlabaugh in 2007. It is less complicated but works more often than my initial RSA implementation. See: https://www.codeproject.com/Articles/10877/Public-Key-RSA-Encryption-in-C-NET
 


Delegates
---------

                   | Delegate                                       | Description                                                                   
------------------ | ---------------------------------------------- | ----------------------------------------------------------------------------- 
![Public delegate] | [AssymetricEncryption.ExchangeKeysDelegate][5] | A delegate used by ExchangeKeys to facilitate the exchange of the public keys 

[1]: AssymetricEncryption/README.md
[2]: MD5/README.md
[3]: PublicPrivateKeyPair/README.md
[4]: RSAMethods/README.md
[5]: AssymetricEncryption_ExchangeKeysDelegate/README.md
[Public class]: ../_icons/pubclass.gif "Public class"
[Public delegate]: ../_icons/pubdelegate.gif "Public delegate"