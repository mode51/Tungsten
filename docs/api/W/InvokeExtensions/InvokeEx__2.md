InvokeExtensions.InvokeEx&lt;T, U> Method (T, Func&lt;T, U>)
============================================================
   Runs the provided Function on the UI thread. Avoids the cross-threaded exceptions.

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static U InvokeEx<T, U>(
	this T this,
	Func<T, U> f
)
where T : ISynchronizeInvoke

```

#### Parameters

##### *this*
Type: **T**  
The form or control which supports ISynchronizationInvoke

##### *f*
Type: [System.Func][2]&lt;**T**, **U**>  
The function to be executed on the UI thread

#### Type Parameters

##### *T*
The form or control who's thread will execute the code

##### *U*
The type of return value

#### Return Value
Type: **U**  
The function should return an object of type U
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type . When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][3] or [Extension Methods (C# Programming Guide)][4].

See Also
--------

#### Reference
[InvokeExtensions Class][5]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549151
[3]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[4]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[5]: README.md