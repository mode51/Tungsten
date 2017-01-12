# Tungsten
A C# library to make Windows Forms and WPF application development easier.  See the Wiki page for details and use.

Tungsten is built with .Net Framework 4.5

# Classes
* [Lockable](https://github.com/mode51/Tungsten/wiki/Lockable-TValue-)\<TValue\> - A generic class to wrap a value in a thread-safe manner
* [Property](https://github.com/mode51/Tungsten/wiki/Property-TValue)\<TValue\> - A generic class providing a number of enhancements to a [Lockable](https://github.com/mode51/Tungsten/wiki/Lockable-TValue-)\<TValue\> value
    * Implements INotifyPropertyChanged
    * Adds IsDirty/MarkAsClean functionality
    * Supports a callback handler in the constructor so you don't have to use events
* [Property](https://github.com/mode51/Tungsten/wiki/Property-TOwner,-TValue)\<TOwner, TValue\> - Like Property\<TValue\> except that you can specify an Owner
    * Events and callback have type-strict sender (which is the Owner you specify)
* [PropertyHost](https://github.com/mode51/Tungsten/wiki/PropertyHost) - a base class which automates the IsDirty, MarkAsClean and InitializeProperties so you don't have to.
