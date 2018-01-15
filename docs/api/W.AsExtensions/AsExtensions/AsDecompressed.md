AsExtensions.AsDecompressed Method
==================================
   

**Note: This API is now obsolete.**
Decompresses the byte array using System.IO.Compression.DeflateStream

  **Namespace:**  [W.AsExtensions][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
[ObsoleteAttribute("Use FromCompressed instead.")]
public static byte[] AsDecompressed(
	this byte[] bytes
)
```

#### Parameters

##### *bytes*
Type: [System.Byte][2][]  
The byte array containing compressed data

#### Return Value
Type: [Byte][2][]  
A byte array of the decompressed data
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type . When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][3] or [Extension Methods (C# Programming Guide)][4].

See Also
--------

#### Reference
[AsExtensions Class][5]  
[W.AsExtensions Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/yyb1w04y
[3]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[4]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[5]: README.md