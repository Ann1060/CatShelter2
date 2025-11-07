using CatEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatShelterDaL
{
    public interface ICatRepository : IRepository<Cat>
    {
        Dictionary<string, int> CalculateCatAgeInHumanYears();
        Dictionary<string, int> GetAgeGroups(); // ДОБАВЛЕН ВТОРОЙ МЕТОД
    }

    public class CatRepository : EntityFrameworkRepository<Cat>, ICatRepository
    {
        public CatRepository() : base()
        {

        }

        // ПЕРВЫЙ МЕТОД: Расчет кошачьего возраста в человеческих годах
        public Dictionary<string, int> CalculateCatAgeInHumanYears()
        {
            var cats = GetAll();
            return cats?.ToDictionary(
                cat => cat.Name,
                cat => {
                    if (cat.Age == 1) return 15;          // 1 кошачий год = 15 человеческих
                    else if (cat.Age == 2) return 24;     // 2 кошачий год = 24 человеческих
                    else return 24 + (cat.Age - 2) * 4;   // Последующие годы ×4
                }
            ) ?? new Dictionary<string, int>();
        }

        //Анализ возрастных групп кошек
        public Dictionary<string, int> GetAgeGroups()
        {
            var cats = GetAll();
            if (cats == null) return new Dictionary<string, int>();

            return cats
                .GroupBy(cat => GetAgeGroupName(cat.Age))
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // Вспомогательный метод
        private string GetAgeGroupName(int age)
        {
            if (age <= 1) return "Котята";
            if (age <= 3) return "Молодые";
            if (age <= 7) return "Взрослые";
            if (age <= 12) return "Зрелые";
            return "Почтенные старцы";
        }
    }
}