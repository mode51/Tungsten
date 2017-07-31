using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W;
using W.Logging;

namespace W.Demo.Winforms
{
    //Make sure PropertyHostNotifier is inherited so that each owned property has it's owner set
    //Alternatively, you could call PropertyHostMethods.InitializeProperties in the CPerson constructor
    public class PersonModel : PropertyHostNotifier
    {
        public Property<PersonModel, string> Last { get; } = new Property<PersonModel, string>(
        (vm, oldValue, newValue) =>
        {
            Log.i("Last Named = {0}", newValue);
            vm.FullName.Value = newValue + ", " + vm.First.Value;
        });
        public Property<PersonModel, string> First { get; } = new Property<PersonModel, string>(
        (vm, oldValue, newValue) =>
        {
            Log.i("First Named = {0}", newValue);
            vm.FullName.Value = vm.Last.Value + ", " + newValue;
        });
        public Property<PersonModel, string> FullName { get; } = new Property<PersonModel, string>();
        public Property<PersonModel, bool> IsBusy { get; } = new Property<PersonModel, bool>(false);
        public Property<PersonModel, int> SaveProgress { get; } = new Property<PersonModel, int>();

        public async Task SaveAsync(Action<bool> onComplete = null)
        {
            IsBusy.Value = true; //occurs on UI thread
            await Task.Factory.StartNew(() =>
            {
                //Do some saving here
                var max = 1000; //I could use 100, but the progress bar needs more updates, otherwise it stops showing progress before 100%.
                for (double t = 1; t <= max; t++)
                {
                    System.Threading.Thread.Sleep(1); //illustrate the passage of time
                    SaveProgress.Value = (int)(100 * (t / max));
                }

                //call onComplete on non-UI thread
                onComplete?.Invoke(true);
                SaveProgress.Value = 0; //reset progress bar
                IsBusy.Value = false; //not on UI thread
            }, TaskCreationOptions.LongRunning);
        }
        public async Task LoadAsync(Action<bool> onComplete)
        {
            IsBusy.Value = true;
            await Task.Run(() =>
            {
                //Do some loading here
                Last.LoadValue("New Last Name");
                First.LoadValue("New First Name");
                onComplete?.Invoke(true);
                IsBusy.Value = false;
            });
        }
    }
}
