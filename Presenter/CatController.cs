using System;
using System.Collections.Generic;
using CatShelter.Shared;
using CatEntity;

namespace CatShelter.Controller
{
    public class CatController
    {
        private readonly IModel _model;
        private IView _currentView;

        private int _currentPage = 1;
        private int _pageSize = 5;

        public CatController(IModel model)
        {
            _model = model;
        }

        // В MVC: View регистрируется у Controller, а не передается в конструктор
        public void RegisterView(IView view)
        {
            _currentView = view;

            // View настраивает свои события, Controller их обрабатывает
            view.SetController(this);

            InitializeView();
        }

        // Инициализация представления
        public void InitializeView()
        {
            LoadCats();
        }

        // Методы, которые View будет вызывать напрямую (а не через события)

        public void AddCat(string name, string breed, int age)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _currentView?.ShowMessage("Имя не может быть пустым!");
                return;
            }

            _model.AddCat(new Cat
            {
                Name = name,
                Breed = breed,
                Age = age
            });

            _currentView?.ShowMessage("Кот успешно добавлен!");
            LoadCats();
        }

        public void EditCat(int id)
        {
            if (id <= 0)
            {
                _currentView?.ShowMessage("Выберите кота!");
                return;
            }

            var existing = _model.GetCatById(id);
            if (existing == null) return;

            // Запрашиваем данные через View
            var updated = _currentView.GetUpdatedCatData(existing);
            if (updated.name == null) return;

            existing.Name = updated.name;
            existing.Breed = updated.breed;
            existing.Age = updated.age;

            _model.UpdateCat(existing);
            _currentView?.ShowMessage("Кот обновлён!");
            LoadCats();
        }

        public void DeleteCat(int id)
        {
            if (id <= 0)
            {
                _currentView?.ShowMessage("Выберите кота для удаления!");
                return;
            }

            if (_currentView.ConfirmDelete())
            {
                _model.DeleteCat(id);
                _currentView?.ShowMessage("Кот удалён!");

                // Пересчитываем страницу
                int totalCount = _model.GetTotalCats();
                int maxPage = (int)Math.Ceiling((double)totalCount / _pageSize);
                if (_currentPage > maxPage) _currentPage = maxPage;

                LoadCats();
            }
        }

        public void LoadCats()
        {
            var cats = _model.GetPagedCats(_currentPage, _pageSize);
            int totalCount = _model.GetTotalCats();

            int totalPages = (int)Math.Ceiling((double)totalCount / _pageSize);
            if (totalPages == 0) totalPages = 1;

            _currentView?.ShowCats(cats);
            _currentView?.UpdateTotalLabel(totalCount);
            _currentView?.UpdatePageInfo(_currentPage, totalPages);

            // Обновляем состояние кнопок
            _currentView?.SetPrevButtonEnabled(_currentPage > 1);
            _currentView?.SetNextButtonEnabled(_currentPage < totalPages);
        }

        public void NextPage()
        {
            int totalCount = _model.GetTotalCats();
            int totalPages = (int)Math.Ceiling((double)totalCount / _pageSize);

            if (_currentPage < totalPages)
            {
                _currentPage++;
                LoadCats();
            }
        }

        public void PrevPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadCats();
            }
        }

        public void ChangePageSize(int newSize)
        {
            _pageSize = newSize;
            _currentPage = 1;
            LoadCats();
        }
    }
}