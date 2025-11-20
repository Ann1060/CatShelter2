using System;
using System.Windows.Forms;
using BisnessLogic;
using CatShelter.Shared;
using CatShelter.Presenter;
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

            var repository = new CatRepository(); // из CatShelterDaL
            IModel model = new CatService(repository);
            MainForm view = new MainForm();   // она реализует IView
            var presenter = new Presenter(view, model); // связываем

            Application.Run(view);
        }
    }
}