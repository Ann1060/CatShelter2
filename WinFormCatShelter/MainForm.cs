using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using CatEntity;
using CatShelter.Shared;

namespace WinFormCatShelter
{
    public partial class MainForm : Form, IView
    {
        //События View (Presenter подпишется на них) ======

        public event Action AddCatClicked;
        public event Action EditCatClicked;
        public event Action DeleteCatClicked;
        public event Action RefreshClicked;
        public event Action StatsCat;

        public event Action NextPageClicked;
        public event Action PrevPageClicked;
        public event Action PageSizeChanged;

        // Поля 
        private BindingList<Cat> catsBinding = new BindingList<Cat>();

        public MainForm()
        {
            InitializeComponent();

            // подключаем обработчики кнопок
            HookEvents();

            // DataGridView
            dataGridViewCats.AutoGenerateColumns = true;
            dataGridViewCats.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCats.ReadOnly = true;
            dataGridViewCats.AllowUserToAddRows = false;
        }

        private void HookEvents()
        {
            buttonAdd.Click += (s, e) => AddCatClicked?.Invoke();
            buttonEdit.Click += (s, e) => EditCatClicked?.Invoke();
            buttonDelete.Click += (s, e) => DeleteCatClicked?.Invoke();
            buttonRefresh.Click += (s, e) => RefreshClicked?.Invoke();
            buttonStats.Click += (s, e) => StatsCat?.Invoke();

            buttonNext.Click += (s, e) => NextPageClicked?.Invoke();
            buttonPrev.Click += (s, e) => PrevPageClicked?.Invoke();

            comboBoxPageSize.SelectedIndexChanged += (s, e) => PageSizeChanged?.Invoke();

            dataGridViewCats.CellDoubleClick += (s, e) => EditCatClicked?.Invoke();
        }

        //Методы IView

        public void ShowCats(IEnumerable<Cat> cats)
        {
            catsBinding = new BindingList<Cat>(new List<Cat>(cats));
            dataGridViewCats.DataSource = catsBinding;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public int GetSelectedCatId()
        {
            if (dataGridViewCats.SelectedRows.Count == 0)
                return -1;

            if (dataGridViewCats.SelectedRows[0].DataBoundItem is Cat cat)
                return cat.Id;

            return -1;
        }

        public void GetCatInput(out string name, out string breed, out int age)
        {
            var form = new AddCatForm();

            if (form.ShowDialog() == DialogResult.OK)
            {
                name = form.NewCat.Name;
                breed = form.NewCat.Breed;
                age = form.NewCat.Age;
            }
            else
            {
                name = breed = "";
                age = 0;
            }
        }

        public (string name, string breed, int age) GetUpdatedCatData(Cat cat)
        {
            var form = new EditCatForm(cat);

            if (form.ShowDialog() == DialogResult.OK)
            {
                return (form.UpdatedCat.Name, form.UpdatedCat.Breed, form.UpdatedCat.Age);
            }

            return (null, null, -1);
        }

        public void UpdatePageInfo(int current, int total)
        {
            labelPageInfo.Text = $"Страница {current} из {total}";
        }

        public void UpdateTotalLabel(int totalCount)
        {
            labelTotal.Text = $"Всего котов: {totalCount}";
        }

        public int GetPageSize()
        {
            if (int.TryParse(comboBoxPageSize.SelectedItem?.ToString(), out int value))
                return value;

            return 5;
        }

        public void SetPrevButtonEnabled(bool enabled)
        {
            buttonPrev.Enabled = enabled;
        }

        public void SetNextButtonEnabled(bool enabled)
        {
            buttonNext.Enabled = enabled;
        }
    }
}