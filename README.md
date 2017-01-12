#Tungsten

Platforms: WinForms, WPF, Windows Universal

A C# library to make Windows Forms, WPF and Windows Universal application development easier.  See the Wiki page for details and use.

Tungsten is built with .Net Framework 4.5
Tungsten.Universal is built on Windows 10 (10.0; Build 10240)


# Classes
* [Lockable](https://github.com/mode51/Tungsten/wiki/Lockable-TValue-)\<TValue\> - A generic class to wrap a value in a thread-safe manner
* [Property](https://github.com/mode51/Tungsten/wiki/Property-TValue)\<TValue\> - A generic class providing a number of enhancements to a [Lockable](https://github.com/mode51/Tungsten/wiki/Lockable-TValue-)\<TValue\> value
    * Implements INotifyPropertyChanged
    * Adds IsDirty/MarkAsClean functionality
    * Supports a callback handler in the constructor so you don't have to use events
* [Property](https://github.com/mode51/Tungsten/wiki/Property-TOwner,-TValue)\<TOwner, TValue\> - Like Property\<TValue\> except that you can specify an Owner
    * Events and callback have type-strict sender (which is the Owner you specify)
* [PropertyHost](https://github.com/mode51/Tungsten/wiki/PropertyHost) - a base class which automates the IsDirty, MarkAsClean and InitializeProperties so you don't have to.
* CallResult - a non-generic class which can be used to return true/false and an exception from a function
* CallResult\<TResult\> - like CallResult except that you can also specify a result

#Sample 1

If you can inherit from PropertyHost

    public class MyClass : PropertyHost
    {
        public Property<MyClass, string> Last {get; } = new Property<MyClass, string>();
        public Property<MyClass, string> First {get; } = new Property<MyClass, string>();
    }

#Sample 2

    public class MyClass : MyBaseClass
    {
        public Property<MyClass, string> Last {get; } = new Property<MyClass, string>();
        public Property<MyClass, string> First {get; } = new Property<MyClass, string>();
        
        private CallResult<int> CalculateAge()
        {
            var result = new CallResult<int>(); //defaults: Success=False, Result=0, Exception=null
            try
            {
                result.Result = 22; //no real calculations in this sample
                result.Success = true;
            }
            catch (Exception e)
            {
              result.Exception = e;
            }
        }
        public MyClass()
        {
            PropertyHostMethods.InitializeProperties(this);
        }
    }
