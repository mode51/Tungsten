ThreadExtensions.CreateThread&lt;TParameterType> Method (Object, Action&lt;TParameterType, CancellationToken>, Boolean)
=======================================================================================================================
   Creates a new thread

  **Namespace:**  [W.Threading.ThreadExtensions][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Thread<TParameterType> CreateThread<TParameterType>(
	this Object this,
	Action<TParameterType, CancellationToken> action,
	bool autoStart
)

```

#### Parameters

##### *this*
Type: [System.Object][2]  
The object to send into a new Thread

##### *action*
Type: [System.Action][3]&lt;**TParameterType**, [CancellationToken][4]>  
Action to call on a thread

##### *autoStart*
Type: [System.Boolean][5]  
If True, the thread will immediately start running

#### Type Parameters

##### *TParameterType*
The Type of the item being extended

#### Return Value
Type: [Thread][6]&lt;**TParameterType**>  
A reference to the new W.Threading.Thread&lt;T>
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [Object][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][7] or [Extension Methods (C# Programming Guide)][8].

See Also
--------

#### Reference
[ThreadExtensions Class][9]  
[W.Threading.ThreadExtensions Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/bb549311
[4]: http://msdn.microsoft.com/en-us/library/dd384802
[5]: http://msdn.microsoft.com/en-us/library/a28wyd50
[6]: ../../W.Threading/Thread_1/README.md
[7]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[8]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[9]: README.md