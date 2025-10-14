
using System.ComponentModel;
using BemEstar.ApiMotivacional.Data;
using BemEstar.ApiMotivacional.Models;
using Npgsql;

namespace BemEstar.ApiMotivacional.Service
{
    public class BaseService<T> : IService<T> where T : BaseModel
    {
        protected readonly DatabaseConfig _config;
        protected readonly string TableName;

        public BaseService(string tableName, DatabaseConfig config)
        {
            _config = config;
            TableName = tableName;
        }

        protected NpgsqlConnection GetConnection()
        {
            var db = new DatabaseConnection(_config);
            return db.Open();
        }

        //metodo auxiliar para executar comandos com parametros(INSERT, UPDATE, DELETE)
        public void ExecuteNonQuery(string commandText, Dictionary<string, object> parameters)
        {
            using var connection = GetConnection();
            using var command = new NpgsqlCommand(commandText, connection);

            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }
            command.ExecuteNonQuery();
        }

        //metodo para executar SELECT, ById e retornar
        public NpgsqlDataReader ExecuteReader(string commandText, Dictionary<string, object>? parameters = null)
        {
            var connection = GetConnection();
            var command = new NpgsqlCommand(commandText, connection);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            return command.ExecuteReader();
        }

       


        // metodos CRUD virtuais podem ser sobrescritos
        public virtual void Create(T model) { }

        public virtual void Delete(int id) { }

        public virtual List<T> Read() => new List<T>();

        public virtual T ReadById(int id) => null;

        public virtual void Update(T model) { }
    }
}
