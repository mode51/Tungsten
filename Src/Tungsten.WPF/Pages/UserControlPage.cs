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
    public class UserControlPage<TModel> : UserControl, IPage where TModel : new()
    {
        public TModel ViewModel { get; private set; }

        public virtual void OnNavigateFrom(IPage nextPage)
        {
        }
        public virtual void OnNavigateTo(IPage previousPage, params object[] args)
        {
        }
        public UserControlPage(TModel model)
        {
            ViewModel = model;
            DataContext = ViewModel;
        }
        public UserControlPage() : this(Activator.CreateInstance<TModel>())
        {
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
