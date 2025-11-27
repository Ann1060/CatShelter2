using CatEntity;
using CatShelter.Shared;
using CatShelterDaL;
using Presenter;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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
        public MainViewModel(IModel model, ViewManager viewManager)
        {
            _model = model;
            _viewManager = viewManager;
            LoadCat();

            // Инициализация команд (аналог button.Click +=)
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

        // СВОЙСТВА ДЛЯ ФОРМЫ ДОБАВЛЕНИЯ
        public string Name { get; set; }
        public int Age { get; set; }
        public string Breed { get; set; }
        private void ExecuteOpenAddDialog()
        {
            Name = string.Empty;
            Age = 0;
            Breed = string.Empty;
            _viewManager.ShowAddCatDialog(this);
        }
        private void ExecuteSaveCat()
        {
            if (string.IsNullOrWhiteSpace(Name) && Age > 0 && string.IsNullOrWhiteSpace(Breed))
            {
                var _cat = new CatDTO
                {
                    Name = this.Name,
                    Age = this.Age,
                    Breed = this.Breed,
                };

                _model.AddCat(_cat.ToDomainModel());
            }
            else
            {
                return;
            }
            LoadCat();
        }
        private void ExecuteCancelAdd() { }

        // СВОЙСТВА ДЛЯ ФОРМЫ УДАЛЕНИЯ
        private string _deleteQuestion;
        public string DeleteQuestion
        {
            get => _deleteQuestion;
            set => SetField(ref _deleteQuestion, value);
        }
        private void ExecuteOpenDeleteDialog()
        {
            if (SelectedCat != null)
            {
                DeleteQuestion = $"Вы точно хотите удалить кота '{SelectedCat.Name}'?";
                _viewManager.ShowDeleteConfirmDialog(this);
            }
            else
            {
                return;
            }
        }
        private void ExecuteConfirmDelete()
        {
            if (SelectedCat != null)
            {
                _model.DeleteCat(SelectedCat.Id);
                SelectedCat = null;
            }
            else { return; }
            LoadCat();
        }
        private void ExecuteCancelDelete()
        {
        }

        // СВОЙСТВА ДЛЯ ФОРМЫ ОБНОВЛЕНИЯ
        private bool _isEditDialogOpen;
        public bool IsEditDialogOpen
        {
            get => _isEditDialogOpen;
            set => SetField(ref _isEditDialogOpen, value);
        }
        public CatDTO _editingCat;
        public CatDTO EditingCat
        {
            get => _editingCat;
            set => SetField(ref _editingCat, value);
        }
        private void ExecuteOpenEditDialog()
        {
            if (SelectedCat == null)
            {
                return;
            }
            EditingCat = new CatDTO
            {
                Id = SelectedCat.Id,
                Name = SelectedCat.Name,
                Age = SelectedCat.Age,
                Breed = SelectedCat.Breed
            };
            IsEditDialogOpen = true;
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
        private void ExecuteCancelEdit()
        {
            IsEditDialogOpen = false;
            EditingCat = null;
        }

        //СВОЙСТВА ДЛЯ ФОРМЫ СТАТИСТИКИ
        private string _statisticsText;
        public string StatisticsText
        {
            get => _statisticsText;
            set => SetField(ref _statisticsText, value);
        }
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
            _viewManager.ShowEditCatDialog(this);
        }
        private void ExecuteCloseStatistics()
        {
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
    }
}
