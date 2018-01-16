SpinLocker.InLock Method (Action)
=================================
   Performs an action from within a SpinLock

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public void InLock(
	Action action
)
```

#### Parameters

##### *action*
Type: [System.Action][2]  
The action to run

#### Implements
[ILocker.InLock(Action)][3]  


See Also
--------

#### Reference
[SpinLocker Class][4]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb534741
[3]: ../ILocker/InLock.md
[4]: README.md