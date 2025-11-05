using BisnessLogic;
using CatEntity;
using Ninject;
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
        private CatService catService;
        private BindingList<Cat> catsBindingList;

        private int currentPage = 1;
        private int pageSize = 5; // Котов на странице
        private int totalPages = 1;
        public MainForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeDependencies();
            LoadCats();
            InitializeTimer();

            comboBoxPageSize.SelectedItem = pageSize.ToString();
        }

        private void InitializeTimer()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 3000; // 3 секунды
            refreshTimer.Tick += (s, e) => LoadCats();
            refreshTimer.Start();
        }

        // НОВЫЙ МЕТОД: Инициализация зависимостей через Ninject
        private void InitializeDependencies()
        {
            try
            {
                IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
                catService = ninjectKernel.Get<CatService>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации зависимостей: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
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

            try
            {
                // ОБНОВЛЕННЫЙ КОД С ПАГИНАЦИЕЙ
                // Получаем данные с пагинацией
                var cats = catService.GetPagedCats(currentPage, pageSize);
                var totalCount = catService.GetTotalCats();

                // Обновляем DataGridView
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

                // ОБНОВЛЯЕМ ИНФОРМАЦИЮ О ПАГИНАЦИИ
                UpdatePaginationInfo(totalCount);

                // Обновляем общее количество котов
                labelTotal.Text = $"Всего котов: {totalCount}";

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для обновления информации о пагинации
        private void UpdatePaginationInfo(int totalCount)
        {
            // Вычисляем общее количество страниц
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            if (totalPages == 0) totalPages = 1;

            // Обновляем информацию о странице
            labelPageInfo.Text = $"Страница {currentPage} из {totalPages}";

            // Обновляем состояние кнопок
            buttonPrev.Enabled = currentPage > 1;
            buttonNext.Enabled = currentPage < totalPages;
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
                try
                {
                    var selectedCat = dataGridViewCats.SelectedRows[0].DataBoundItem as Cat;
                    if (selectedCat != null)
                    {
                        var result = MessageBox.Show($"Вы уверены, что хотите удалить кота {selectedCat.Name}?",
                            "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            catService.DeleteCat(selectedCat.Id);

                            // Проверяем, не остались ли мы на пустой странице после удаления
                            var totalCount = catService.GetTotalCats();
                            var maxPageForCurrentSize = (int)Math.Ceiling((double)totalCount / pageSize);

                            if (currentPage > maxPageForCurrentSize && maxPageForCurrentSize > 0)
                            {
                                currentPage = maxPageForCurrentSize;
                            }

                            LoadCats();

                            MessageBox.Show("Кот успешно удален", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите кота для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonStats_Click(object sender, EventArgs e)
        {
            var stats = catService.GetCatsByBreedGrouped();
            string message = "🐱 Коты по породам:\n\n";

            foreach (var item in stats)
            {
                string catWord = GetCorrectCatWord(item.Value);
                message += $"{item.Key}: {item.Value} {catWord}\n";
            }

            message += "\n\n🐱 Кошачьи года:\n\n";
            var catYears = catService.CalculateCatAgeInHumanYears();
            foreach (var item in catYears)
            {
                message += $"{item.Key}: {item.Value}\n";
            }

            message += $"\nВсего котов: {catService.GetTotalCats()}";
            MessageBox.Show(message, "Статистика приюта");
        }

        private string GetCorrectCatWord(int count)
        {
            int lastDigit = count % 10;
            int lastTwoDigits = count % 100;

            // Исключения для чисел 11-14
            if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
            {
                return "котов";
            }

            switch (lastDigit)
            {
                case 1:
                    return "кот";
                case 2:
                case 3:
                case 4:
                    return "кота";
                default:
                    return "котов";
            }
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

        private void ButtonPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadCats();
            }
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadCats();
            }
        }

        private void ComboBoxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPageSize.SelectedItem != null)
            {
                // Сохраняем текущую позицию прокрутки
                var firstDisplayedScrollingRowIndex = dataGridViewCats.FirstDisplayedScrollingRowIndex;

                // Обновляем размер страницы
                pageSize = int.Parse(comboBoxPageSize.SelectedItem.ToString());
                currentPage = 1; // Сбрасываем на первую страницу

                // Перезагружаем данные
                LoadCats();

                // Восстанавливаем позицию прокрутки (если возможно)
                if (firstDisplayedScrollingRowIndex >= 0 && firstDisplayedScrollingRowIndex < dataGridViewCats.Rows.Count)
                {
                    dataGridViewCats.FirstDisplayedScrollingRowIndex =
                        Math.Min(firstDisplayedScrollingRowIndex, dataGridViewCats.Rows.Count - 1);
                }
            }
        }
    }
}
