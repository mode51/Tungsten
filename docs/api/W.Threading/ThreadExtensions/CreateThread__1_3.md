ThreadExtensions.CreateThread&lt;TParameterType> Method (TParameterType, Action&lt;TParameterType, CancellationToken>, Boolean)
===============================================================================================================================
   Creates a new thread

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Thread<TParameterType> CreateThread<TParameterType>(
	this TParameterType this,
	Action<TParameterType, CancellationToken> action,
	bool autoStart
)

```

#### Parameters

##### *this*
Type: **TParameterType**  
The object to send into a new Thread

##### *action*
Type: [System.Action][2]&lt;**TParameterType**, [CancellationToken][3]>  
Action to call on a thread

##### *autoStart*
Type: [System.Boolean][4]  
If True, the thread will immediately start running

#### Type Parameters

##### *TParameterType*
The Type of the item being extended

#### Return Value
Type: [Thread][5]&lt;**TParameterType**>  
A reference to the new W.Threading.Thread&lt;T>
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type . When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][6] or [Extension Methods (C# Programming Guide)][7].

See Also
--------

#### Reference
[ThreadExtensions Class][8]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: http://msdn.microsoft.com/en-us/library/dd384802
[4]: http://msdn.microsoft.com/en-us/library/a28wyd50
[5]: ../Thread_1/README.md
[6]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[7]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[8]: README.md