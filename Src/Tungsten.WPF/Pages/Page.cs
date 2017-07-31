using System;
using System.Windows.Controls;

namespace W.WPF.Pages
{
    /// <summary>
    /// Must be hosted in a Window or Frame
    /// </summary>
    /// <typeparam name="TModel">The type of viewmodel to use</typeparam>
    /// <remarks>Can be hosted in a ContentPresenter</remarks>
    public class Page<TModel> : Page where TModel : new()// System.Windows.Controls.Page, IPage where TModel : new()
    {
        public new TModel ViewModel { get; private set; }

        protected override void SetViewModel(object vm)
        {
            ViewModel = (TModel)vm;
            base.SetViewModel(vm);
        }
        public Page() : this(Activator.CreateInstance<TModel>()) { }
        public Page(TModel model) : base(model)
        {
            //SetViewModel<TModel>(model);
            //DataContext = ViewModel;
        }
    }
    public class Page : System.Windows.Controls.Page, IPage
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
        public Page() : this(null)
        {
        }
        public Page(object viewModel)
        {
            SetViewModel(viewModel);
            DataContext = ViewModel;
        }
    }
}
