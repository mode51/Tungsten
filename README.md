#Tungsten

Platforms: WinForms, WPF, Windows Universal, Windows Portable (PCL)

A C# library to make Windows Forms, WPF, Windows Universal and Windows Portable application development easier.  See the Wiki page for details and use.

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
* InvokeExtensions - A static class exposing InvokeEx extension methods (to ease InvokeRequired handling)
* CallResult - A non-generic class which can be used to return true/false and an exception from a function
* CallResult\<TResult\> - Like CallResult except that you can also specify a result
* Thread - automates creating a thread with an Action
* Thread\<T\> - like Thread, except Action is now Action\<T\>
* Gate - similar to Thread, a Gate can be started some time after creation
* Gate\<T\> - like Gate, exception Action is now Action\<T\>
* ActionQueue\<T\> - Merges a Thread with a ConcurrentQueue.  Executes the provided Action\<T\> or Func\<T, bool\> whenever an item is added to the ConcurrentQueue

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
