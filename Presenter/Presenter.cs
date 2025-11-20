using CatEntity;
using CatShelter.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatShelter.Presenter
{
    public class Presenter
    {
        private readonly IView _view;
        private readonly IModel _model;

        private int _currentPage = 1;
        private int _pageSize = 5;
        private int _totalPages = 1;

        public Presenter(IView view, IModel model)
        {
            _view = view;
            _model = model;

            // Подписываемся на события View
            _view.AddCatClicked += OnAddCat;
            _view.EditCatClicked += OnEditCat;
            _view.DeleteCatClicked += OnDeleteCat;
            _view.RefreshClicked += OnRefresh;
            _view.StatsCat += OnStatsCat;
            _view.NextPageClicked += OnNextPage;
            _view.PrevPageClicked += OnPrevPage;
            _view.PageSizeChanged += OnPageSizeChanged;

            LoadCats();
        }

        // ------ ЗАГРУЗКА КОТОВ ------
        private void LoadCats()
        {
            _pageSize = _view.GetPageSize();

            var cats = _model.GetPagedCats(_currentPage, _pageSize);
            int totalCount = _model.GetTotalCats();

            _totalPages = (int)Math.Ceiling((double)totalCount / _pageSize);
            if (_totalPages == 0) _totalPages = 1;

            _view.ShowCats(cats);
            _view.UpdateTotalLabel(totalCount);
            _view.UpdatePageInfo(_currentPage, _totalPages);

            bool canGoPrev = _currentPage > 1;
            bool canGoNext = _currentPage < _totalPages;

            _view.SetPrevButtonEnabled(canGoPrev);
            _view.SetNextButtonEnabled(canGoNext);
        }

        // ------ ДОБАВЛЕНИЕ ------
        private void OnAddCat()
        {
            _view.GetCatInput(out string name, out string breed, out int age);
            if (string.IsNullOrWhiteSpace(name)) return;

            _model.AddCat(new Cat
            {
                Name = name,
                Breed = breed,
                Age = age
            });

            _view.ShowMessage("Кот успешно добавлен!");
            LoadCats();
        }

        // ------ РЕДАКТИРОВАНИЕ ------
        private void OnEditCat()
        {
            int id = _view.GetSelectedCatId();
            if (id <= 0)
            {
                _view.ShowMessage("Выберите кота!");
                return;
            }

            var existing = _model.GetCatById(id);
            if (existing == null) return;

            var updated = _view.GetUpdatedCatData(existing);
            if (updated.name == null) return;

            existing.Name = updated.name;
            existing.Breed = updated.breed;
            existing.Age = updated.age;

            _model.UpdateCat(existing);

            _view.ShowMessage("Кот обновлён!");
            LoadCats();
        }

        // ------ УДАЛЕНИЕ ------
        private void OnDeleteCat()
        {
            int id = _view.GetSelectedCatId();
            if (id <= 0)
            {
                _view.ShowMessage("Выберите кота для удаления!");
                return;
            }

            var result = _view.DeleteOrNotDelete();

            if (result)
            {
                _model.DeleteCat(id);

                int totalCount = _model.GetTotalCats();
                int maxPage = (int)Math.Ceiling((double)totalCount / _pageSize);
                if (_currentPage > maxPage) _currentPage = maxPage;

                _view.ShowMessage("Кот удалён!");
                LoadCats();
            }
        }

        // ------ ОБНОВИТЬ ------
        private void OnRefresh() => LoadCats();

        private void OnStatsCat()
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
            _view.ShowMessage($"{messageBuilder.ToString()}");
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

        // ------ ПАГИНАЦИЯ ------
        private void OnNextPage()
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadCats();
            }
        }

        private void OnPrevPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadCats();
            }
        }

        private void OnPageSizeChanged()
        {
            _currentPage = 1;   // сбрасываем на первую страницу
            LoadCats();
        }
    }
}
