using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.WPF.Demo.Models
{
    public class HomeModel : W.WPF.Models.ViewModel
    {
        public HomeModel()
        {
            base.Title.Value = "Home";
        }
    }
}
