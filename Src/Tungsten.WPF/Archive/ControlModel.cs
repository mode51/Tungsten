using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using W.Logging;
using W.WPF.Core;

namespace W.WPF.Models
{

    ///// <summary>
    ///// Base class for a model customized for WPF controls
    ///// </summary>
    ///// <typeparam name="TControl">The UI control which owns this model</typeparam>
    //public class ControlModel<TControl> : ModelBase
    //{
    //    private TControl _owner;

    //    /// <summary>
    //    /// The UI Owner of this model
    //    /// </summary>
    //    public new TControl Owner
    //    {
    //        get { return _owner; }
    //        set
    //        {
    //            _owner = value;
    //            this.InitializeProperties();
    //        }
    //    } // used by Property members
    //    /// <summary>
    //    /// Constructs a new ControlModel
    //    /// </summary>
    //    public ControlModel() : this(default(TControl))
    //    {
    //    }
    //    /// <summary>
    //    /// Constructs a new ControlModel
    //    /// </summary>
    //    /// <param name="owner">The UI control which owns this model</param>
    //    public ControlModel(TControl owner) : base() //calls InitializeProperties
    //    {
    //        _owner = owner; //don't cause another call to InitializeProperties by calling the property setter
    //        //this.InitializeProperties();
    //    }
    //}

    /// <summary>
    /// Base class for a model with no Owner, customized for WPF controls
    /// </summary>
    public class ControlModel : ModelBase
    {
        /// <summary>
        /// Constructs a new ControlModel
        /// </summary>
        public ControlModel() : base()
        {
        }
    }
}
