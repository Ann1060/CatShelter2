using BisnessLogic;
using CatEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormCatShelter
{
    public partial class MainForm : Form
    {
        private System.Windows.Forms.Timer refreshTimer;
        private CatService catService = new CatService();
        private BindingList<Cat> catsBindingList;

        public MainForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            LoadCats();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 3000; // 3 секунды
            refreshTimer.Tick += (s, e) => LoadCats();
            refreshTimer.Start();
        }
        private void InitializeDataGridView()
        {
            dataGridViewCats.AutoGenerateColumns = true;
            dataGridViewCats.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCats.ReadOnly = true;
            dataGridViewCats.AllowUserToAddRows = false;
        }

        private void LoadCats()
        {
            // Сохраняем состояние ДО обновления
            int selectedCatId = -1;
            int currentRowIndex = -1;
            int currentColumnIndex = -1;

            if (dataGridViewCats.SelectedRows.Count > 0 && dataGridViewCats.SelectedRows[0].DataBoundItem is Cat selectedCat)
            {
                selectedCatId = selectedCat.Id;
            }

            // Сохраняем текущую ячейку (для стрелочки)
            if (dataGridViewCats.CurrentCell != null)
            {
                currentRowIndex = dataGridViewCats.CurrentCell.RowIndex;
                currentColumnIndex = dataGridViewCats.CurrentCell.ColumnIndex;
            }

            int firstVisibleRow = dataGridViewCats.FirstDisplayedScrollingRowIndex;

            // Обновляем данные
            var cats = catService.GetAllCats();
            catsBindingList = new BindingList<Cat>(cats ?? new List<Cat>());
            dataGridViewCats.DataSource = catsBindingList;

            // Восстанавливаем позицию прокрутки
            if (firstVisibleRow >= 0 && firstVisibleRow < dataGridViewCats.RowCount)
            {
                dataGridViewCats.FirstDisplayedScrollingRowIndex = firstVisibleRow;
            }

            // Восстанавливаем выделение и текущую ячейку
            if (selectedCatId != -1)
            {
                dataGridViewCats.ClearSelection();

                for (int i = 0; i < dataGridViewCats.Rows.Count; i++)
                {
                    if (dataGridViewCats.Rows[i].DataBoundItem is Cat cat && cat.Id == selectedCatId)
                    {
                        // Выделяем строку
                        dataGridViewCats.Rows[i].Selected = true;

                        // Восстанавливаем текущую ячейку (стрелочку)
                        if (currentColumnIndex >= 0 && currentColumnIndex < dataGridViewCats.Columns.Count)
                        {
                            dataGridViewCats.CurrentCell = dataGridViewCats.Rows[i].Cells[currentColumnIndex];
                        }
                        else
                        {
                            dataGridViewCats.CurrentCell = dataGridViewCats.Rows[i].Cells[0]; // Первая колонка
                        }
                        break;
                    }
                }
            }

            labelTotal.Text = $"Всего котов: {cats?.Count ?? 0}";
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var addForm = new AddCatForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    catService.AddCat(addForm.NewCat);
                    LoadCats(); // Перезагружаем данные
                    MessageBox.Show("Кот успешно добавлен!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewCats.SelectedRows.Count > 0)
            {
                var selectedCat = (Cat)dataGridViewCats.SelectedRows[0].DataBoundItem;
                var editForm = new EditCatForm(selectedCat);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        catService.UpdateCat(editForm.UpdatedCat);
                        LoadCats(); // Перезагружаем данные
                        MessageBox.Show("Данные кота обновлены!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите кота для редактирования");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewCats.SelectedRows.Count > 0)
            {
                var selectedCat = (Cat)dataGridViewCats.SelectedRows[0].DataBoundItem;

                if (MessageBox.Show($"Удалить кота {selectedCat.Name}?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        catService.DeleteCat(selectedCat.Id);
                        LoadCats(); // Перезагружаем данные
                        MessageBox.Show("Кот удален!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите кота для удаления");
            }
        }

        private void buttonStats_Click(object sender, EventArgs e)
        {
            var stats = catService.GetCatsByBreedGrouped();
            string message = "🐱 Коты по породам:\n\n";

            foreach (var item in stats)
            {
                message += $"{item.Key}: {item.Value} котов\n";
            }

            message += $"\nВсего котов: {catService.GetTotalCats()}";

            MessageBox.Show(message, "Статистика приюта");
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadCats();
        }

        // Двойной клик по строке для редактирования
        private void dataGridViewCats_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                buttonEdit_Click(sender, e);
            }
        }
    }
}
