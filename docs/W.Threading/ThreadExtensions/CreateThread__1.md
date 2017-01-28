ThreadExtensions.CreateThread&lt;T> Method (Object, Action&lt;T, CancellationTokenSource>, Action&lt;Boolean, Exception>, T)
============================================================================================================================
  Starts a new thread

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Thread<T> CreateThread<T>(
	this Object this,
	Action<T, CancellationTokenSource> action,
	Action<bool, Exception> onComplete,
	T customData
)

```

#### Parameters

##### *this*
Type: [System.Object][2]  

[Missing &lt;param name="this"/> documentation for "M:W.Threading.ThreadExtensions.CreateThread``1(System.Object,System.Action{``0,System.Threading.CancellationTokenSource},System.Action{System.Boolean,System.Exception},``0)"]


##### *action*
Type: [System.Action][3]&lt;**T**, [CancellationTokenSource][4]>  
Action to call on a thread

##### *onComplete*
Type: [System.Action][3]&lt;[Boolean][5], [Exception][6]>  
Action to call upon comletion. Executes on the same thread as Action.

##### *customData*
Type: **T**  
The data to pass to the thread (Action)

#### Type Parameters

##### *T*

[Missing &lt;typeparam name="T"/> documentation for "M:W.Threading.ThreadExtensions.CreateThread``1(System.Object,System.Action{``0,System.Threading.CancellationTokenSource},System.Action{System.Boolean,System.Exception},``0)"]


#### Return Value
Type: [Thread][7]&lt;**T**>  
A reference to the new W.Threading.Thread&lt;T>
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [Object][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][8] or [Extension Methods (C# Programming Guide)][9].

See Also
--------

#### Reference
[ThreadExtensions Class][10]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/bb549311
[4]: http://msdn.microsoft.com/en-us/library/dd321629
[5]: http://msdn.microsoft.com/en-us/library/a28wyd50
[6]: http://msdn.microsoft.com/en-us/library/c18k6c59
[7]: ../Thread_1/README.md
[8]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[9]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[10]: README.md
[11]: ../../_icons/Help.png