using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatEntity;

namespace CatShelterDaL
{
    public class Context : DbContext
    {
        public Context() : base("DbConnection")
        {
            
        }
        public DbSet<Cat> Cats { get; set; }
    }
    public class EntityFrameworkRepository<T> :
        IRepository<T> where T : class, IDomainObject, new()
    {
        public Context _context;
        public bool _disposed = false;
        public EntityFrameworkRepository()
        {
            _context = new Context();
        }
        public List<T> GetAll()
        {
            if (_context.Set<T>().Count() == 0)
            {
                return null;
            }
            return _context.Set<T>().ToList();
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            T entity = GetById(id);
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
        public void Update(T entity)
        {
            T item = GetById(entity.Id);
            if (item == null)
                throw new ArgumentException($"Сущность с ID {entity.Id} не найдена");
            _context.Entry(item).CurrentValues.SetValues(entity);
            _context.SaveChanges();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public int GetTotal()
        {
            return _context.Set<T>().Count();
        }
        public List<T> GetPaged(int pageNumber, int pageSize)
        {
            return _context.Set<T>()
                .OrderBy(x => x.Id) // или любое другое поле для сортировки
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
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
        ~EntityFrameworkRepository()
        {
            Dispose(false);
        }
    }
}