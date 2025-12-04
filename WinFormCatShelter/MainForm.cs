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
        // ССЫЛКА НА CONTROLLER (вместо событий)
        private CatController _controller;

        private BindingList<Cat> catsBinding = new BindingList<Cat>();

        public MainForm()
        {
            InitializeComponent();
            SetupDataGridView();

            // Подписываем обработчики на UI события
            HookEvents();
        }

        // Реализация IView.SetController
        public void SetController(CatController controller)
        {
            _controller = controller;
        }

        private void HookEvents()
        {
            // В MVC: View напрямую вызывает методы Controller

            buttonAdd.Click += (s, e) =>
            {
                var input = GetCatInput();
                if (!string.IsNullOrWhiteSpace(input.name))
                {
                    _controller?.AddCat(input.name, input.breed, input.age);
                }
            };

            buttonEdit.Click += (s, e) =>
            {
                int id = GetSelectedCatId();
                _controller?.EditCat(id);
            };

            buttonDelete.Click += (s, e) =>
            {
                int id = GetSelectedCatId();
                _controller?.DeleteCat(id);
            };

            buttonRefresh.Click += (s, e) => _controller?.LoadCats();

            buttonNext.Click += (s, e) => _controller?.NextPage();
            buttonPrev.Click += (s, e) => _controller?.PrevPage();

            comboBoxPageSize.SelectedIndexChanged += (s, e) =>
            {
                int size = GetPageSize();
                _controller?.ChangePageSize(size);
            };

            dataGridViewCats.CellDoubleClick += (s, e) =>
            {
                int id = GetSelectedCatId();
                _controller?.EditCat(id);
            };
        }

        private void SetupDataGridView()
        {
            dataGridViewCats.AutoGenerateColumns = true;
            dataGridViewCats.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCats.ReadOnly = true;
            dataGridViewCats.AllowUserToAddRows = false;
        }

        // Реализация методов IView
        public void ShowCats(IEnumerable<Cat> cats)
        {
            catsBinding = new BindingList<Cat>(new List<Cat>(cats));
            dataGridViewCats.DataSource = catsBinding;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public (string name, string breed, int age) GetCatInput()
        {
            var form = new AddCatForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                return (form.NewCat.Name, form.NewCat.Breed, form.NewCat.Age);
            }
            return (null, null, -1);
        }

        public int GetSelectedCatId()
        {
            if (dataGridViewCats.SelectedRows.Count == 0)
                return -1;

            if (dataGridViewCats.SelectedRows[0].DataBoundItem is Cat cat)
                return cat.Id;

            return -1;
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

        public bool ConfirmDelete()
        {
            var result = MessageBox.Show("Удалить кота?", "Подтверждение", MessageBoxButtons.YesNo);
            return result == DialogResult.Yes;
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