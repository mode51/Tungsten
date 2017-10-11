AsExtensions.As&lt;TType> Method
================================
   Use Generic syntax for the as operator.

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static TType As<TType>(
	this Object this
)
where TType : class

```

#### Parameters

##### *this*
Type: [System.Object][2]  
The item to convert to type TType

#### Type Parameters

##### *TType*
The type to convert the item reference to.

#### Return Value
Type: **TType**  
Null if @this cannot be referenced as TType. Otherwise, the item as TType
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [Object][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][3] or [Extension Methods (C# Programming Guide)][4].

Examples
--------

```csharp
expression as type
```
 becomes 
```csharp
expression<type>()
```


See Also
--------

#### Reference
[AsExtensions Class][5]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[4]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[5]: README.md