using CatEntity;
using CatShelter.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Presenter
{
    public class CatViewModel : BaseViewModel
    {
        private readonly IModel _catService;
        private readonly ViewManager _viewManager;

        private BindingList<CatDTO> _cats = new BindingList<CatDTO>();
        public BindingList<CatDTO> Cats
        {
            get => _cats;
            set { _cats = value; OnPropertyChanged(); }
        }

        private CatDTO _selectedCat;
        public CatDTO SelectedCat
        {
            get => _selectedCat;
            set
            {
                _selectedCat = value;
                OnPropertyChanged();
                (EditCatCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteCatCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private CatDTO _editingCat;
        public CatDTO EditingCat
        {
            get => _editingCat;
            set { _editingCat = value; OnPropertyChanged(); }
        }

        private bool _isEditDialogOpen;
        public bool IsEditDialogOpen
        {
            get => _isEditDialogOpen;
            set { _isEditDialogOpen = value; OnPropertyChanged(); }
        }

        // Команды по шаблону с картинки
        private ICommand _loadCatsCommand;
        public ICommand LoadCatsCommand => _loadCatsCommand
            ?? (_loadCatsCommand = new RelayCommand(LoadCats));

        private ICommand _addCatCommand;
        public ICommand AddCatCommand => _addCatCommand
            ?? (_addCatCommand = new RelayCommand(AddCat));

        private ICommand _editCatCommand;
        public ICommand EditCatCommand => _editCatCommand
            ?? (_editCatCommand = new RelayCommand(EditCat, CanEditCat));

        private ICommand _deleteCatCommand;
        public ICommand DeleteCatCommand => _deleteCatCommand
            ?? (_deleteCatCommand = new RelayCommand(DeleteCat, CanDeleteCat));

        private ICommand _saveEditCommand;
        public ICommand SaveEditCommand => _saveEditCommand
            ?? (_saveEditCommand = new RelayCommand(SaveEdit));

        private ICommand _cancelEditCommand;
        public ICommand CancelEditCommand => _cancelEditCommand
            ?? (_cancelEditCommand = new RelayCommand(CancelEdit));

        public CatViewModel(IModel catService, ViewManager viewManager)
        {
            _catService = catService;
            _viewManager = viewManager;
            LoadCats();
        }

        public void LoadCats()
        {
            var domainCats = _catService.GetAllCats();
            Cats = new BindingList<CatDTO>(domainCats.Select(ToDTO).ToList());
        }

        public void AddCat()
        {
            EditingCat = new CatDTO();
            IsEditDialogOpen = true;
        }

        public void EditCat()
        {
            if (SelectedCat == null) return;

            EditingCat = new CatDTO
            {
                Id = SelectedCat.Id,
                Name = SelectedCat.Name,
                Breed = SelectedCat.Breed,
                Age = SelectedCat.Age
            };
            IsEditDialogOpen = true;
        }

        private bool CanEditCat() => SelectedCat != null;

        private bool CanDeleteCat() => SelectedCat != null;

        public void SaveEdit()
        {
            if (EditingCat == null) return;

            if (string.IsNullOrWhiteSpace(EditingCat.Name))
                return;

            var domainCat = ToDomain(EditingCat);

            if (EditingCat.Id == 0)
            {
                _catService.AddCat(domainCat);
            }
            else
            {
                _catService.UpdateCat(domainCat);
            }

            IsEditDialogOpen = false;
            LoadCats();
        }

        public void CancelEdit()
        {
            IsEditDialogOpen = false;
        }

        public void DeleteCat()
        {
            if (SelectedCat != null)
            {
                _catService.DeleteCat(SelectedCat.Id);
                LoadCats();
            }
        }

        private CatDTO ToDTO(Cat domainCat) => new CatDTO
        {
            Id = domainCat.Id,
            Name = domainCat.Name,
            Breed = domainCat.Breed,
            Age = domainCat.Age
        };

        private Cat ToDomain(CatDTO dto) => new Cat
        {
            Id = dto.Id,
            Name = dto.Name,
            Breed = dto.Breed,
            Age = dto.Age
        };
    }
}
