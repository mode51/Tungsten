ArrayMethods.Trim&lt;T> Method
==============================
   Removes the specified range of elements from the array, resizing the array as necessary

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static T[] Trim<T>(
	ref T[] source,
	int startIndex,
	int length
)

```

#### Parameters

##### *source*
Type: **T**[]  
The source array

##### *startIndex*
Type: [System.Int32][2]  
The index from which to start removing elements

##### *length*
Type: [System.Int32][2]  
The number of elements to remove

#### Type Parameters

##### *T*
The data type

#### Return Value
Type: **T**[]  
The modified source array

See Also
--------

#### Reference
[ArrayMethods Class][3]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/td2s409d
[3]: README.md