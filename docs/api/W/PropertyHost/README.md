PropertyHost Class
==================
   
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class does not support INotifyPropertyChanged and is not intented to host owned properties (though nothing prevents you from doing so)



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.PropertyHost**  
    [W.Logging.Log.LogMessageHistory][2]  

  **Namespace:**  [W][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class PropertyHost
```

The **PropertyHost** type exposes the following members.


Constructors
------------

                 | Name              | Description                                                         
---------------- | ----------------- | ------------------------------------------------------------------- 
![Public method] | [PropertyHost][4] | Calls PropertyHostMethods.InitializeProperties so you don't have to 


Properties
----------

                   | Name         | Description                                        
------------------ | ------------ | -------------------------------------------------- 
![Public property] | [IsDirty][5] | Finds all Properties and checks their IsDirty flag 


Methods
-------

                    | Name                  | Description                                                                                 
------------------- | --------------------- | ------------------------------------------------------------------------------------------- 
![Public method]    | [Equals][6]           | (Inherited from [Object][1].)                                                               
![Protected method] | [Finalize][7]         | (Inherited from [Object][1].)                                                               
![Public method]    | [GetHashCode][8]      | (Inherited from [Object][1].)                                                               
![Public method]    | [GetType][9]          | (Inherited from [Object][1].)                                                               
![Public method]    | [MarkAsClean][10]     | Uses reflection to find all Properties and mark them as clean (call Property.MarkAsClean()) 
![Protected method] | [MemberwiseClone][11] | (Inherited from [Object][1].)                                                               
![Public method]    | [ToString][12]        | (Inherited from [Object][1].)                                                               


See Also
--------

#### Reference
[W Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../../W.Logging/Log_LogMessageHistory/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: IsDirty.md
[6]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[7]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[8]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[9]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[10]: MarkAsClean.md
[11]: http://msdn.microsoft.com/en-us/library/57ctke0a
[12]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"