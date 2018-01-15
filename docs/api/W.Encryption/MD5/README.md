MD5 Class
=========
   Used to generate MD5 hashes and verify input strings against them


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Encryption.MD5**  

  **Namespace:**  [W.Encryption][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class MD5
```

The **MD5** type exposes the following members.


Constructors
------------

                 | Name     | Description                                     
---------------- | -------- | ----------------------------------------------- 
![Public method] | [MD5][3] | Initializes a new instance of the **MD5** class 


Methods
-------

                                 | Name                         | Description                               
-------------------------------- | ---------------------------- | ----------------------------------------- 
![Public method]                 | [Equals][4]                  | (Inherited from [Object][1].)             
![Protected method]              | [Finalize][5]                | (Inherited from [Object][1].)             
![Public method]                 | [GetHashCode][6]             | (Inherited from [Object][1].)             
![Public method]![Static member] | [GetMd5Hash(String)][7]      | Generates an MD5 hash of the input string 
![Public method]![Static member] | [GetMd5Hash(String, MD5)][8] | Generates an MD5 hash of the input string 
![Public method]                 | [GetType][9]                 | (Inherited from [Object][1].)             
![Protected method]              | [MemberwiseClone][10]        | (Inherited from [Object][1].)             
![Public method]                 | [ToString][11]               | (Inherited from [Object][1].)             
![Public method]![Static member] | [VerifyMd5Hash][12]          | Verifies a hash against a string          


See Also
--------

#### Reference
[W.Encryption Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[5]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[6]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[7]: GetMd5Hash.md
[8]: GetMd5Hash_1.md
[9]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[10]: http://msdn.microsoft.com/en-us/library/57ctke0a
[11]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[12]: VerifyMd5Hash.md
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Static member]: ../../_icons/static.gif "Static member"