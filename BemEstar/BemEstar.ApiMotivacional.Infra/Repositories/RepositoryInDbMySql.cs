using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BemEstar.ApiMotivacional.Infra.Db;
using BemEstar.ApiMotivacional.Models;
using MySql.Data.MySqlClient;
using Npgsql;

namespace BemEstar.ApiMotivacional.Infra.Repositories
{
    public class RepositoryInDbMySql<T> : IRepository<T> where T : BaseModel
    {
        private IDbConnectionFactory _connectionFactory;
        private readonly string tablename = typeof(T).Name.ToLower();

        public RepositoryInDbMySql(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public int Create(T entity)
        {
            //pega conexão com MySql
            using MySqlConnection connection = (MySqlConnection)this._connectionFactory.GetConnection();
            
            var props = entity.GetType().GetProperties();
            string columns = string.Join(",", props.Where(c => c.Name != "Id").Select(c => c.Name.ToLower()));
            string parameters = string.Join(",", props.Where(p => p.Name != "Id").Select(p => "@" + p.Name.ToLower()));
            var propertiesValues = props.Where(p => p.Name != "Id").ToDictionary(p => p.Name.ToLower(), p => p.GetValue(entity, null));

            string commandText = $"INSERT INTO {tablename} ({columns}) values ({parameters})";
            using MySqlCommand insertCommand = new(commandText, connection);

            foreach (var item in propertiesValues)
            {
                insertCommand.Parameters.AddWithValue(item.Key, item.Value);
            }
            insertCommand.ExecuteNonQuery();
            return 0;
        }

        public void Delete(int id)
        {
            using MySqlConnection connection = (MySqlConnection)this._connectionFactory.GetConnection();
            
            string commandText = $"DELETE FROM {tablename} WHERE id = @id";
            using MySqlCommand deleteCommand = new(commandText, connection);
            deleteCommand.Parameters.AddWithValue("id", id);

            deleteCommand.ExecuteNonQuery();
        }

        public bool Exists(int id)
        {
            using MySqlConnection connection = (MySqlConnection)this._connectionFactory.GetConnection();
            
            string commandText = $"SELECT 1 FROM {tablename} WHERE id = @id LIMIT 1";
            using MySqlCommand existsCommand = new(commandText, connection);
            existsCommand.Parameters.AddWithValue("id", id);

            using MySqlDataReader dataReader = existsCommand.ExecuteReader();
            return dataReader.Read();
        }

        public List<T> Read()
        {
            //pega conexão com MySql
            using MySqlConnection connection = (MySqlConnection)this._connectionFactory.GetConnection();
            
            string commandText = $"SELECT * FROM {tablename}";
            using MySqlCommand selectCommand = new(commandText, connection);

            using MySqlDataReader dataReader = selectCommand.ExecuteReader();

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
            using MySqlConnection connection = (MySqlConnection)this._connectionFactory.GetConnection();
            
            string commandText = $"SELECT * FROM {tablename} WHERE id = @id";
            using MySqlCommand selectCommand = new(commandText, connection);
            selectCommand.Parameters.AddWithValue("id", id);

            using MySqlDataReader dataReader = selectCommand.ExecuteReader();

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
            using MySqlConnection connection = (MySqlConnection)this._connectionFactory.GetConnection();
            
            var props = entity.GetType().GetProperties();
            string setClause = string.Join(",", props.Where(p => p.Name != "Id").Select(p => $"{p.Name.ToLower()} = @{p.Name.ToLower()}"));

            string commandText = $"UPDATE {tablename} SET {setClause} WHERE id = @id";
            using MySqlCommand updateCommand = new(commandText, connection);

            var propertiesValues = props.ToDictionary(p => p.Name.ToLower(), p => p.GetValue(entity, null));

            foreach (var item in propertiesValues)
            {
                updateCommand.Parameters.AddWithValue(item.Key, item.Value);

            }

            updateCommand.ExecuteNonQuery();
        }
    }
}
