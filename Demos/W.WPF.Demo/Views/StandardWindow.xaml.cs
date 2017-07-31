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
using W.WPF.Demo.Models;

namespace W.WPF.Demo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StandardWindow : W.WPF.Views.Window
    {
        public StandardWindow() : base(new MainWindowModel())
        {
            ViewModel.As<MainWindowModel>()?.SetPageHost(this);
            InitializeComponent();
        }
        protected override void OnLoaded(object sender, RoutedEventArgs args)
        {
            ViewModel.As<MainWindowModel>()?.PageFramework.NavigateTo("Home");
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.As<MainWindowModel>()?.PageFramework.NavigateTo("Home");
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.As<MainWindowModel>()?.PageFramework.NavigateTo("Settings");
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.As<MainWindowModel>().Busy.IsBusy.Value = !ViewModel.As<MainWindowModel>().Busy.IsBusy.Value;
            //await ViewModel.RefreshCommander.Execute(null);
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("This is a general MahApps message dialog", "About W.WPF.Demo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
