using System;
using System.Windows.Controls;

namespace W.WPF.Pages
{
    /// <summary>
    /// Must be hosted in a Window or Frame
    /// </summary>
    /// <typeparam name="TModel">The type of viewmodel to use</typeparam>
    /// <remarks>Can be hosted in a ContentPresenter</remarks>
    public class Page<TModel> : System.Windows.Controls.Page, IPage where TModel : new()
    {
        public TModel ViewModel { get; private set; }

        public virtual void OnNavigateFrom(IPage nextPage)
        {
        }
        public virtual void OnNavigateTo(IPage previousPage, params object[] args)
        {
        }
        public Page(TModel model)
        {
            ViewModel = model;
            DataContext = ViewModel;
        }
        public Page() : this(Activator.CreateInstance<TModel>())
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
