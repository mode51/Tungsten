LiteDbMethods.LiteDbAction&lt;T, U> Method (String, Func&lt;LiteDatabase, U>, String, String, Int32)
====================================================================================================
  
[Missing &lt;summary> documentation for "M:W.LiteDb.LiteDbMethods.LiteDbAction``2(System.String,System.Func{LiteDB.LiteDatabase,``1},System.String,System.String,System.Int32)"]


  **Namespace:**  [W.LiteDb][1]  
  **Assembly:**  Tungsten.LiteDb (in Tungsten.LiteDb.dll)

Syntax
------

```csharp
public static CallResult<U> LiteDbAction<T, U>(
	string path,
	Func<LiteDatabase, U> f,
	string callerMemberName = "",
	string callerFilePath = "",
	int callerLineNumber = 0
)
where T : new()

```

#### Parameters

##### *path*
Type: [System.String][2]  

[Missing &lt;param name="path"/> documentation for "M:W.LiteDb.LiteDbMethods.LiteDbAction``2(System.String,System.Func{LiteDB.LiteDatabase,``1},System.String,System.String,System.Int32)"]


##### *f*
Type: [System.Func][3]&lt;LiteDatabase, **U**>  

[Missing &lt;param name="f"/> documentation for "M:W.LiteDb.LiteDbMethods.LiteDbAction``2(System.String,System.Func{LiteDB.LiteDatabase,``1},System.String,System.String,System.Int32)"]


##### *callerMemberName* (Optional)
Type: [System.String][2]  

[Missing &lt;param name="callerMemberName"/> documentation for "M:W.LiteDb.LiteDbMethods.LiteDbAction``2(System.String,System.Func{LiteDB.LiteDatabase,``1},System.String,System.String,System.Int32)"]


##### *callerFilePath* (Optional)
Type: [System.String][2]  

[Missing &lt;param name="callerFilePath"/> documentation for "M:W.LiteDb.LiteDbMethods.LiteDbAction``2(System.String,System.Func{LiteDB.LiteDatabase,``1},System.String,System.String,System.Int32)"]


##### *callerLineNumber* (Optional)
Type: [System.Int32][4]  

[Missing &lt;param name="callerLineNumber"/> documentation for "M:W.LiteDb.LiteDbMethods.LiteDbAction``2(System.String,System.Func{LiteDB.LiteDatabase,``1},System.String,System.String,System.Int32)"]


#### Type Parameters

##### *T*

[Missing &lt;typeparam name="T"/> documentation for "M:W.LiteDb.LiteDbMethods.LiteDbAction``2(System.String,System.Func{LiteDB.LiteDatabase,``1},System.String,System.String,System.Int32)"]


##### *U*

[Missing &lt;typeparam name="U"/> documentation for "M:W.LiteDb.LiteDbMethods.LiteDbAction``2(System.String,System.Func{LiteDB.LiteDatabase,``1},System.String,System.String,System.Int32)"]


#### Return Value
Type: [CallResult][5]&lt;**U**>  

[Missing &lt;returns> documentation for "M:W.LiteDb.LiteDbMethods.LiteDbAction``2(System.String,System.Func{LiteDB.LiteDatabase,``1},System.String,System.String,System.Int32)"]


See Also
--------

#### Reference
[LiteDbMethods Class][6]  
[W.LiteDb Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/bb549151
[4]: http://msdn.microsoft.com/en-us/library/td2s409d
[5]: ../../W/CallResult_1/README.md
[6]: README.md
[7]: ../../_icons/Help.png