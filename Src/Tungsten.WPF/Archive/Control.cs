using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using W.Logging;

namespace W.WPF.Views
{
    public class Control<TModel> : Control where TModel : new()
    {
        public TModel ViewModel { get; private set; }

        public Control(TModel model) : base()
        {
            ViewModel = model;
            DataContext = ViewModel;
        }
        public Control() : this(Activator.CreateInstance<TModel>())
        {
        }
    }
    //public class Control : UserControl //don't implement IPage here because not all controls are pages
    //{
    //    public TModel GetViewModel<TModel>() { return (TModel)DataContext; }

    //    public Control(object viewModel)
    //    {
    //        if (viewModel == null)
    //            viewModel = this;
    //        DataContext = viewModel;
    //    }
    //    public Control() : this(null)
    //    {
    //    }
    //}
}
