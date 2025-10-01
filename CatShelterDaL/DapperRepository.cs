using CatEntity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CatShelterDaL
{
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private bool _disposed = false;

        public DapperRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            _tableName = typeof(T).Name + "s";
        }

        public List<T> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Query<T>($"SELECT * FROM {_tableName}").ToList();
                return result.Count == 0 ? null : result;
            }
        }

        public void Add(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Автоматическое создание SQL запроса для INSERT
                var properties = typeof(T).GetProperties().Where(p => p.Name != "Id");
                var columnNames = string.Join(", ", properties.Select(p => p.Name));
                var parameterNames = string.Join(", ", properties.Select(p => "@" + p.Name));

                var sql = $"INSERT INTO {_tableName} ({columnNames}) VALUES ({parameterNames}); SELECT CAST(SCOPE_IDENTITY() as int)";

                var id = connection.QuerySingle<int>(sql, entity);

                // Устанавливаем ID для entity
                var idProperty = typeof(T).GetProperty("Id");
                if (idProperty != null && idProperty.CanWrite)
                {
                    idProperty.SetValue(entity, id);
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute($"DELETE FROM {_tableName} WHERE Id = @Id", new { Id = id });
            }
        }

        public void Update(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Автоматическое создание SQL запроса для UPDATE
                var properties = typeof(T).GetProperties().Where(p => p.Name != "Id");
                var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

                var sql = $"UPDATE {_tableName} SET {setClause} WHERE Id = @Id";

                var affectedRows = connection.Execute(sql, entity);
                if (affectedRows == 0)
                {
                    throw new ArgumentException($"Сущность с ID {entity.Id} не найдена");
                }
            }
        }

        public T GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>($"SELECT * FROM {_tableName} WHERE Id = @Id", new { Id = id });
            }
        }

        public int GetTotal()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string tableName = typeof(T).Name;
                string sql = $"SELECT COUNT(*) FROM {tableName}";

                return connection.ExecuteScalar<int>(sql);
            }
        }

        public List<T> GetPaged(int pageNumber, int pageSize)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Получаем имя таблицы (предполагаем, что имя таблицы совпадает с именем типа)
                string tableName = typeof(T).Name;

                // SQL запрос для пагинации
                string sql = $@"
                SELECT * FROM {tableName}
                ORDER BY Id
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                // Вычисляем смещение
                int offset = (pageNumber - 1) * pageSize;

                // Выполняем запрос
                return connection.Query<T>(sql, new { Offset = offset, PageSize = pageSize }).ToList();
            }
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
                    // Dapper не требует Dispose для соединений, так как мы использу using
                }

                _disposed = true;
            }
        }

        ~DapperRepository()
        {
            Dispose(false);
        }
    }
}
