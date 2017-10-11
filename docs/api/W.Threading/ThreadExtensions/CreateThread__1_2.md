ThreadExtensions.CreateThread&lt;TParameterType> Method (TParameterType, Action&lt;TParameterType, CancellationToken>)
======================================================================================================================
   Creates and starts a new thread

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Thread<TParameterType> CreateThread<TParameterType>(
	this TParameterType this,
	Action<TParameterType, CancellationToken> action
)

```

#### Parameters

##### *this*
Type: **TParameterType**  
The object to send into a new Thread

##### *action*
Type: [System.Action][2]&lt;**TParameterType**, [CancellationToken][3]>  
Action to call on a thread

#### Type Parameters

##### *TParameterType*
The Type of the item being extended

#### Return Value
Type: [Thread][4]&lt;**TParameterType**>  
A reference to the new W.Threading.Thread&lt;T>
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type . When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][5] or [Extension Methods (C# Programming Guide)][6].

See Also
--------

#### Reference
[ThreadExtensions Class][7]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: http://msdn.microsoft.com/en-us/library/dd384802
[4]: ../Thread_1/README.md
[5]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[6]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[7]: README.md