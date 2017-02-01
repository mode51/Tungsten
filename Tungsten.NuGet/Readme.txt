Tungsten is a C# library to make application development easier.  See the wiki @ https://github.com/mode51/Tungsten/wiki for details, examples and use.

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

1/13/2017
Added ThreadExtensions

1/12/2017

Added Invoke (InvokeExtensions)
Added CallResult and CallResult<TResult>
Added CallResult tests


Initial Release 1/11/2017

Lockable<TValue>
Property<TValue>
Property<TOwner, TValue>
PropertyHost

Thanks for using Tungsten,

Jordan Duerksen
