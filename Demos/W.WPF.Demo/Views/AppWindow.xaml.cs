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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace W.WPF.Demo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AppWindow : Window
    {
        public AppWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void btnMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new MainWindow();
            dlg.Show();
        }

        private void btnMainMetroWindow_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new MainMetroWindow();
            dlg.Show();
        }

        private void btnGenericMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new GenericMainWindow();
            dlg.Show();
        }

        private void btnGenericMainMetroWindow_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new GenericMainMetroWindow();
            dlg.Show();
        }

        private void btnPageHost_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FrameHost();
            dlg.Show();
        }
    }
}
