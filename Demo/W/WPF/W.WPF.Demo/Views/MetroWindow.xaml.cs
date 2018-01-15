using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using W.AsExtensions;
using W.WPF.Demo.Models;

namespace W.WPF.Demo.Views
{
    /// <summary>
    /// Interaction logic for MainMetroWindow.xaml
    /// </summary>
    public partial class MetroWindow : W.WPF.Views.MetroWindow
    {
        public MetroWindow() : base(new MainWindowModel())
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs args)
        {
            ViewModel.As<MainWindowModel>()?.PageFramework.NavigateTo("Home");
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.As<MainWindowModel>()?.PageFramework.NavigateTo("Settings");
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            MahApps.Metro.Controls.Dialogs.DialogManager.ShowMessageAsync(this, "About W.WPF.Demo", "This is a general MahApps message dialog");
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.As<MainWindowModel>()?.PageFramework.NavigateTo("Home");
        }
    }
}
