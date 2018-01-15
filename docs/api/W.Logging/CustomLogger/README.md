CustomLogger Class
==================
   Allows the programmer to add a custom message logger


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Logging.CustomLogger**  

  **Namespace:**  [W.Logging][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class CustomLogger : IDisposable
```

The **CustomLogger** type exposes the following members.


Constructors
------------

                 | Name              | Description                   
---------------- | ----------------- | ----------------------------- 
![Public method] | [CustomLogger][3] | Constructs a new CustomLogger 


Properties
----------

                      | Name              | Description                                                                           
--------------------- | ----------------- | ------------------------------------------------------------------------------------- 
![Public property]    | [AddTimestamp][4] | If true, FormatLogMessage will, by default, add a timestamp prefix to the log message 
![Protected property] | [IsDisposed][5]   | True if OnDispose has been called                                                     
![Public property]    | [Name][6]         | The name of this custom logger                                                        


Methods
-------

                    | Name                   | Description                                                                                
------------------- | ---------------------- | ------------------------------------------------------------------------------------------ 
![Public method]    | [Dispose][7]           | Disposes the CustomLogger and releases resources                                           
![Public method]    | [Equals][8]            | (Inherited from [Object][1].)                                                              
![Protected method] | [Finalize][9]          | (Inherited from [Object][1].)                                                              
![Protected method] | [FormatLogMessage][10] | Formats the Log Messge (if AddTimestamp is true, the message is prefixed with a timestamp) 
![Public method]    | [GetHashCode][11]      | (Inherited from [Object][1].)                                                              
![Public method]    | [GetType][12]          | (Inherited from [Object][1].)                                                              
![Protected method] | [LogMessage][13]       | Log a message to the custom logger                                                         
![Protected method] | [MemberwiseClone][14]  | (Inherited from [Object][1].)                                                              
![Protected method] | [OnDispose][15]        | Disposes the CustomLogger, releases resources and supresses the finalizer                  
![Public method]    | [ToString][16]         | (Inherited from [Object][1].)                                                              


See Also
--------

#### Reference
[W.Logging Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: AddTimestamp.md
[5]: IsDisposed.md
[6]: Name.md
[7]: Dispose.md
[8]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[9]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[10]: FormatLogMessage.md
[11]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[12]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[13]: LogMessage.md
[14]: http://msdn.microsoft.com/en-us/library/57ctke0a
[15]: OnDispose.md
[16]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected property]: ../../_icons/protproperty.gif "Protected property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"