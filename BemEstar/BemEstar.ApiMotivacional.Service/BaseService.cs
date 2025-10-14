
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

        /// <summary>
        /// Cria e abre uma conexão com o banco de dados PostgreSQL.
        /// Encapsula a lógica de conexão dentro do padrão <b>Repository</b>.
        /// </summary>
        /// <returns>Conexão aberta (NpgsqlConnection).</returns>
        protected NpgsqlConnection GetConnection()
        {
            var db = new DatabaseConnection(_config);
            return db.Open();
        }

        /// <summary>
        /// Executa comandos SQL do tipo INSERT, UPDATE e DELETE.
        /// Aplica o padrão <b>Repository</b> para centralizar o acesso ao banco
        /// e reutilizar a lógica de execução de comandos parametrizados.
        /// </summary>
        /// <param name="commandText">Comando SQL parametrizado.</param>
        /// <param name="parameters">Dicionário de parâmetros SQL e valores.</param>
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

        /// <summary>
        /// Executa consultas SQL (SELECT) e retorna um leitor de dados (DataReader).
        /// Aplica o padrão <b>Template Method</b>, permitindo que subclasses personalizem a leitura dos dados.
        /// </summary>
        /// <param name="commandText">Comando SQL SELECT.</param>
        /// <param name="parameters">Parâmetros opcionais da consulta.</param>
        /// <returns>Objeto <see cref="NpgsqlDataReader"/> com os resultados da consulta.</returns>
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

        /// <summary>
        /// Cria um novo registro no banco de dados.
        /// Método virtual que pode ser sobrescrito por classes derivadas.
        /// </summary>
        public virtual void Create(T model) { }

        public virtual void Delete(int id) { }

        public virtual List<T> Read() => new List<T>();

        public virtual T ReadById(int id) => null;

        public virtual void Update(T model) { }
    }
}
