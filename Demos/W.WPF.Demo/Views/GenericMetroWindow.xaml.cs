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
    /// Interaction logic for MainMetroWindow.xaml
    /// </summary>
    public partial class GenericMetroWindow : W.WPF.Views.MetroWindow<MainWindowModel>
    {
        //private MetroApplicationModel.EAccent _accent = WPF.Models.MetroApplicationModel<MetroApplicationModel>.EAccent.Blue;
        //private MetroApplicationModel.ETheme _theme = WPF.Models.MetroApplicationModel<MetroApplicationModel>.ETheme.BaseLight;
        public Property<W.WPF.MetroStyle> ApplicationStyle { get; } = new Property<MetroStyle>();
        public Property<W.WPF.MetroStyle> LocalStyle { get; } = new Property<WPF.MetroStyle>();

        protected override void OnLoaded(object sender, RoutedEventArgs args)
        {
            ViewModel.PageFramework.NavigateTo("Home");
        }
        public GenericMetroWindow() : base()
        {
            InitializeComponent();
            ApplicationStyle.Value = new MetroStyle();
            LocalStyle.Value = new MetroStyle(this);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PageFramework.NavigateTo("Settings");
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            MahApps.Metro.Controls.Dialogs.DialogManager.ShowMessageAsync(this, "About W.WPF.Demo", "This is a general MahApps message dialog");
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PageFramework.NavigateTo("Home");
        }

        private void btnToggleAccent_Click(object sender, RoutedEventArgs e)
        {
            switch (LocalStyle.Value.Accent.Value)
            {
                case "Blue":
                    LocalStyle.Value.Accent.Value = "Emerald";
                    break;
                case "Emerald":
                    LocalStyle.Value.Accent.Value = "Blue";
                    break;
            }
        }
        private void btnToggleTheme_Click(object sender, RoutedEventArgs e)
        {

            switch (LocalStyle.Value.Theme.Value)
            {
                case "BaseLight":
                    LocalStyle.Value.Theme.Value = "BaseDark";
                    break;
                case "BaseDark":
                    LocalStyle.Value.Theme.Value = "BaseLight";
                    break;
            }
        }
    }
}
