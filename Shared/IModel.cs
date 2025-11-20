using CatEntity;
using System.Collections.Generic;

namespace CatShelter.Shared
{
    public interface IModel
    {
        List<Cat> GetPagedCats(int page, int pageSize);
        int GetTotalCats();

        void AddCat(Cat cat);
        void UpdateCat(Cat cat);
        void DeleteCat(int id);
        Cat GetCatById(int id);
        List<Cat> GetAllCats();

        Dictionary<string, int> CalculateCatAgeInHumanYears();
        Dictionary<string, int> GetAgeGroups();
        Dictionary<string, int> GetCatsByBreedGrouped();
    }
}