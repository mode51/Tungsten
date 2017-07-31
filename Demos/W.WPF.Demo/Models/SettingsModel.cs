using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.WPF.Demo.Models
{
    public class SettingsModel : W.WPF.Models.ViewModel//.PageModel<W.WPF.Demo.Pages.Settings>
    {
        public SettingsModel()
        {
            base.Title.Value = "Settings";
        }
    }
}
