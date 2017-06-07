using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.WPF.Models
{
    /// <summary>
    /// A base class Model which can be used for a Tungsten.WPF.Page
    /// </summary>
    /// <typeparam name="TPage"></typeparam>
    public class PageModel<TPage> : ModelBase
    {
        public new TPage Owner { get; private set; } // used by OwnedDPProperty members

        public PageModel() : this(default(TPage))
        {
        }
        public PageModel(TPage owner) : base()
        {
            Owner = owner;
        }
    }

    public class PageModel : ModelBase
    {
        public PageModel() : base()
        {
        }
    }
}
