ThreadSlim.Create&lt;TArg> Method (Action&lt;CancellationToken, TArg>)
======================================================================
   Constructs a new ThreadSlim using an Action&lt;CancellationToken> as the thread method

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static ThreadSlim Create<TArg>(
	Action<CancellationToken, TArg> action
)

```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[CancellationToken][3], **TArg**>  
The action to run on a separate thread

#### Type Parameters

##### *TArg*
The Type of the argument to be passed into the thread method

#### Return Value
Type: [ThreadSlim][4]  

[Missing &lt;returns> documentation for "M:W.Threading.ThreadSlim.Create``1(System.Action{System.Threading.CancellationToken,``0})"]


See Also
--------

#### Reference
[ThreadSlim Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549311
[3]: http://msdn.microsoft.com/en-us/library/dd384802
[4]: README.md