using CatEntity;
using CatShelter.Shared;
using CatShelterDaL;
using Presenter;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CatShelter.Presenter
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IModel _model;
        private readonly ViewManager _viewManager;
        private ObservableCollection<CatDTO> _cats;
        private CatDTO _selectedCat;

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

        // Свойства для форм
        public string Name { get; set; }
        public int Age { get; set; }
        public string Breed { get; set; }
        public CatDTO EditingCat { get; set; }
        public string DeleteQuestion { get; set; }
        public string StatisticsText { get; set; }

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
        }

        private void LoadCat()
        {
            var catsFromDb = _model.GetAllCats();
            Cats = new ObservableCollection<CatDTO>(
                catsFromDb.Select(c => new CatDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Breed = c.Breed,
                    Age = c.Age,
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
        }

        private void ExecuteCancelAdd() { }

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
        }

        private void ExecuteCancelDelete() { }

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
                    Breed = SelectedCat.Breed
                };
                _viewManager.ShowEditCatDialog(this); // ИСПРАВЛЕНО - вызываем Edit диалог
            }
        }

        private void ExecuteSaveEdit()
        {
            if (SelectedCat != null && EditingCat != null)
            {
                SelectedCat.Name = EditingCat.Name;
                SelectedCat.Age = EditingCat.Age;
                SelectedCat.Breed = EditingCat.Breed;
                _model.UpdateCat(SelectedCat.ToDomainModel());
                LoadCat();
            }
        }

        private void ExecuteCancelEdit() { }

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

        private void ExecuteCloseStatistics() { }

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
    }
}