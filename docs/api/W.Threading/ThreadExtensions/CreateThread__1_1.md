ThreadExtensions.CreateThread&lt;T> Method (T, Action&lt;T, CancellationTokenSource>, Action&lt;Boolean, Exception>)
====================================================================================================================
  Starts a new thread

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Thread<T> CreateThread<T>(
	this T this,
	Action<T, CancellationTokenSource> action,
	Action<bool, Exception> onComplete
)

```

#### Parameters

##### *this*
Type: **T**  
The object to send into a new Thread

##### *action*
Type: [System.Action][2]&lt;**T**, [CancellationTokenSource][3]>  
Action to call on a thread

##### *onComplete*
Type: [System.Action][2]&lt;[Boolean][4], [Exception][5]>  
Action to call upon comletion. Executes on the same thread as Action.

#### Type Parameters

##### *T*

[Missing &lt;typeparam name="T"/> documentation for "M:W.Threading.ThreadExtensions.CreateThread``1(``0,System.Action{``0,System.Threading.CancellationTokenSource},System.Action{System.Boolean,System.Exception})"]


#### Return Value
Type: [Thread][6]&lt;**T**>  
A reference to the new W.Threading.Thread&lt;T>
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type . When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][7] or [Extension Methods (C# Programming Guide)][8].

See Also
--------

#### Reference
[ThreadExtensions Class][9]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: http://msdn.microsoft.com/en-us/library/dd321629
[4]: http://msdn.microsoft.com/en-us/library/a28wyd50
[5]: http://msdn.microsoft.com/en-us/library/c18k6c59
[6]: ../Thread_1/README.md
[7]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[8]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[9]: README.md