EventTemplate&lt;TEventArg1, TEventArg2, TEventArg3, TEventArg4, TEventArg5>.EventDelegate Delegate
===================================================================================================
   The template delegate

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate void EventDelegate(
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
Type: [TEventArg1][3]  
The first detailed event argument

##### *arg2*
Type: [TEventArg2][3]  
The second detailed event argument

##### *arg3*
Type: [TEventArg3][3]  
The third detailed event argument

##### *arg4*
Type: [TEventArg4][3]  
The fourth detailed event argument

##### *arg5*
Type: [TEventArg5][3]  
The fifth detailed event argument

##### *callerMemberName*
Type: [System.String][4]  
The name of the method which raised the event


See Also
--------

#### Reference
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: ../EventTemplate_5/README.md
[4]: http://msdn.microsoft.com/en-us/library/s1wwdcbf