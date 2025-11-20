using CatEntity;
using System;
using System.Collections.Generic;

namespace CatShelter.Shared
{
    public interface IView
    {
        // События, которые View поднимает и на которые подписывается Presenter
        event Action AddCatClicked;
        event Action EditCatClicked;
        event Action DeleteCatClicked;
        event Action RefreshClicked;
        event Action StatsCat;
        event Action NextPageClicked;
        event Action PrevPageClicked;
        event Action PageSizeChanged;

        // Методы для отображения/взаимодействия, которые Presenter вызывает у View
        void ShowCats(IEnumerable<Cat> cats);
        void ShowMessage(string message);

        // Получить ID выделённого кота (для редактирования/удаления)
        int GetSelectedCatId();

        // Получить ввод пользователя для добавления кота (может вызывать форму AddCatForm)
        void GetCatInput(out string name, out string breed, out int age);

        // Получить обновлённые данные для существующего кота (может вызывать EditCatForm)
        // Возвращаемый кортеж: (name, breed, age). Если пользователь отменил — name == null.
        (string name, string breed, int age) GetUpdatedCatData(Cat cat);

        // Пагинация и служебные методы
        void UpdatePageInfo(int currentPage, int totalPages);
        void UpdateTotalLabel(int totalCount);
        int GetPageSize();
        void SetPrevButtonEnabled(bool enabled);
        void SetNextButtonEnabled(bool enabled);
        bool DeleteOrNotDelete();
    }
}