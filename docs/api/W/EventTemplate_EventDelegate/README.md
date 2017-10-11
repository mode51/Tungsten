EventTemplate.EventDelegate Delegate
====================================
   The template delegate

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public delegate void EventDelegate(
	Object sender,
	string callerMemberName,
	params Object[] args
)
```

#### Parameters

##### *sender*
Type: [System.Object][2]  
The object which raised this event

##### *callerMemberName*
Type: [System.String][3]  
The name of the method which raised the event

##### *args*
Type: [System.Object][2][]  
An array of untyped arguments


See Also
--------

#### Reference
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: http://msdn.microsoft.com/en-us/library/s1wwdcbf