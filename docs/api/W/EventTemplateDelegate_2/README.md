EventTemplateDelegate&lt;TEventArg1, TEventArg2> Delegate
=========================================================
   The template delegate

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate void EventTemplateDelegate<TEventArg1, TEventArg2>(
	Object sender,
	TEventArg1 arg1,
	TEventArg2 arg2,
	string callerMemberName
)

```

#### Parameters

##### *sender*
Type: [System.Object][2]  
The object which raised this event

##### *arg1*
Type: **TEventArg1**  
The first detailed event argument

##### *arg2*
Type: **TEventArg2**  
The second detailed event argument

##### *callerMemberName*
Type: [System.String][3]  
The name of the method which raised the event

#### Type Parameters

##### *TEventArg1*

[Missing &lt;typeparam name="TEventArg1"/> documentation for "T:W.EventTemplateDelegate`2"]


##### *TEventArg2*

[Missing &lt;typeparam name="TEventArg2"/> documentation for "T:W.EventTemplateDelegate`2"]



See Also
--------

#### Reference
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/s1wwdcbf