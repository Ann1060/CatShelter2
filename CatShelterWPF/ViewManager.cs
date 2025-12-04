using System.Windows;
using CatShelter.Presenter;

namespace CatShelterWPF
{
    public class ViewManager : Presenter.ViewManager
    {
        AddCatView addView;
        EditCatView editView;
        DeleteConfirmView deleteView;
        StatisticsView statsView;
        public override bool? ShowAddCatDialog(MainViewModel mainViewModel)
        {
            addView = new AddCatView(mainViewModel);
            addView.Owner = Application.Current.MainWindow;
            return addView.ShowDialog();
        }
        public override bool? CloseAddCatDialog()
        {
            return addView.DialogResult = true;
        }

        public override bool? ShowEditCatDialog(MainViewModel mainViewModel)
        {
            editView = new EditCatView(mainViewModel);
            editView.Owner = Application.Current.MainWindow;
            return editView.ShowDialog();
        }
        public override bool? CloseEditCatDialog()
        {
            return editView.DialogResult = true;
        }

        public override bool? ShowDeleteConfirmDialog(MainViewModel mainViewModel)
        {
            deleteView = new DeleteConfirmView(mainViewModel);
            deleteView.Owner = Application.Current.MainWindow;
            return deleteView.ShowDialog();
        }
        public override bool? CloseDeleteConfirmDialog()
        {
            return deleteView.DialogResult = true;
        }

        public override bool? ShowStatisticsDialog(MainViewModel mainViewModel)
        {
            statsView = new StatisticsView(mainViewModel);
            statsView.Owner = Application.Current.MainWindow;
            return statsView.ShowDialog();
        }
        public override bool? CloseStatisticsDialog()
        {
            return statsView.DialogResult = true;
        }
    }
}