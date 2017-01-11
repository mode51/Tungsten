# Tungsten
A C# library to make application development easier.  See the Wiki page for details and use.

# Classes
* Lockable\<TValue\> - A generic class to wrap a value in a thread-safe manner
* Property\<TValue\> - A generic class providing a number of enhancements to a Lockable\<TValue\> value
    * Implements INotifyPropertyChanged
    * Adds IsDirty/MarkAsClean functionality
    * Supports a callback handler in the constructor so you don't have to use events
* Property\<TOwner, TValue\> - Like Property\<TValue\> except that you can specify an Owner
    * Events and callback have type-strict sender (which is the Owner you specify)
    
