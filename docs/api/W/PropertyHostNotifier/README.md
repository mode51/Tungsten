PropertyHostNotifier Class
==========================
   
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class inherits PropertyChangedNotifier for INotifyPropertyChanged support



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.PropertyChangedNotifier][2]  
    **W.PropertyHostNotifier**  

  **Namespace:**  [W][3]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class PropertyHostNotifier : PropertyChangedNotifier
```

The **PropertyHostNotifier** type exposes the following members.


Constructors
------------

                 | Name                      | Description                                                         
---------------- | ------------------------- | ------------------------------------------------------------------- 
![Public method] | [PropertyHostNotifier][4] | Calls PropertyHostMethods.InitializeProperties so you don't have to 


Properties
----------

                   | Name         | Description                                        
------------------ | ------------ | -------------------------------------------------- 
![Public property] | [IsDirty][5] | Finds all Properties and checks their IsDirty flag 


Methods
-------

                    | Name                         | Description                                                                                                                                              
------------------- | ---------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public method]    | [Equals][6]                  | (Inherited from [Object][1].)                                                                                                                            
![Protected method] | [Finalize][7]                | (Inherited from [Object][1].)                                                                                                                            
![Public method]    | [GetHashCode][8]             | (Inherited from [Object][1].)                                                                                                                            
![Public method]    | [GetType][9]                 | (Inherited from [Object][1].)                                                                                                                            
![Protected method] | [GetValue][10]               | 
Override this method to provide Get functionality
 (Inherited from [PropertyChangedNotifier][2].)                                                     
![Public method]    | [MarkAsClean][11]            | Uses reflection to find all Properties and mark them as clean (call Property.MarkAsClean())                                                              
![Protected method] | [MemberwiseClone][12]        | (Inherited from [Object][1].)                                                                                                                            
![Protected method] | [OnPropertyChanged][13]      | 
Calls RaisePropertyChanged to raise the PropertyChanged event
 (Inherited from [PropertyChangedNotifier][2].)                                         
![Protected method] | [RaiseOnPropertyChanged][14] | 
Raises the PropertyChanged event
 (Inherited from [PropertyChangedNotifier][2].)                                                                      
![Protected method] | [SetValue][15]               | 
Calls OnPropertyChanged. This method does not make assignments. Override this method to make assignments.
 (Inherited from [PropertyChangedNotifier][2].) 
![Public method]    | [ToString][16]               | (Inherited from [Object][1].)                                                                                                                            


Events
------

                | Name                  | Description                                                                   
--------------- | --------------------- | ----------------------------------------------------------------------------- 
![Public event] | [PropertyChanged][17] | Raised when a property changes (Inherited from [PropertyChangedNotifier][2].) 


See Also
--------

#### Reference
[W Namespace][3]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../PropertyChangedNotifier/README.md
[3]: ../README.md
[4]: _ctor.md
[5]: IsDirty.md
[6]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[7]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[8]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[9]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[10]: ../PropertyChangedNotifier/GetValue.md
[11]: MarkAsClean.md
[12]: http://msdn.microsoft.com/en-us/library/57ctke0a
[13]: ../PropertyChangedNotifier/OnPropertyChanged.md
[14]: ../PropertyChangedNotifier/RaiseOnPropertyChanged.md
[15]: ../PropertyChangedNotifier/SetValue.md
[16]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[17]: ../PropertyChangedNotifier/PropertyChanged.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public event]: ../../_icons/pubevent.gif "Public event"