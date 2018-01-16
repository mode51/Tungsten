ReaderWriterLocker.InLock&lt;TResult> Method (Func&lt;TResult>)
===============================================================
   Performs the function in a read lock

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public TResult InLock<TResult>(
	Func<TResult> func
)

```

#### Parameters

##### *func*
Type: [System.Func][2]&lt;**TResult**>  
The function to perform

#### Type Parameters

##### *TResult*

[Missing &lt;typeparam name="TResult"/> documentation for "M:W.Threading.Lockers.ReaderWriterLocker.InLock``1(System.Func{``0})"]


#### Return Value
Type: **TResult**  

[Missing &lt;returns> documentation for "M:W.Threading.Lockers.ReaderWriterLocker.InLock``1(System.Func{``0})"]

#### Implements
[ILocker.InLock&lt;TResult>(Func&lt;TResult>)][3]  


See Also
--------

#### Reference
[ReaderWriterLocker Class][4]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb534960
[3]: ../ILocker/InLock__1.md
[4]: README.md