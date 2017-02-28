AsExtensions Class
==================
  Extensions which convert objects of one type to another


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.AsExtensions**  

  **Namespace:**  [W][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static class AsExtensions
```

The **AsExtensions** type exposes the following members.


Methods
-------

                                                | Name                     | Description                                                                    
----------------------------------------------- | ------------------------ | ------------------------------------------------------------------------------ 
![Public method]![Static member]![Code example] | [As&lt;TType>][3]        | Use Generic syntax for the as operator.                                        
![Public method]![Static member]                | [AsBase64][4]            | Converts a string to Base64 encoding                                           
![Public method]![Static member]                | [AsBytes][5]             | Converts a string to an encoded byte array                                     
![Public method]![Static member]                | [AsJson&lt;TType>][6]    | Serializes an object to a Json string                                          
![Public method]![Static member]                | [AsStream(Byte[])][7]    | Creates a MemoryStream object and initializes it with the specified byte array 
![Public method]![Static member]                | [AsStream(String)][8]    | Creates a MemoryStream object and initializes it with the specified string     
![Public method]![Static member]                | [AsString][9]            | Converts an encoded byte array to a string                                     
![Public method]![Static member]                | [AsXml&lt;TType>][10]    | Serializes an object to an xml string                                          
![Public method]![Static member]                | [FromBase64(Byte[])][11] | Converts a Base64 encoded byte array back to a normal byte array               
![Public method]![Static member]                | [FromBase64(String)][12] | Converts a Base64 encoded string back to a normal string                       


See Also
--------

#### Reference
[W Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: As__1.md
[4]: AsBase64.md
[5]: AsBytes.md
[6]: AsJson__1.md
[7]: AsStream.md
[8]: AsStream_1.md
[9]: AsString.md
[10]: AsXml__1.md
[11]: FromBase64.md
[12]: FromBase64_1.md
[13]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Static member]: ../../_icons/static.gif "Static member"
[Code example]: ../../_icons/CodeExample.png "Code example"