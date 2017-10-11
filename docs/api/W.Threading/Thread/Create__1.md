Thread.Create&lt;TParameterType> Method (Action&lt;TParameterType, CancellationToken>)
======================================================================================
   Creates and starts a new thread

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Thread<TParameterType> Create<TParameterType>(
	Action<TParameterType, CancellationToken> action
)

```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;**TParameterType**, [CancellationToken][3]>  
Action to call on a thread

#### Type Parameters

##### *TParameterType*
The argument Type to be passed into the thread action

#### Return Value
Type: [Thread][4]&lt;**TParameterType**>  
A new Thread

See Also
--------

#### Reference
[Thread Class][5]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: http://msdn.microsoft.com/en-us/library/dd384802
[4]: ../Thread_1/README.md
[5]: README.md