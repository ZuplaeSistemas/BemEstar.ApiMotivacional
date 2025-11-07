using System.Data;
using BemEstar.ApiMotivacional.Infra.Config;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BemEstar.ApiMotivacional.Infra.Db
{
    public class NpgsqlConnectionFactory :  IDbConnectionFactory
    {
        private readonly string _connectionString;

        public NpgsqlConnectionFactory(AppConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Postgres");
        }

        //Padrão de projeto Factory Method

        public IDbConnection GetConnection()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }



    }
}
