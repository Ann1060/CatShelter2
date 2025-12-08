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
        ExportView exportView;
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

        public override bool? ShowExportDialog(MainViewModel mainViewModel)
        {
            exportView = new ExportView(mainViewModel);
            exportView.Owner = Application.Current.MainWindow;
            return exportView.ShowDialog();
        }

        public override void CloseExportDialog()
        {
            exportView?.Close();
        }

        public override string ShowSaveFileDialog(string filter, string defaultExt, string fileName, string initialDirectory)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt,
                FileName = fileName,
                InitialDirectory = initialDirectory,
                Title = "Сохранить файл",
                OverwritePrompt = true
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public override void ShowMessage(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public override bool ShowConfirmationDialog(string message)
        {
            var result = MessageBox.Show(message, "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }
    }
}