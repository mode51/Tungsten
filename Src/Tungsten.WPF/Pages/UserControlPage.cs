using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace W.WPF.Pages
{
    /// <summary>
    /// Can be hosted in a ContentPresenter to simulate page navigation
    /// </summary>
    /// <typeparam name="TModel">The type of viewmodel to use</typeparam>
    /// <remarks>Can be hosted in a ContentPresenter</remarks>
    public class UserControlPage<TModel> : UserControlPage where TModel : new()
    {
        public new TModel ViewModel { get; private set; }
        protected override void SetViewModel(object vm)
        {
            ViewModel = (TModel)vm;
            base.SetViewModel(vm);
        }
        public UserControlPage() : this(Activator.CreateInstance<TModel>()) { }
        public UserControlPage(TModel model) : base(model)
        {
        }
    }
    /// <summary>
    /// Can be hosted in a ContentPresenter to simulate page navigation
    /// </summary>
    /// <typeparam name="TModel">The type of viewmodel to use</typeparam>
    /// <remarks>Can be hosted in a ContentPresenter</remarks>
    public class UserControlPage : UserControl, IPage
    {
        public object ViewModel { get; private set; }
        protected virtual void SetViewModel(object vm)
        {
            ViewModel = vm;
        }
        public virtual void OnNavigateFrom(IPage nextPage)
        {
        }
        public virtual void OnNavigateTo(IPage previousPage, params object[] args)
        {
        }
        public UserControlPage() : this(null) { }
        public UserControlPage(object model)
        {
            if (model == null)
                model = this;
            SetViewModel(model);
            ViewModel = model;
            DataContext = ViewModel;
        }
    }
    //public class Page : Control, IPage
    //{
    //    public virtual void OnNavigateTo(params object[] args)
    //    {
    //    }
    //    public Page(object viewModel) : base(viewModel)
    //    {
    //    }
    //    public Page() : this(null)
    //    {
    //    }
    //}
}
