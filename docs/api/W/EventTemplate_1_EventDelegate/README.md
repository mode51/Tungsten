EventTemplate&lt;TEventArg>.EventDelegate Delegate
==================================================
   The template delegate

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate void EventDelegate(
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
Type: [TEventArg][3]  
The detailed event argument

##### *callerMemberName*
Type: [System.String][4]  
The name of the method which raised the event


See Also
--------

#### Reference
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: ../EventTemplate_1/README.md
[4]: http://msdn.microsoft.com/en-us/library/s1wwdcbf