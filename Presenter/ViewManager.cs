using CatShelter.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class ViewManager
    {
        public abstract void Show<TViewModel>(TViewModel viewModel) where TViewModel : BaseViewModel;
        public abstract void ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : BaseViewModel;
        public abstract void Close(BaseViewModel viewModel);

        // Бизнес-логика навигации
        public void ShowMainView(IModel catService)
        {
            var mainViewModel = new CatViewModel(catService, this);
            Show(mainViewModel);
        }

        public void ShowEditDialog(CatDTO cat)
        {
            // Логика показа диалога редактирования
        }
    }
}
