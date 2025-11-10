using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BemEstar.ApiMotivacional.Infra.Db;
using BemEstar.ApiMotivacional.Models;

namespace BemEstar.ApiMotivacional.Infra.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseModel
    {
        private IDbConnectionFactory _connectionFactory;
        private readonly string tablename = typeof(T).Name.ToLower();

        public BaseRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public int Create(T entity)
        {
            using var connection = _connectionFactory.GetConnection();
            using var insertCommand = _connectionFactory.CreateCommand("", connection);

            var props = entity.GetType().GetProperties();
            string columns = string.Join(",", props.Where(p => p.Name != "Id").Select(p => p.Name.ToLower()));
            string parameters = string.Join(",", props.Where(p => p.Name != "Id").Select(p => "@" + p.Name.ToLower()));
            var propertiesValues = props.Where(p => p.Name != "Id")
                .ToDictionary(p => p.Name.ToLower(), p => p.GetValue(entity, null));

            insertCommand.CommandText = $"INSERT INTO {tablename} ({columns}) VALUES ({parameters})";

            foreach (var item in propertiesValues)
            {
                AddParameter(insertCommand, "@" + item.Key, item.Value);
            }

            return insertCommand.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = _connectionFactory.GetConnection();
            using var deleteCommand = _connectionFactory.CreateCommand("", connection);

            deleteCommand.CommandText = $"DELETE FROM {tablename} WHERE id = @id";

            AddParameter(deleteCommand, "id", id);


            deleteCommand.ExecuteNonQuery();

        }

        public bool Exists(int id)
        {
            using var connection = _connectionFactory.GetConnection();
            using var existsCommand = _connectionFactory.CreateCommand("", connection);

            existsCommand.CommandText = $"SELECT 1 FROM {tablename} WHERE id = @id LIMIT 1";

            AddParameter(existsCommand, "id", id);

            using var dataReader = existsCommand.ExecuteReader();
            return dataReader.Read();
        }

        public List<T> Read()
        {
            using var connection = _connectionFactory.GetConnection();
            using var selectCommand = _connectionFactory.CreateCommand("", connection);

            selectCommand.CommandText = $"SELECT * FROM {tablename}";
            using var dataReader = selectCommand.ExecuteReader();

            List<T> list = new List<T>();
            var props = typeof(T).GetProperties(); //pega as propriedades da entidade

            while (dataReader.Read())
            {
                T entity = (T)Activator.CreateInstance(typeof(T)); //pega o tipo da entidade tranforma em objeto

                foreach (var prop in props)
                {
                    if (dataReader[prop.Name.ToLower()] == null)
                        continue;

                    var colValue = dataReader[prop.Name.ToLower()];
                    if (colValue != DBNull.Value)
                        prop.SetValue(entity, Convert.ChangeType(colValue, prop.PropertyType));
                }

                list.Add(entity);
            }
            return list;
        }

        public T ReadById(int id)
        {
            using var connection = _connectionFactory.GetConnection();
            using var selectCommand = _connectionFactory.CreateCommand("", connection);

            selectCommand.CommandText = $"SELECT * FROM {tablename} WHERE id = @id";

            AddParameter(selectCommand, "id", id);

            using var dataReader = selectCommand.ExecuteReader();

            T entity = (T)Activator.CreateInstance(typeof(T));
            var props = typeof(T).GetProperties();

            if (dataReader.Read())
            {
                foreach (var prop in props)
                {
                    if (dataReader[prop.Name.ToLower()] == null)
                        continue;

                    var colValue = dataReader[prop.Name.ToLower()];
                    if (colValue != DBNull.Value)
                        prop.SetValue(entity, Convert.ChangeType(colValue, prop.PropertyType));
                }

            }
            return entity;
        }

        public void Update(T entity)
        {
            using var connection = _connectionFactory.GetConnection();
            using var updateCommand = _connectionFactory.CreateCommand("", connection);

            var props = entity.GetType().GetProperties();
            string setClause = string.Join(",", props.Where(p => p.Name != "Id").Select(p => $"{p.Name.ToLower()} = @{p.Name.ToLower()}"));

            updateCommand.CommandText = $"UPDATE {tablename} SET {setClause} WHERE id = @id";
            var propertiesValues = props.ToDictionary(p => p.Name.ToLower(), p => p.GetValue(entity, null));

            foreach (var item in propertiesValues)
            {
                AddParameter(updateCommand, "@" + item.Key, item.Value);

            }

            updateCommand.ExecuteNonQuery();
        }

        //metodos auxiliares
        public static void AddParameter(IDbCommand command, string parameterName, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }
    }
}
