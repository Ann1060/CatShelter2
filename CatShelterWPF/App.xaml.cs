using CatShelter.Presenter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CatShelterDaL;
using BisnessLogic;
using CatShelter.Shared;

namespace CatShelterWPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Создаем модель и ViewModel
            var _repository = new CatRepository();
            var model = new CatService(_repository);
            var viewManager = new ViewManager();
            var mainViewModel = new MainViewModel(model, viewManager);

            var mainWindow = new MainWindow();
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();
        }
    }
}
