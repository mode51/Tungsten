CustomLogger.FormatLogMessage Method
====================================
   Formats the Log Messge (if AddTimestamp is true, the message is prefixed with a timestamp)

  **Namespace:**  [W.Logging][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
protected virtual string FormatLogMessage(
	Log.LogMessageCategory category,
	string message
)
```

#### Parameters

##### *category*
Type: [W.Logging.Log.LogMessageCategory][2]  
The log message category

##### *message*
Type: [System.String][3]  
The log message

#### Return Value
Type: [String][3]  
The formatted log message

See Also
--------

#### Reference
[CustomLogger Class][4]  
[W.Logging Namespace][1]  

[1]: ../README.md
[2]: ../Log_LogMessageCategory/README.md
[3]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[4]: README.md