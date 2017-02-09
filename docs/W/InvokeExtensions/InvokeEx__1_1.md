InvokeExtensions.InvokeEx&lt;T> Method (T, Func&lt;T, Object>)
==============================================================
  Runs the provided Function on the UI thread. Avoids the cross-threaded exceptions.

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Object InvokeEx<T>(
	this T this,
	Func<T, Object> f
)
where T : ISynchronizeInvoke

```

#### Parameters

##### *this*
Type: **T**  
The form or control which supports ISynchronizationInvoke

##### *f*
Type: [System.Func][2]&lt;**T**, [Object][3]>  
The function to be executed on the UI thread

#### Type Parameters

##### *T*
The form or control who's thread will execute the code

#### Return Value
Type: [Object][3]  
The function should return an object
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type . When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][4] or [Extension Methods (C# Programming Guide)][5].

See Also
--------

#### Reference
[InvokeExtensions Class][6]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549151
[3]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[4]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[5]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[6]: README.md
[7]: ../../_icons/Help.png