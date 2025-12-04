using System;
using System.Windows.Forms;
using BisnessLogic;
using CatShelter.Shared;
using CatShelter.Controller;
using CatShelterDaL;

namespace WinFormCatShelter
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Создаем Model (бизнес-логика)
            var repository = new CatRepository();
            IModel model = new CatService(repository);

            // 2. Создаем Controller
            var controller = new CatController(model);

            // 3. Создаем View
            MainForm view = new MainForm();

            // 4. Регистрируем View в Controller (в MVC View знает о Controller)
            controller.RegisterView(view);

            Application.Run(view);
        }
    }
}