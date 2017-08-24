MD5 Class
=========
  Used to generate MD5 hashes and verify input strings against them


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Encryption.MD5**  

  **Namespace:**  [W.Encryption][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class MD5
```

The **MD5** type exposes the following members.


Constructors
------------

                 | Name     | Description                                     
---------------- | -------- | ----------------------------------------------- 
![Public method] | [MD5][3] | Initializes a new instance of the **MD5** class 


Methods
-------

                                 | Name                         | Description                               
-------------------------------- | ---------------------------- | ----------------------------------------- 
![Public method]                 | [Equals][4]                  | (Inherited from [Object][1].)             
![Protected method]              | [Finalize][5]                | (Inherited from [Object][1].)             
![Public method]                 | [GetHashCode][6]             | (Inherited from [Object][1].)             
![Public method]![Static member] | [GetMd5Hash(String)][7]      | Generates an MD5 hash of the input string 
![Public method]![Static member] | [GetMd5Hash(String, MD5)][8] | Generates an MD5 hash of the input string 
![Public method]                 | [GetType][9]                 | (Inherited from [Object][1].)             
![Protected method]              | [MemberwiseClone][10]        | (Inherited from [Object][1].)             
![Public method]                 | [ToString][11]               | (Inherited from [Object][1].)             
![Public method]![Static member] | [VerifyMd5Hash][12]          | Verifies a hash against a string          


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][13]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][14].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][15]     | Serializes an object to a Json string (Defined by [AsExtensions][14].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][16]      | Serializes an object to an xml string (Defined by [AsExtensions][14].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][17]   | Starts a new thread (Defined by [ThreadExtensions][18].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][19] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][20].) 
![Public Extension Method]                | [IsDirty][21]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][20].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][22]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][20].)                                                                                                  
![Public Extension Method]                | [WaitForValue][23]         | Initiates a Task which will wait for the given variable to have the specified value (Defined by [ExtensionMethods][24].)                                                                                                         


See Also
--------

#### Reference
[W.Encryption Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[5]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[6]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[7]: GetMd5Hash.md
[8]: GetMd5Hash_1.md
[9]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[10]: http://msdn.microsoft.com/en-us/library/57ctke0a
[11]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[12]: VerifyMd5Hash.md
[13]: ../../W/AsExtensions/As__1.md
[14]: ../../W/AsExtensions/README.md
[15]: ../../W/AsExtensions/AsJson__1.md
[16]: ../../W/AsExtensions/AsXml__1.md
[17]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[18]: ../../W.Threading/ThreadExtensions/README.md
[19]: ../../W/PropertyHostMethods/InitializeProperties.md
[20]: ../../W/PropertyHostMethods/README.md
[21]: ../../W/PropertyHostMethods/IsDirty.md
[22]: ../../W/PropertyHostMethods/MarkAsClean.md
[23]: ../../W/ExtensionMethods/WaitForValue.md
[24]: ../../W/ExtensionMethods/README.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Static member]: ../../_icons/static.gif "Static member"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"