using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.WPF.Demo.Models
{
    public class MetroWindowModel : W.WPF.Models.WindowModel//<MainWindowModel>
    {
        //public W.WPF.Core.CollectionProperty<MetroWindowModel, string> AllAccents { get; } = new Core.CollectionProperty<MetroWindowModel, string>(Enum.GetNames(typeof(MetroApplicationModel.EAccent)));
        //public W.WPF.Core.CollectionProperty<MetroWindowModel, string> AllThemes { get; } = new Core.CollectionProperty<MetroWindowModel, string>(Enum.GetNames(typeof(MetroApplicationModel.ETheme)));

        public MetroWindowModel() : base()
        {
        }
    }
}
