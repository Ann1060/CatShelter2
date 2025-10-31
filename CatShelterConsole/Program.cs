using BisnessLogic;
using CatEntity;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatShelterConsole
{
    internal class Program
    {
        private static CatService catService;
        private static IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());

        static void Main(string[] args)
        {
            catService = ninjectKernel.Get<CatService>();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Приют для кошек");
                Console.WriteLine("1. Посмотреть всех котов");
                Console.WriteLine("2. Добавить кота");
                Console.WriteLine("3. Изменить кота");
                Console.WriteLine("4. Удалить кота");
                Console.WriteLine("5. Статистика по породам");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowAllCats(); break;
                    case "2": AddCat(); break;
                    case "3": EditCat(); break;
                    case "4": DeleteCat(); break;
                    case "5": ShowStats(); break;
                    case "6": return;
                }
            }
        }

        static void ShowAllCats()
        {
            Console.Clear();
            Console.WriteLine("Все коты:");
            Console.WriteLine("ID | Имя | Порода | Возраст");
            Console.WriteLine("---------------------------");

            List<Cat> _cats = catService.GetAllCats();
            if (_cats == null)
            {
                Console.WriteLine("\nНажмите любую клавишу...");
                Console.ReadKey();
                return;
            }
            for (int i = 0; i < _cats.Count; i++)
            {
                Cat cat = _cats[i];
                if (cat == null)
                {
                    Console.WriteLine($"{i + 1} | [ОШИБКА: данные кота повреждены]");
                    continue; // Пропускаем проблемного кота, но продолжаем цикл
                }
                Console.WriteLine($"{cat.Id} | {cat.Name} | {cat.Breed} | {cat.Age}");
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void AddCat()
        {
            Console.Clear();
            Console.WriteLine("Добавление кота:");

            var cat = new Cat();

            Console.Write("Имя: ");
            cat.Name = Console.ReadLine();

            Console.Write("Порода: ");
            cat.Breed = Console.ReadLine();

            Console.Write("Возраст: ");
            cat.Age = int.Parse(Console.ReadLine());

            catService.AddCat(cat);
            Console.WriteLine("Кот добавлен!");
            Console.ReadKey();
        }

        static void EditCat()
        {
            Console.Clear();
            ShowAllCats();

            Console.Write("Введите ID кота для изменения: ");
            int id = int.Parse(Console.ReadLine());

            var cat = catService.GetCatById(id);
            if (cat == null)
            {
                Console.WriteLine("Кот не найден!");
                Console.ReadKey();
                return;
            }

            Console.Write($"Имя ({cat.Name}): ");
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name)) cat.Name = name;

            Console.Write($"Порода ({cat.Breed}): ");
            string breed = Console.ReadLine();
            if (!string.IsNullOrEmpty(breed)) cat.Breed = breed;

            Console.Write($"Возраст ({cat.Age}): ");
            string ageInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(ageInput)) cat.Age = int.Parse(ageInput);

            catService.UpdateCat(cat);
            Console.WriteLine("Кот изменен!");
            Console.ReadKey();
        }

        static void DeleteCat()
        {
            Console.Clear();
            ShowAllCats();

            Console.Write("Введите ID кота для удаления: ");
            int id = int.Parse(Console.ReadLine());

            var cat = catService.GetCatById(id);
            if (cat == null)
            {
                Console.WriteLine("Кот не найден!");
                Console.ReadKey();
                return;
            }

            Console.Write($"Удалить кота {cat.Name}? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                catService.DeleteCat(id);
                Console.WriteLine("Кот удален!");
            }

            Console.ReadKey();
        }

        static void ShowStats()
        {
            Console.Clear();
            Console.WriteLine("Статистика по породам:");

            var stats = catService.GetCatsByBreedGrouped();
            foreach (var item in stats)
            {
                Console.WriteLine($"{item.Key}: {item.Value} котов");
            }

            Console.WriteLine($"\nВсего котов: {catService.GetTotalCats()}");
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
