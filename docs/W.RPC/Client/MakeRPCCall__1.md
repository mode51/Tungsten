Client.MakeRPCCall&lt;T> Method (String, Action&lt;T>)
======================================================
  Calls a method on the Tungsten RPC Server

  **Namespace:**  [W.RPC][1]  
  **Assembly:**  Tungsten.RPC (in Tungsten.RPC.dll)

Syntax
------

```csharp
public ManualResetEvent MakeRPCCall<T>(
	string methodName,
	Action<T> onResponse
)

```

#### Parameters

##### *methodName*
Type: [System.String][2]  
The name of the method to call, including full namespace and class name

##### *onResponse*
Type: [System.Action][3]&lt;**T**>  
A callback where

#### Type Parameters

##### *T*
The result from the call

#### Return Value
Type: [ManualResetEvent][4]  
A ManualResetEvent which can be joined (with or without a timeout) to block the calling thread until a respoonse is received.
#### Implements
[IClient.MakeRPCCall&lt;T>(String, Action&lt;T>)][5]  


See Also
--------

#### Reference
[Client Class][6]  
[W.RPC Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/018hxwa8
[4]: http://msdn.microsoft.com/en-us/library/2ssskfws
[5]: ../IClient/MakeRPCCall__1.md
[6]: README.md
[7]: ../../_icons/Help.png