AsExtensions Class
==================
   Extensions which convert objects of one type to another


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.AsExtensions.AsExtensions**  

  **Namespace:**  [W.AsExtensions][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static class AsExtensions
```

The **AsExtensions** type exposes the following members.


Methods
-------

                                                | Name                                 | Description                                                                        
----------------------------------------------- | ------------------------------------ | ---------------------------------------------------------------------------------- 
![Public method]![Static member]![Code example] | [As&lt;TType>][3]                    | Use Generic syntax for the as operator.                                            
![Public method]![Static member]                | [AsBase64(Byte[])][4]                | Converts a byte array to a Base64 encoded string                                   
![Public method]![Static member]                | [AsBase64(String)][5]                | Converts a string to Base64 encoding                                               
![Public method]![Static member]                | [AsBytes][6]                         | Converts a string to an encoded byte array                                         
![Public method]![Static member]                | [AsCompressed][7]                    | Compresses the byte array using System.IO.Compression.DeflateStream                
![Public method]![Static member]                | [AsDecompressed][8]                  | **Obsolete.**Decompresses the byte array using System.IO.Compression.DeflateStream 
![Public method]![Static member]                | [AsJson&lt;TType>][9]                | Serializes an object to a Json string                                              
![Public method]![Static member]                | [AsStream(Byte[])][10]               | Creates a MemoryStream object and initializes it with the specified byte array     
![Public method]![Static member]                | [AsStream(String)][11]               | Creates a MemoryStream object and initializes it with the specified string         
![Public method]![Static member]                | [AsString(Byte[])][12]               | Converts an encoded byte array to a string                                         
![Public method]![Static member]                | [AsString(Byte[], Int32, Int32)][13] | Converts an encoded byte array to a string                                         
![Public method]![Static member]                | [AsXml&lt;TType>][14]                | Serializes an object to an xml string                                              


See Also
--------

#### Reference
[W.AsExtensions Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: As__1.md
[4]: AsBase64.md
[5]: AsBase64_1.md
[6]: AsBytes.md
[7]: AsCompressed.md
[8]: AsDecompressed.md
[9]: AsJson__1.md
[10]: AsStream.md
[11]: AsStream_1.md
[12]: AsString.md
[13]: AsString_1.md
[14]: AsXml__1.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"
[Code example]: ../../_icons/CodeExample.png "Code example"