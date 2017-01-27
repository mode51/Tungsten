using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W;

namespace Tungsten.Demo.WPF.Models
{
    //public class MainWindowModel : PropertyChangedNotifier //(must call PropertyHostMethods.InitializeProperties)
    //or
    public class MainWindowModel : PropertyHostNotifier //or PropertyChangedNotifier(with call to PropertyHostMethods.InitializeProperties)
    {
        private W.Threading.Thread _thread;

        //not owned properties
        public Property<string> Title { get; } = new Property<string>();
        public Property<int> RandomNumber { get; } = new Property<int>();
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

        ~MainWindowModel()
        {
            _thread?.Cancel();
        }
        public MainWindowModel()
        {
            //this is a sample viewmodel
            Title.Value = "Tungsten";
            _thread = W.Threading.Thread.Create(cts =>
            {
                while (!cts.IsCancellationRequested)
                {
                    if (RandomNumber.WaitForChanged())
                        Count.Value += 1;
                }
            });
        }
    }
}
