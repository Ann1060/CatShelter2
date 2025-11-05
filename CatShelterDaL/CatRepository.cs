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
    }
    public class CatRepository : ICatRepository
    {
        public Context _context;
        private bool _disposed = false;
        public CatRepository()
        {
            _context = new Context();
        }
        public List<Cat> GetAll()
        {
            if (_context.Set<Cat>().Count() == 0)
            {
                return null;
            }
            return _context.Set<Cat>().ToList();
        }
        public void Add(Cat entity)
        {
            _context.Set<Cat>().Add(entity);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            Cat entity = GetById(id);
            _context.Set<Cat>().Remove(entity);
            _context.SaveChanges();
        }
        public void Update(Cat entity)
        {
            Cat item = GetById(entity.Id);
            if (item == null)
                throw new ArgumentException($"Сущность с ID {entity.Id} не найдена");
            _context.Entry(item).CurrentValues.SetValues(entity);
            _context.SaveChanges();
        }
        public Cat GetById(int id)
        {
            return _context.Set<Cat>().Find(id);
        }
        public int GetTotal()
        {
            return _context.Set<Cat>().Count();
        }
        public List<Cat> GetPaged(int pageNumber, int pageSize)
        {
            return _context.Set<Cat>()
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        Dictionary<string, int> ICatRepository.CalculateCatAgeInHumanYears()
        {
            return _context.Set<Cat>().ToDictionary(cat => cat.Name,
            cat =>
            {
                if (cat.Age == 1) return 15;          // 1 кошачий год = 15 человеческих
                else if (cat.Age == 2) return 24;     // 2 кошачий год = 24 человеческих
                else return 24 + (cat.Age - 2) * 4;   // Последующие годы ×4
            }
            ) ?? new Dictionary<string, int>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Освобождаем управляемые ресурсы
                    _context?.Dispose();
                }

                // Освобождаем неуправляемые ресурсы (если есть)
                _disposed = true;
            }
        }

        // на случай, если Dispose не был вызван
        ~CatRepository()
        {
            Dispose(false);
        }
    }
}
