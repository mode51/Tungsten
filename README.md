#Tungsten

Platforms: WinForms, WPF, Windows Universal, Windows Portable (PCL)

A C# library to make Windows Forms, WPF, Windows Universal and Windows Portable application development easier.  See the Wiki page for details and use.

Tungsten is built with .Net Framework 4.5
Tungsten.Universal is built on Windows 10 (10.0; Build 10240)

# Projects
* Tungsten - A classic class library
* Tungsten.Portable - A Portable version of Tungsten
* Tungsten.Universal - A Universal version of Tungsten
* Tungsten.RPC - A classic class library

# Demos
* Tungsten.Demo.WPF - Illustrates how to use Tungsten in a WPF application
* Tungsten.RPC.Server - Illustrates how to create an RPC server with Tungsten.RPC
* Tungsten.RPC.Client - Illustrates how to create an RPC client with Tungsten.RPC

# Tungsten Classes
* [W.Lockable](https://github.com/mode51/Tungsten/wiki/Lockable-TValue-)\<TValue\> - A generic class to wrap a value in a thread-safe manner
* [W.Property](https://github.com/mode51/Tungsten/wiki/Property-TValue)\<TValue\> - A generic class providing a number of enhancements to a [W.Lockable](https://github.com/mode51/Tungsten/wiki/Lockable-TValue-)\<TValue\> value
    * Implements INotifyPropertyChanged
    * Adds IsDirty/MarkAsClean functionality
    * Supports a callback handler in the constructor so you don't have to use events
* [W.Property](https://github.com/mode51/Tungsten/wiki/Property-TOwner,-TValue)\<TOwner, TValue\> - Like Property\<TValue\> except that you can specify an Owner
* Events and callback have type-strict sender (which is the Owner you specify)
* [W.PropertyHost](https://github.com/mode51/Tungsten/wiki/PropertyHost) - a base class which automates the IsDirty, MarkAsClean and InitializeProperties so you don't have to.
* W.PropertyHostNotifier - aggregates PropertyHost and PropertyChangedNotifier
* W.InvokeExtensions - A static class exposing InvokeEx extension methods (to ease InvokeRequired handling)
* W.CallResult - A non-generic class which can be used to return true/false and an exception from a function
* W.CallResult\<TResult\> - Like CallResult except that you can also specify a result
* W.Threading.Thread - automates creating a thread with an Action
* W.Threading.Thread\<T\> - like Thread, except Action is now Action\<T\>
* W.Threading.Gate - similar to Thread, a Gate can be started some time after creation
* W.Threading.Gate\<T\> - like Gate, exception Action is now Action\<T\>
* W.ActionQueue\<T\> - Merges a Thread with a ConcurrentQueue.  Executes the provided Action\<T\> or Func\<T, bool\> whenever an item is added to the ConcurrentQueue

#Tungsten.RPC Classes
* W.RPC.Server - An RPC server (see Tungsten.RPC.ServerDemo for use)
* W.RPC.Client - An RPC client (see Tungsten.RPC.ClientDemo for use)

##Declaring Properties in your code

If you inherit from PropertyHost, the properties will be initialized in the default constructor.

    public class MyClass : PropertyHost
    {
        public Property<MyClass, string> Last { get; } = new Property<MyClass, string>();
        public Property<MyClass, string> First { get; } = new Property<MyClass, string>();
    }

##CallResult, Property and PropertyHost Sample

This sample does not inherit PropertyHost, so it must call PropertyHostMethods.InitializeProperties in the constructor

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
            return result;
        }
        public MyClass()
        {
            PropertyHostMethods.InitializeProperties(this);
            //or this.InitializeProperties();            
        }
    }
