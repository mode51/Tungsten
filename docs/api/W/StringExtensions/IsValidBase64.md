StringExtensions.IsValidBase64 Method
=====================================
   Validates the given string conforms to Base64 encoding. It does not verify the value is a Base64 encoded value.

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static bool IsValidBase64(
	string value
)
```

#### Parameters

##### *value*
Type: [System.String][2]  
The string to test

#### Return Value
Type: [Boolean][3]  
True if the value is valid Base64, otherwise false

Remarks
-------

This solution is based on a StackOverflow [answer][4]


See Also
--------

#### Reference
[StringExtensions Class][5]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/a28wyd50
[4]: http://stackoverflow.com/questions/8571501/how-to-check-whether-the-string-is-base64-encoded-or-not
[5]: README.md