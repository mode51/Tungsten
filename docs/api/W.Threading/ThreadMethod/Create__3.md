ThreadMethod.Create&lt;T1, T2, T3> Method (Action&lt;T1, T2, T3>)
=================================================================
   Creates a new ThreadMethod from the Action

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static ThreadMethod Create<T1, T2, T3>(
	Action<T1, T2, T3> action
)

```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;**T1**, **T2**, **T3**>  
The Action to wrap with an ThreadMethod

#### Type Parameters

##### *T1*

[Missing &lt;typeparam name="T1"/> documentation for "M:W.Threading.ThreadMethod.Create``3(System.Action{``0,``1,``2})"]


##### *T2*

[Missing &lt;typeparam name="T2"/> documentation for "M:W.Threading.ThreadMethod.Create``3(System.Action{``0,``1,``2})"]


##### *T3*

[Missing &lt;typeparam name="T3"/> documentation for "M:W.Threading.ThreadMethod.Create``3(System.Action{``0,``1,``2})"]


#### Return Value
Type: [ThreadMethod][3]  
A new instance of an ThreadMethod

See Also
--------

#### Reference
[ThreadMethod Class][3]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb549392
[3]: README.md