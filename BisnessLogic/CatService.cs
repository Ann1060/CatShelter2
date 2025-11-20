using CatEntity;
using CatShelter.Shared;     // здесь интерфейс IModel
using CatShelterDaL;       // здесь ICatRepository
using System;
using System.Collections.Generic;
using System.Linq;

namespace BisnessLogic
{
    /// <summary>
    /// Это и есть Model + Business Logic из лабораторной 4
    /// Presenter получает именно этот класс через интерфейс IModel
    /// </summary>
    public class CatService : IModel
    {
        private readonly ICatRepository _repository;

        public CatService(ICatRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public List<Cat> GetAllCats() => _repository.GetAll().ToList();

        public List<Cat> GetPagedCats(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            return _repository.GetAll()
                .OrderBy(c => c.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public int GetTotalCats() => _repository.GetAll().Count();

        public Cat GetCatById(int id) => _repository.GetById(id);

        public void AddCat(Cat cat)
        {
            if (cat == null) throw new ArgumentException("Кот не может быть null");
            if (string.IsNullOrWhiteSpace(cat.Name)) throw new ArgumentException("Имя обязательно");

            _repository.Add(cat);
        }

        public void UpdateCat(Cat cat)
        {
            _repository.Update(cat);
        }

        public void DeleteCat(int id)
        {
            _repository.Delete(id);
        }
        public Dictionary<string, int> GetCatsByBreedGrouped()
        {
            return _repository.GetAll()
                .GroupBy(c => string.IsNullOrEmpty(c.Breed) ? "Без породы" : c.Breed)
                .ToDictionary(g => g.Key, g => g.Count());
        }
        public Dictionary<string, int> GetAgeGroups()
            => _repository.GetAgeGroups();

        public Dictionary<string, int> CalculateCatAgeInHumanYears()
            => _repository.CalculateCatAgeInHumanYears();
    }
}