using Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatShelterWPF
{
    public class ViewManager : Presenter.ViewManager
    {
        private readonly Dictionary<Type, Type> _viewModelToViewMapping = new Dictionary<Type, Type>();
        private readonly Dictionary<BaseViewModel, Window> _openViews = new Dictionary<BaseViewModel, Window>();

        public void Register<TViewModel, TView>() where TViewModel : BaseViewModel
        {
            _viewModelToViewMapping[typeof(TViewModel)] = typeof(TView);
        }

        public override void Show<TViewModel>(TViewModel viewModel)
        {
            var viewType = _viewModelToViewMapping[typeof(TViewModel)];
            var view = Activator.CreateInstance(viewType) as Window;

            if (view != null)
            {
                view.DataContext = viewModel;
                view.Show();
                _openViews[viewModel] = view;
            }
        }

        public override void ShowDialog<TViewModel>(TViewModel viewModel)
        {
            var viewType = _viewModelToViewMapping[typeof(TViewModel)];
            var view = Activator.CreateInstance(viewType) as Window;

            if (view != null)
            {
                view.DataContext = viewModel;
                view.ShowDialog();
            }
        }

        public override void Close(BaseViewModel viewModel)
        {
            if (_openViews.TryGetValue(viewModel, out var view))
            {
                view.Close();
                _openViews.Remove(viewModel);
            }
        }
    }
}
