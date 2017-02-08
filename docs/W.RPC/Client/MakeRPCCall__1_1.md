Client.MakeRPCCall&lt;T> Method (String, Action&lt;T>, Object[])
================================================================
  Calls a method on the Tungsten RPC Server

  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public ManualResetEvent MakeRPCCall<T>(
	string methodName,
	Action<T> onResponse,
	params Object[] args
)

```

#### Parameters

##### *methodName*
Type: [System.String][2]  
The name of the method to call, including full namespace and class name

##### *onResponse*
Type: [System.Action][3]&lt;**T**>  
A callback where

##### *args*
Type: [System.Object][4][]  
Optional parameters to be passed into the method

#### Type Parameters

##### *T*
The result from the call

#### Return Value
Type: [ManualResetEvent][5]  
A ManualResetEvent which can be joined (with or without a timeout) to block the calling thread until a respoonse is received.

See Also
--------

#### Reference
[Client Class][6]  
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/018hxwa8
[4]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[5]: http://msdn.microsoft.com/en-us/library/2ssskfws
[6]: README.md
[7]: ../../_icons/Help.png