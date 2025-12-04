using System.Collections.Generic;
using CatEntity;
using CatShelter.CatController;

namespace CatShelter.Shared
{
    public interface IView
    {
        // УБИРАЕМ ВСЕ СОБЫТИЯ - в MVC View напрямую вызывает Controller

        // Добавляем метод для установки Controller
        void SetController(CatController controller);

        // Методы для отображения (вызываются Controller'ом)
        void ShowCats(IEnumerable<Cat> cats);
        void ShowMessage(string message);
        void UpdatePageInfo(int currentPage, int totalPages);
        void UpdateTotalLabel(int totalCount);
        void SetPrevButtonEnabled(bool enabled);
        void SetNextButtonEnabled(bool enabled);

        // Методы для получения данных от пользователя
        int GetSelectedCatId();
        (string name, string breed, int age) GetCatInput();
        (string name, string breed, int age) GetUpdatedCatData(Cat cat);
        bool ConfirmDelete();
        int GetPageSize();
    }
}