EventTemplateDelegate&lt;TEventArg> Delegate
============================================
   The template delegate

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate void EventTemplateDelegate<TEventArg>(
	Object sender,
	TEventArg arg,
	string callerMemberName
)

```

#### Parameters

##### *sender*
Type: [System.Object][2]  
The object which raised this event

##### *arg*
Type: **TEventArg**  
The detailed event argument

##### *callerMemberName*
Type: [System.String][3]  
The name of the method which raised the event

#### Type Parameters

##### *TEventArg*

[Missing &lt;typeparam name="TEventArg"/> documentation for "T:W.EventTemplateDelegate`1"]



See Also
--------

#### Reference
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/s1wwdcbf