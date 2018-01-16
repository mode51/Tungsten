PropertyChangedNotifier Class
=============================
   
This is a base class for supporting INotifyPropertyChanged



Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.PropertyChangedNotifier**  
    [W.PropertyHostNotifier][2]  

  **Namespace:**  [W][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public abstract class PropertyChangedNotifier : INotifyPropertyChanged
```

The **PropertyChangedNotifier** type exposes the following members.


Constructors
------------

                    | Name                         | Description                                                         
------------------- | ---------------------------- | ------------------------------------------------------------------- 
![Protected method] | [PropertyChangedNotifier][4] | Initializes a new instance of the **PropertyChangedNotifier** class 


Methods
-------

                    | Name                         | Description                                                                                               
------------------- | ---------------------------- | --------------------------------------------------------------------------------------------------------- 
![Public method]    | [Equals][5]                  | (Inherited from [Object][1].)                                                                             
![Protected method] | [Finalize][6]                | (Inherited from [Object][1].)                                                                             
![Public method]    | [GetHashCode][7]             | (Inherited from [Object][1].)                                                                             
![Public method]    | [GetType][8]                 | (Inherited from [Object][1].)                                                                             
![Protected method] | [GetValue][9]                | 
Override this method to provide Get functionality
                                                     
![Protected method] | [MemberwiseClone][10]        | (Inherited from [Object][1].)                                                                             
![Protected method] | [OnPropertyChanged][11]      | 
Calls RaisePropertyChanged to raise the PropertyChanged event
                                         
![Protected method] | [RaiseOnPropertyChanged][12] | 
Raises the PropertyChanged event
                                                                      
![Protected method] | [SetValue][13]               | 
Calls OnPropertyChanged. This method does not make assignments. Override this method to make assignments.
 
![Public method]    | [ToString][14]               | (Inherited from [Object][1].)                                                                             


Events
------

                | Name                  | Description                    
--------------- | --------------------- | ------------------------------ 
![Public event] | [PropertyChanged][15] | Raised when a property changes 


See Also
--------

#### Reference
[W Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../PropertyHostNotifier/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[6]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[7]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[8]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[9]: GetValue.md
[10]: http://msdn.microsoft.com/en-us/library/57ctke0a
[11]: OnPropertyChanged.md
[12]: RaiseOnPropertyChanged.md
[13]: SetValue.md
[14]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[15]: PropertyChanged.md
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public event]: ../../_icons/pubevent.gif "Public event"