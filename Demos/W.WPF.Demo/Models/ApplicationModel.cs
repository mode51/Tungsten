using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.WPF.Demo.Models
{
    public class ApplicationModel : W.WPF.Models.ApplicationModel<ApplicationModel>
    {
        public ApplicationModel()
        {
            Title.Value = "W.WPF.Demo - Demo App";
        }
    }
}
