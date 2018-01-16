ArrayMethods.Insert&lt;T> Method
================================
   Appends the items to an array, resizing the array as necessary

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static T[] Insert<T>(
	ref T[] source,
	T[] itemsToInsert,
	int index
)

```

#### Parameters

##### *source*
Type: **T**[]  
The source array

##### *itemsToInsert*
Type: **T**[]  
The array of items to append to the source

##### *index*
Type: [System.Int32][2]  
The index where the items should be inserted

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