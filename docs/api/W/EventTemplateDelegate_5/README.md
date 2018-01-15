EventTemplateDelegate&lt;TEventArg1, TEventArg2, TEventArg3, TEventArg4, TEventArg5> Delegate
=============================================================================================
   The template delegate

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate void EventTemplateDelegate<TEventArg1, TEventArg2, TEventArg3, TEventArg4, TEventArg5>(
	Object sender,
	TEventArg1 arg1,
	TEventArg2 arg2,
	TEventArg3 arg3,
	TEventArg4 arg4,
	TEventArg5 arg5,
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

##### *arg3*
Type: **TEventArg3**  
The third detailed event argument

##### *arg4*
Type: **TEventArg4**  
The fourth detailed event argument

##### *arg5*
Type: **TEventArg5**  
The fifth detailed event argument

##### *callerMemberName*
Type: [System.String][3]  
The name of the method which raised the event

#### Type Parameters

##### *TEventArg1*

[Missing &lt;typeparam name="TEventArg1"/> documentation for "T:W.EventTemplateDelegate`5"]


##### *TEventArg2*

[Missing &lt;typeparam name="TEventArg2"/> documentation for "T:W.EventTemplateDelegate`5"]


##### *TEventArg3*

[Missing &lt;typeparam name="TEventArg3"/> documentation for "T:W.EventTemplateDelegate`5"]


##### *TEventArg4*

[Missing &lt;typeparam name="TEventArg4"/> documentation for "T:W.EventTemplateDelegate`5"]


##### *TEventArg5*

[Missing &lt;typeparam name="TEventArg5"/> documentation for "T:W.EventTemplateDelegate`5"]



See Also
--------

#### Reference
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/s1wwdcbf