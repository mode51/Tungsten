FromExtensions.FromXml&lt;TType> Method (String)
================================================
   Deserializes an Xml string to an object

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static TType FromXml<TType>(
	this string this
)

```

#### Parameters

##### *this*
Type: [System.String][2]  
The Json formatted string

#### Type Parameters

##### *TType*
The type of object to deserialize

#### Return Value
Type: **TType**  
A new instance of TType deserialized from the specified Xml string
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [String][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][3] or [Extension Methods (C# Programming Guide)][4].

See Also
--------

#### Reference
[FromExtensions Class][5]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[4]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[5]: README.md