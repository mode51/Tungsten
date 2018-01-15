Log.LogMessageHistory Class
===========================
   Maintains a history of Log information


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.PropertyHost][2]  
    **W.Logging.Log.LogMessageHistory**  

  **Namespace:**  [W.Logging][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class LogMessageHistory : PropertyHost
```

The **Log.LogMessageHistory** type exposes the following members.


Constructors
------------

                 | Name                       | Description                        
---------------- | -------------------------- | ---------------------------------- 
![Public method] | [Log.LogMessageHistory][4] | Constructs a new LogMessageHistory 


Properties
----------

                   | Name                         | Description                                                                                                                    
------------------ | ---------------------------- | ------------------------------------------------------------------------------------------------------------------------------ 
![Public property] | [Enabled][5]                 | If True, log messages will be added to the history. If False, no history is maintained.                                        
![Public property] | [IsDirty][6]                 | Finds all Properties and checks their IsDirty flag (Inherited from [PropertyHost][2].)                                         
![Public property] | [MaximumNumberOfMessages][7] | The maximum number of historical messages to maintain. When the maximum is reached, the oldest messages are removed as needed. 
![Public property] | [Messages][8]                | The history of log messages                                                                                                    


Methods
-------

                    | Name                  | Description                                                                                                                     
------------------- | --------------------- | ------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Equals][9]           | (Inherited from [Object][1].)                                                                                                   
![Protected method] | [Finalize][10]        | (Inherited from [Object][1].)                                                                                                   
![Public method]    | [GetHashCode][11]     | (Inherited from [Object][1].)                                                                                                   
![Public method]    | [GetType][12]         | (Inherited from [Object][1].)                                                                                                   
![Public method]    | [MarkAsClean][13]     | Uses reflection to find all Properties and mark them as clean (call Property.MarkAsClean()) (Inherited from [PropertyHost][2].) 
![Protected method] | [MemberwiseClone][14] | (Inherited from [Object][1].)                                                                                                   
![Public method]    | [ToString][15]        | (Inherited from [Object][1].)                                                                                                   


See Also
--------

#### Reference
[W.Logging Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../../W/PropertyHost/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: Enabled.md
[6]: ../../W/PropertyHost/IsDirty.md
[7]: MaximumNumberOfMessages.md
[8]: Messages.md
[9]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[10]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[11]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[12]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[13]: ../../W/PropertyHost/MarkAsClean.md
[14]: http://msdn.microsoft.com/en-us/library/57ctke0a
[15]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"