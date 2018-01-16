MonitorLocker.InLockAsync Method (Action)
=========================================
   Executes an action from within a Monitor

  **Namespace:**  [W.Threading.Lockers][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public Task InLockAsync(
	Action action
)
```

#### Parameters

##### *action*
Type: [System.Action][2]  
The action to run

#### Return Value
Type: [Task][3]  

[Missing &lt;returns> documentation for "M:W.Threading.Lockers.MonitorLocker.InLockAsync(System.Action)"]

#### Implements
[ILocker.InLockAsync(Action)][4]  


See Also
--------

#### Reference
[MonitorLocker Class][5]  
[W.Threading.Lockers Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb534741
[3]: http://msdn.microsoft.com/en-us/library/dd235678
[4]: ../ILocker/InLockAsync.md
[5]: README.md