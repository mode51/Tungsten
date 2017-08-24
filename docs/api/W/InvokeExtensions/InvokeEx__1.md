InvokeExtensions.InvokeEx&lt;T> Method (T, Action&lt;T>)
========================================================
  Runs the provided Action on the UI thread

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static void InvokeEx<T>(
	this T this,
	Action<T> action
)
where T : ISynchronizeInvoke

```

#### Parameters

##### *this*
Type: **T**  
The form or control which supports ISynchronizeInvoke

##### *action*
Type: [System.Action][2]&lt;**T**>  
The code to be executed on the UI thread

#### Type Parameters

##### *T*
The form or control who's thread will execute the code

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type . When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][3] or [Extension Methods (C# Programming Guide)][4].

See Also
--------

#### Reference
[InvokeExtensions Class][5]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/018hxwa8
[3]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[4]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[5]: README.md