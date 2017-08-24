FromExtensions Class
====================
  Extensions which convert objects of one type to another


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.FromExtensions**  

  **Namespace:**  [W][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static class FromExtensions
```

The **FromExtensions** type exposes the following members.


Methods
-------

                                 | Name                            | Description                                                           
-------------------------------- | ------------------------------- | --------------------------------------------------------------------- 
![Public method]![Static member] | [FromBase64(Byte[])][3]         | Converts a Base64 encoded byte array back to a normal byte array      
![Public method]![Static member] | [FromBase64(String)][4]         | Converts a Base64 encoded string back to a normal string              
![Public method]![Static member] | [FromCompressed][5]             | Decompresses the byte array using System.IO.Compression.DeflateStream 
![Public method]![Static member] | [FromJson&lt;TType>(Byte[])][6] | Deserializes an encoded byte array of Json to an object               
![Public method]![Static member] | [FromJson&lt;TType>(String)][7] | Deserializes a Json string to an object                               
![Public method]![Static member] | [FromXml&lt;TType>(Byte[])][8]  | Deserializes an Xml string to an object                               
![Public method]![Static member] | [FromXml&lt;TType>(String)][9]  | Deserializes an Xml string to an object                               


See Also
--------

#### Reference
[W Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: FromBase64.md
[4]: FromBase64_1.md
[5]: FromCompressed.md
[6]: FromJson__1.md
[7]: FromJson__1_1.md
[8]: FromXml__1.md
[9]: FromXml__1_1.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"