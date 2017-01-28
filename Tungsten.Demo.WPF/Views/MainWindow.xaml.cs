using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tungsten.Demo.WPF.Models;

namespace Tungsten.Demo.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Models.MainWindowModel vm = new MainWindowModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        /// <summary>Raises the <see cref="E:System.Windows.Window.Closing" /> event.</summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            vm?.Dispose();
            base.OnClosing(e);
        }

        private void BtnGenerateNewRandomNumber_OnClick(object sender, RoutedEventArgs e)
        {
            vm.GenerateRandomNumber();
        }
    }
}
