ReaderWriterLocker.InLock&lt;TValue> Method (LockTypeEnum, Func&lt;TValue>)
===========================================================================
   Executes a function from within a ReaderWriterLockSlim

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public TValue InLock<TValue>(
	LockTypeEnum lockType,
	Func<TValue> func
)

```

#### Parameters

##### *lockType*
Type: [W.Threading.Lockers.LockTypeEnum][2]  

[Missing &lt;param name="lockType"/> documentation for "M:W.Threading.Lockers.ReaderWriterLocker.InLock``1(W.Threading.Lockers.LockTypeEnum,System.Func{``0})"]


##### *func*
Type: [System.Func][3]&lt;**TValue**>  
The function to run

#### Type Parameters

##### *TValue*
The type of return value

#### Return Value
Type: **TValue**  
The result of the function call (a value of type TValue)

See Also
--------

#### Reference
[ReaderWriterLocker Class][4]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: ../LockTypeEnum/README.md
[3]: http://msdn.microsoft.com/en-us/library/bb534960
[4]: README.md