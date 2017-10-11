RSAMethods Class
================
   
Replaces RSA. This code was adaptd for NetStandard from an article published on CodeProject by Mathew John Schlabaugh in 2007. It is less complicated but works more often than my initial RSA implementation. See: https://www.codeproject.com/Articles/10877/Public-Key-RSA-Encryption-in-C-NET



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Encryption.RSAMethods**  

  **Namespace:**  [W.Encryption][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class RSAMethods
```

The **RSAMethods** type exposes the following members.


Constructors
------------

                 | Name            | Description                                            
---------------- | --------------- | ------------------------------------------------------ 
![Public method] | [RSAMethods][3] | Initializes a new instance of the **RSAMethods** class 


Methods
-------

                                 | Name                  | Description                                                                             
-------------------------------- | --------------------- | --------------------------------------------------------------------------------------- 
![Public method]![Static member] | [CreateKeyPair][4]    | Generates a public/private key pair                                                     
![Public method]![Static member] | [Decrypt][5]          | Decrypts a string which was previously encrypted with the Encrypt method                
![Public method]![Static member] | [DecryptAsync][6]     | Asynchronously decrypts a string which was previously encrypted with the Encrypt method 
![Public method]![Static member] | [Encrypt][7]          | Encrypts a string using the specified keysize and public key                            
![Public method]![Static member] | [EncryptAsync][8]     | Asynchronously encrypts a string using the specified keysize and public key             
![Public method]                 | [Equals][9]           | (Inherited from [Object][1].)                                                           
![Protected method]              | [Finalize][10]        | (Inherited from [Object][1].)                                                           
![Public method]                 | [GetHashCode][11]     | (Inherited from [Object][1].)                                                           
![Public method]                 | [GetType][12]         | (Inherited from [Object][1].)                                                           
![Public method]![Static member] | [LegalKeySizes][13]   | Returns an arrary containing the supported key sizes                                    
![Protected method]              | [MemberwiseClone][14] | (Inherited from [Object][1].)                                                           
![Public method]                 | [ToString][15]        | (Inherited from [Object][1].)                                                           


Extension Methods
-----------------

                                          | Name                                                                                         | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][16]                                                                           | Use Generic syntax for the as operator. (Defined by [AsExtensions][17].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][18]                                                                       | Serializes an object to a Json string (Defined by [AsExtensions][17].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][19]                                                                        | Serializes an object to an xml string (Defined by [AsExtensions][17].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>)][20]          | Overloaded. Creates and starts a new thread and (Defined by [ThreadExtensions][21].)                                                                                                                                             
![Public Extension Method]                | [CreateThread&lt;TParameterType>(Action&lt;TParameterType, CancellationToken>, Boolean)][22] | Overloaded. Creates a new thread (Defined by [ThreadExtensions][21].)                                                                                                                                                            
![Public Extension Method]                | [InitializeProperties][23]                                                                   | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][24].) 
![Public Extension Method]                | [IsDirty][25]                                                                                | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][24].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][26]                                                                            | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][24].)                                                                                                  
![Public Extension Method]                | [WaitForValueAsync][27]                                                                      | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][28].)                                                                                                         


See Also
--------

#### Reference
[W.Encryption Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: CreateKeyPair.md
[5]: Decrypt.md
[6]: DecryptAsync.md
[7]: Encrypt.md
[8]: EncryptAsync.md
[9]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[10]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[11]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[12]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[13]: LegalKeySizes.md
[14]: http://msdn.microsoft.com/en-us/library/57ctke0a
[15]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[16]: ../../W/AsExtensions/As__1.md
[17]: ../../W/AsExtensions/README.md
[18]: ../../W/AsExtensions/AsJson__1.md
[19]: ../../W/AsExtensions/AsXml__1.md
[20]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[21]: ../../W.Threading/ThreadExtensions/README.md
[22]: ../../W.Threading/ThreadExtensions/CreateThread__1_1.md
[23]: ../../W/PropertyHostMethods/InitializeProperties.md
[24]: ../../W/PropertyHostMethods/README.md
[25]: ../../W/PropertyHostMethods/IsDirty.md
[26]: ../../W/PropertyHostMethods/MarkAsClean.md
[27]: ../../W/ExtensionMethods/WaitForValueAsync.md
[28]: ../../W/ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"