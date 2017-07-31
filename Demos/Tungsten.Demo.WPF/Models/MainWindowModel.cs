using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W;

namespace W.Demo.WPF.Models
{
    //this is a sample viewmodel

    //public class MainWindowModel : PropertyChangedNotifier //(must call PropertyHostMethods.InitializeProperties)
    //or
    public class MainWindowModel : PropertyHostNotifier, IDisposable //or PropertyChangedNotifier(with call to PropertyHostMethods.InitializeProperties)
    {
        private W.Threading.Thread _thread;

        //not owned properties
        public Property<string> Title { get; } = new Property<string>();
        public Property<int> RandomNumber { get; } = new Property<int>((owner, value, newValue) =>
        {
            System.Diagnostics.Trace.WriteLine($"RandomNumber = {newValue}");
        });
        public Property<int> Count { get; } = new Property<int>();

        //owned properties (MainWindowModel must support INotifyPropertyChanged)
        public Property<MainWindowModel, string> Description { get; } = new Property<MainWindowModel, string>("This is a Tungsten demo");
        public Property<MainWindowModel, int> RandomNumber2 { get; } = new Property<MainWindowModel, int>(); //works because MainWindowModel is PropertyChangedNotifier and is the owner

        public void GenerateRandomNumber()
        {
            var r = new Random();
            RandomNumber.Value = r.Next(1, 20000);
            RandomNumber2.Value = r.Next(1, 20000);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _thread?.Cancel(1000);
        }
        ~MainWindowModel()
        {
            Dispose();
        }
        public MainWindowModel()
        {
            Title.Value = "Tungsten";
            
            //just demonstrate a background thread
            _thread = W.Threading.Thread.Create(cts =>
            {
                while (!cts.IsCancellationRequested)
                {
                    //technically, the value could change while we're looping and we'd never know (so mitigate this in production code)
                    if (RandomNumber.WaitForChanged()) //dont' block indefinitely
                    {
                        Count.Value += 1;
                    }
                }
            });
        }
    }
}
