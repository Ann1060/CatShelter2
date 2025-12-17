using CatEntity;
using CatShelter.Shared;
using CatShelterDaL;
using Presenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace CatShelter.Presenter
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IModel _model;
        private readonly ViewManager _viewManager;
        private ObservableCollection<CatDTO> _cats;
        private CatDTO _selectedCat;
        private List<Cat> _catsToExport; // Данные для экспорта
        private Timer timer;
        private Timer timerStroke;

        public ObservableCollection<CatDTO> Cats
        {
            get => _cats;
            set => SetField(ref _cats, value);
        }

        public CatDTO SelectedCat
        {
            get => _selectedCat;
            set => SetField(ref _selectedCat, value);
        }

        // Команды
        public RelayCommand OpenAddDialogCommand { get; }
        public RelayCommand SaveCatCommand { get; }
        public RelayCommand CancelAddCommand { get; }
        public RelayCommand OpenDeleteDialogCommand { get; }
        public RelayCommand ConfirmDeleteCommand { get; }
        public RelayCommand CancelDeleteCommand { get; }
        public RelayCommand OpenEditDialogCommand { get; }
        public RelayCommand SaveEditCommand { get; }
        public RelayCommand CancelEditCommand { get; }
        public RelayCommand ShowStatisticsCommand { get; }
        public RelayCommand CloseStatisticsCommand { get; }
        public RelayCommand BrowseCommand { get; }
        public RelayCommand ExportCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand OpenExportDialogCommand { get; }
        public RelayCommand StrokeCatCommand { get; }

        // Свойства для форм
        public string Name { get; set; }
        public int Age { get; set; }
        public string Breed { get; set; }
        public CatDTO EditingCat { get; set; }
        public string DeleteQuestion { get; set; }
        public string StatisticsText { get; set; }

        //Событие для обновление шкалы голода
        public event EventHandler HungerUpdated;
        public event EventHandler StrokeUpdate;

        public MainViewModel(IModel model, ViewManager viewManager)
        {
            _model = model;
            _viewManager = viewManager;
            LoadCat();

            // Инициализация команд
            OpenAddDialogCommand = new RelayCommand(ExecuteOpenAddDialog);
            SaveCatCommand = new RelayCommand(ExecuteSaveCat);
            CancelAddCommand = new RelayCommand(ExecuteCancelAdd);
            OpenDeleteDialogCommand = new RelayCommand(ExecuteOpenDeleteDialog);
            ConfirmDeleteCommand = new RelayCommand(ExecuteConfirmDelete);
            CancelDeleteCommand = new RelayCommand(ExecuteCancelDelete);
            OpenEditDialogCommand = new RelayCommand(ExecuteOpenEditDialog);
            SaveEditCommand = new RelayCommand(ExecuteSaveEdit);
            CancelEditCommand = new RelayCommand(ExecuteCancelEdit);
            ShowStatisticsCommand = new RelayCommand(ExecuteShowStatistics);
            CloseStatisticsCommand = new RelayCommand(ExecuteCloseStatistics);
            StrokeCatCommand = new RelayCommand(StrokeCat);
            // Новые методы для экспорта
            OpenExportDialogCommand = new RelayCommand(ExecuteOpenExportDialog);
            BrowseCommand = new RelayCommand(ExecuteBrowse);
            ExportCommand = new RelayCommand(ExecuteExport);
            CancelCommand = new RelayCommand(ExecuteCancel);
            // Подписки на события
            HungerUpdated += UpdateHunger;
            StrokeUpdate += UpdateStroke;
            //Таймер для обновления шкалы голода
            timer = new Timer(1000);
            timer.Elapsed += (s, e) => HungerUpdated?.Invoke(this, EventArgs.Empty);
            timerStroke = new Timer(1000);
            timerStroke.Elapsed += (s, e) => StrokeUpdate?.Invoke(this, EventArgs.Empty);
            timer.AutoReset = true;
            timer.Enabled = true;
            timerStroke.Enabled = true;
            timerStroke.AutoReset = true;
        }

        private void MainViewModel_HungerUpdated(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoadCat()
        {
            var catsFromDb = _model.GetAllCats();

            // Создаем новую коллекцию
            Cats = new ObservableCollection<CatDTO>(
                catsFromDb.Select(c => new CatDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Breed = c.Breed,
                    Age = c.Age,
                    LastFeeding = c.LastFeeding,
                    HungryLevel = c.HungryLevel,
                    LastPetTime = c.LastPetTime,
                    IsPet = c.IsPet
                })
            );
        }

        // ФОРМА ДОБАВЛЕНИЯ
        private void ExecuteOpenAddDialog()
        {
            Name = string.Empty;
            Age = 0;
            Breed = string.Empty;
            _viewManager.ShowAddCatDialog(this);
        }

        private void ExecuteSaveCat()
        {
            if (!string.IsNullOrWhiteSpace(Name) && Age > 0 && !string.IsNullOrWhiteSpace(Breed))
            {
                var _cat = new CatDTO
                {
                    Name = this.Name,
                    Age = this.Age,
                    Breed = this.Breed,
                };

                _model.AddCat(_cat.ToDomainModel());
                LoadCat();
            }
            _viewManager.CloseAddCatDialog();
        }

        private void ExecuteCancelAdd() { _viewManager.CloseAddCatDialog(); }

        // ФОРМА УДАЛЕНИЯ
        
        private void ExecuteOpenDeleteDialog()
        {
            if (SelectedCat != null)
            {
                DeleteQuestion = $"Вы точно хотите удалить кота '{SelectedCat.Name}'?";
                _viewManager.ShowDeleteConfirmDialog(this);
            }
        }

        private void ExecuteConfirmDelete()
        {
            if (SelectedCat != null)
            {
                _model.DeleteCat(SelectedCat.Id);
                SelectedCat = null;
                LoadCat();
            }
            _viewManager.CloseDeleteConfirmDialog();
        }

        private void ExecuteCancelDelete() { _viewManager.CloseDeleteConfirmDialog(); }

        // ФОРМА РЕДАКТИРОВАНИЯ
        private void ExecuteOpenEditDialog()
        {
            if (SelectedCat != null)
            {
                EditingCat = new CatDTO
                {
                    Id = SelectedCat.Id,
                    Name = SelectedCat.Name,
                    Age = SelectedCat.Age,
                    Breed = SelectedCat.Breed,
                    HungryLevel = SelectedCat.HungryLevel,
                    LastPetTime = SelectedCat.LastPetTime,
                    LastFeeding = SelectedCat.LastFeeding,
                    IsPet = SelectedCat.IsPet
                };
                _viewManager.ShowEditCatDialog(this);
            }
        }

        private void ExecuteSaveEdit()
        {
            if (SelectedCat != null && EditingCat != null)
            {
                SelectedCat.Name = EditingCat.Name;
                SelectedCat.Age = EditingCat.Age;
                SelectedCat.Breed = EditingCat.Breed;
                SelectedCat.HungryLevel = EditingCat.HungryLevel;
                SelectedCat.LastPetTime = EditingCat.LastPetTime;
                SelectedCat.LastFeeding = EditingCat.LastFeeding;
                SelectedCat.IsPet = EditingCat.IsPet;
                _model.UpdateCat(SelectedCat.ToDomainModel());
                LoadCat();
            }
            _viewManager.CloseEditCatDialog();
        }

        private void ExecuteCancelEdit() { _viewManager.CloseEditCatDialog(); }

        // ФОРМА СТАТИСТИКИ
        private void ExecuteShowStatistics()
        {
            var messageBuilder = new StringBuilder();

            var stats = _model.GetCatsByBreedGrouped();
            messageBuilder.AppendLine("🐱 Коты по породам:\n");
            foreach (var item in stats)
            {
                string catWord = GetCorrectCatWord(item.Value);
                messageBuilder.AppendLine($"{item.Key}: {item.Value} {catWord}\n");
            }
            messageBuilder.AppendLine("🐱 Кошачьи года:\n");
            var catYears = _model.CalculateCatAgeInHumanYears();
            foreach (var item in catYears)
            {
                messageBuilder.AppendLine($"{item.Key}: {item.Value}\n");
            }
            messageBuilder.AppendLine("🐱 Возрастные группы:\n");
            var catYearsGroup = _model.GetAgeGroups();
            foreach (var item in catYearsGroup)
            {
                messageBuilder.AppendLine($"{item.Key}: {item.Value}\n");
            }
            messageBuilder.AppendLine($"\nВсего котов: {_model.GetTotalCats()}");
            StatisticsText = messageBuilder.ToString();
            _viewManager.ShowStatisticsDialog(this);
        }

        private void ExecuteCloseStatistics() { _viewManager.CloseStatisticsDialog(); }

        private string GetCorrectCatWord(int count)
        {
            int lastDigit = count % 10;
            int lastTwoDigits = count % 100;

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

        //Экспорт данных в json или csv
        private string _selectedFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string _fileName = $"cats_export_{DateTime.Now:yyyyMMdd}";
        private string _selectedFormat = "JSON";
        private bool _isExporting;
        private string _statusMessage = "Готово к экспорту";

        public string SelectedFolder
        {
            get => _selectedFolder;
            set => SetField(ref _selectedFolder, value);
        }

        public string FileName
        {
            get => _fileName;
            set => SetField(ref _fileName, value);
        }

        public string SelectedFormat
        {
            get => _selectedFormat;
            set => SetField(ref _selectedFormat, value);
        }

        public bool IsExporting
        {
            get => _isExporting;
            set => SetField(ref _isExporting, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetField(ref _statusMessage, value);
        }

        public string FullPath
        {
            get
            {
                if (string.IsNullOrEmpty(SelectedFolder) || string.IsNullOrEmpty(FileName))
                    return "Не выбран путь или имя файла";

                string extension = SelectedFormat == "JSON" ? ".json" : ".csv";
                string name = FileName.EndsWith(extension) ? FileName : FileName + extension;

                return Path.Combine(SelectedFolder, name);
            }
        }

        public ObservableCollection<string> Formats { get; } = new ObservableCollection<string>
        {
            "JSON",
            "CSV"
        };

        private void ExecuteOpenExportDialog()
        {
            // Получаем данные
            List<Cat> cats = _model.GetAllCats();

            if (cats == null || cats.Count == 0)
            {
                _viewManager.ShowMessage("Нет данных для экспорта");
                return;
            }
            _viewManager.ShowExportDialog(this);
        }

        private void ExecuteBrowse()
        {
            string filter = SelectedFormat == "JSON"
                ? "JSON файлы (*.json)|*.json"
                : "CSV файлы (*.csv)|*.csv";

            string defaultExt = SelectedFormat == "JSON" ? ".json" : ".csv";

            string filePath = _viewManager.ShowSaveFileDialog(filter, defaultExt, FileName, SelectedFolder);

            if (!string.IsNullOrEmpty(filePath))
            {
                // Обрабатываем выбранный файл
                SelectedFolder = Path.GetDirectoryName(filePath);
                FileName = Path.GetFileNameWithoutExtension(filePath);

                // Автоматически выбираем формат по расширению
                string extension = Path.GetExtension(filePath).ToUpper();
                SelectedFormat = extension == ".JSON" ? "JSON" : "CSV";
            }
        }

        private async void ExecuteExport()
        {
            if (string.IsNullOrEmpty(SelectedFolder) || string.IsNullOrEmpty(FileName))
            {
                _viewManager.ShowMessage("Заполните все поля");
                return;
            }

            try
            {
                IsExporting = true;
                StatusMessage = "Подготовка данных...";

                string fullPath = FullPath;

                // Проверяем существование файла
                if (File.Exists(fullPath))
                {
                    bool overwrite = _viewManager.ShowConfirmationDialog(
                        $"Файл '{Path.GetFileName(fullPath)}' уже существует. Перезаписать?");

                    if (!overwrite)
                    {
                        StatusMessage = "Экспорт отменен";
                        IsExporting = false;
                        return;
                    }
                }

                StatusMessage = "Получение данных из базы...";

                // Получаем данные для экспорта
                List<Cat> cats;
                if (_catsToExport != null)
                {
                    cats = _catsToExport;
                }
                else if (_model != null)
                {
                    // Получаем всех котиков из модели
                    cats = _model.GetAllCats();
                }
                else
                {
                    _viewManager.ShowMessage("Нет данных для экспорта");
                    IsExporting = false;
                    return;
                }

                if (cats == null || cats.Count == 0)
                {
                    _viewManager.ShowMessage("Нет данных для экспорта");
                    IsExporting = false;
                    return;
                }

                StatusMessage = $"Экспорт {cats.Count} записей...";

                // Экспортируем в выбранном формате
                bool exportSuccess = await Task.Run(() =>
                {
                    try
                    {
                        if (SelectedFormat == "JSON")
                        {
                            ExportToJson(cats, fullPath);
                        }
                        else
                        {
                            ExportToCsv(cats, fullPath);
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _viewManager.ShowMessage($"Ошибка при сохранении файла: {ex.Message}");
                        return false;
                    }
                });

                if (exportSuccess)
                {
                    StatusMessage = $"✅ Экспорт завершен!\nСохранено: {cats.Count} записей";

                    // Спрашиваем, открыть ли папку
                    bool openFolder = _viewManager.ShowConfirmationDialog(
                        $"Экспортировано {cats.Count} записей.\nХотите открыть папку с файлом?");

                    if (openFolder)
                    {
                        OpenFolderWithFile(fullPath);
                    }

                    // Закрываем окно через 2 секунды
                    await Task.Delay(2000);
                    _viewManager.CloseExportDialog();
                }
                else
                {
                    StatusMessage = "❌ Ошибка экспорта";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Ошибка";
                _viewManager.ShowMessage($"Ошибка при экспорте: {ex.Message}");
            }
            finally
            {
                IsExporting = false;
            }
        }

        private void ExportToJson(List<Cat> cats, string filePath)
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = System.Text.Json.JsonSerializer.Serialize(cats, options);
            File.WriteAllText(filePath, json);
        }

        private void ExportToCsv(List<Cat> cats, string filePath)
        {
            using (var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Заголовки
                writer.WriteLine("Id,Name,Age,Breed,LastFeeding,HungryLevel");

                // Данные
                foreach (var cat in cats)
                {
                    // Экранируем кавычки и запятые
                    string name = EscapeCsvField(cat.Name);
                    string breed = EscapeCsvField(cat.Breed);
                    string lastFeeding = FormatDateTimeForCsv(cat.LastFeeding);
                    string hungryLevel = FormatDoubleForCsv(cat.HungryLevel);

                    writer.WriteLine($"{cat.Id},{name},{cat.Age},{breed},{lastFeeding},{hungryLevel}");
                }
            }
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";

            // Если поле содержит запятые, кавычки или переносы строк - заключаем в кавычки
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                field = field.Replace("\"", "\"\"");
                return $"\"{field}\"";
            }

            return field;
        }

        private string FormatDateTimeForCsv(DateTime dateTime)
        {
            return dateTime.ToString();
        }
        private string FormatDoubleForCsv(double value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        private void OpenFolderWithFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    // Открываем папку и выделяем файл
                    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{filePath}\"");
                }
                else
                {
                    // Просто открываем папку
                    System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(filePath));
                }
            }
            catch (Exception ex)
            {
                _viewManager.ShowMessage($"Не удалось открыть папку: {ex.Message}");
            }
        }

        private void ExecuteCancel()
        {
            _viewManager.CloseExportDialog();
        }

        // Методы чтобы покормить кота
        public void FeedCat ()
        {
            _selectedCat.LastFeeding = DateTime.Now;
            _selectedCat.HungryLevel = 100.0;
            var cat = new CatDTO
            {
                Id = SelectedCat.Id,
                Name = SelectedCat.Name,
                Age = SelectedCat.Age,
                Breed = SelectedCat.Breed,
                LastPetTime = SelectedCat.LastPetTime,
                IsPet = SelectedCat.IsPet,
                LastFeeding = DateTime.Now,
                HungryLevel = 100.0
            };
            _model.UpdateCat(cat.ToDomainModel());
            HungerUpdated?.Invoke(this, EventArgs.Empty);
        }
        public void UpdateHunger(object sender, EventArgs e)
        {
            foreach (var cat in Cats)
            {
                var hoursSinceFed = (DateTime.Now - cat.LastFeeding).TotalHours;
                cat.HungryLevel = Math.Max(0, 100 - (hoursSinceFed * (100.0 / 3.0)));
                _model.UpdateCat(cat.ToDomainModel());

            }
        }
        //Методы для поглаживания
        public void UpdateStroke(object sender, EventArgs e)
        {
            foreach (var cat in Cats)
            {
                if ((DateTime.Now - cat.LastPetTime).TotalMinutes >= 10000)
                {
                    cat.IsPet = false;
                    _model.UpdateCat(cat.ToDomainModel());
                }
            }
        }
        public void StrokeCat()
        {
            _selectedCat.LastPetTime = DateTime.Now;
            _selectedCat.IsPet = true;
            var cat = new CatDTO
            {
                Id = SelectedCat.Id,
                Name = SelectedCat.Name,
                Age = SelectedCat.Age,
                Breed = SelectedCat.Breed,
                LastFeeding = SelectedCat.LastFeeding,
                HungryLevel = SelectedCat.HungryLevel,
                LastPetTime = DateTime.Now,
                IsPet = true
            };
            _model.UpdateCat(cat.ToDomainModel());
            StrokeUpdate?.Invoke(this, EventArgs.Empty);
            LoadCat();
        }
    }
}