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
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void btnMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new StandardWindow();
            dlg.Show();
        }

        private void btnMainMetroWindow_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new MetroWindow();
            dlg.Show();
        }

        private void btnGenericMainWindow_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new GenericWindow();
            dlg.Show();
        }

        private void btnGenericMainMetroWindow_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new GenericMetroWindow();
            dlg.Show();
        }

        private void btnPageHost_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new StandardFrameWindow();
            dlg.Show();
        }

        private void btnGenericPageHost_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new GenericFrameWindow();
            dlg.Show();
        }
    }
}
