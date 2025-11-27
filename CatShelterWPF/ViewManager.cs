using System.Windows;
using CatShelter.Presenter;

namespace CatShelterWPF
{
    public class ViewManager : Presenter.ViewManager
    {
        public override bool? ShowAddCatDialog(MainViewModel mainViewModel)
        {
            var addView = new AddCatView(mainViewModel);
            addView.Owner = Application.Current.MainWindow;
            return addView.ShowDialog();
        }

        public override bool? ShowEditCatDialog(MainViewModel mainViewModel)
        {
            var editView = new EditCatView(mainViewModel);
            editView.Owner = Application.Current.MainWindow;
            return editView.ShowDialog();
        }

        public override bool? ShowDeleteConfirmDialog(MainViewModel mainViewModel)
        {
            var deleteView = new DeleteConfirmView(mainViewModel);
            deleteView.Owner = Application.Current.MainWindow;
            return deleteView.ShowDialog();
        }

        public override bool? ShowStatisticsDialog(MainViewModel mainViewModel)
        {
            var statsView = new StatisticsView(mainViewModel);
            statsView.Owner = Application.Current.MainWindow;
            statsView.ShowDialog();
            return statsView.ShowDialog();
        }
    }
}