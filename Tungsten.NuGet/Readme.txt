Tungsten is a C# library to make application development easier.  See the wiki @ https://github.com/mode51/Tungsten/wiki for details, examples and use.

2.28.2017 v1.1.4
Had to remove Tungsten.Core from this package because NuProj doesn't support it yet

2.28.2017 v1.1.3
Merged the Tungsten, Tungsten.Portable, Tungsten.Universal and Tungsten.Core NuGet packages

2.16.2017 v1.1.2
Fixed AsJson extension method

2.16.2017 v1.1.1
Fixed As<TType> and AsJson<TType>

2.16.2017
Added As.cs which contains a number of extension methods for converting objects from one type to another

2.9.2017
Removed Tungsten.Threading.GateExtensions - the methods seem rather redundant or at least unnecessary

2.1.2017
Removed unused references

1.29.2017
PropertyHostMethods now actually exposes extension methods (forgot to make them extensions)

1.27.2017
Added PropertyHostNotifier which aggregates the functionality of PropertyChangedNotifier and PropertyHost
Fixed Property so it actually updates the UI automatically (Owner functionality wasn't working)

1.21.2017
Added PropertyBase.WaitForChanged

1.16.2017
Added W.ActionQueue<T>
Added W.Threading.Gate/Gate<T>
Added W.Threading.Thread/Thread<T>
Updated ThreadExtensions

1.13.2017
Added ThreadExtensions

1.12.2017

Added Invoke (InvokeExtensions)
Added CallResult and CallResult<TResult>
Added CallResult tests


Initial Release 1.11.2017

Lockable<TValue>
Property<TValue>
Property<TOwner, TValue>
PropertyHost

Thanks for using Tungsten,

Jordan Duerksen
