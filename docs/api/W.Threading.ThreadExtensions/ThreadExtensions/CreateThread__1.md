ThreadExtensions.CreateThread&lt;TParameterType> Method (Object, Action&lt;TParameterType, CancellationToken>)
==============================================================================================================
   Creates and starts a new thread and

  **Namespace:**  [W.Threading.ThreadExtensions][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Thread<TParameterType> CreateThread<TParameterType>(
	this Object this,
	Action<TParameterType, CancellationToken> action
)

```

#### Parameters

##### *this*
Type: [System.Object][2]  
The object to send into a new Thread

##### *action*
Type: [System.Action][3]&lt;**TParameterType**, [CancellationToken][4]>  
Action to call on a thread

#### Type Parameters

##### *TParameterType*
The Type of the item being extended

#### Return Value
Type: [Thread][5]&lt;**TParameterType**>  
A reference to the new W.Threading.Thread&lt;T>
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [Object][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][6] or [Extension Methods (C# Programming Guide)][7].

See Also
--------

#### Reference
[ThreadExtensions Class][8]  
[W.Threading.ThreadExtensions Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/bb549311
[4]: http://msdn.microsoft.com/en-us/library/dd384802
[5]: ../../W.Threading/Thread_1/README.md
[6]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[7]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[8]: README.md