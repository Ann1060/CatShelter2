using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatEntity;
using CatShelterDaL;

namespace BisnessLogic
{
    interface ICatService <T> where T : class
    {
        void AddCat(T entity);
        List<Cat> GetAllCats();
        Cat GetCatById(int id);
        void UpdateCat(T entity);
        void DeleteCat(int id);
        Dictionary<string, int> GetCatsByBreedGrouped();
        int GetTotalCats();
    }
    public class CatService : ICatService<Cat>
    {
        private readonly IRepository<Cat> repository;
        public CatService(IRepository<Cat> _repository)
        {
            repository = _repository;
        }


        // Добавить кота
        public void AddCat(Cat cat)
        {
            if (cat == null)
                throw new ArgumentException("Кот не может быть пустым");

            if (string.IsNullOrEmpty(cat.Name))
                throw new ArgumentException("Имя кота обязательно");

            if (string.IsNullOrEmpty(cat.Breed))
                throw new ArgumentException("Порода кота обязательна");

            repository.Add(cat);
        }

        // Получить всех котов
        public List<Cat> GetAllCats()
        {
            return repository.GetAll();
        }

        // Найти кота по ID
        public Cat GetCatById(int id)
        {
            return repository.GetById(id);
        }

        // Обновить кота
        public void UpdateCat(Cat updatedCat)
        {
            repository.Update(updatedCat);
        }

        // Удалить кота
        public void DeleteCat(int id)
        {
            repository.Delete(id);
        }

        // Группировка котов по породе
        public Dictionary<string, int> GetCatsByBreedGrouped()
        {
            List<Cat> cats = repository.GetAll();
            return cats
                .GroupBy(c => c.Breed)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // Получить общее количество котов
        public int GetTotalCats()
        {
            return repository.GetTotal();
        }

        public List<Cat> GetPagedCats(int pageNumber, int pageSize)
        {
            return repository.GetPaged(pageNumber, pageSize);
        }
    }
}
