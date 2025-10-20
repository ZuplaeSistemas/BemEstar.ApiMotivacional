using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BemEstar.ApiMotivacional.Service
{
    internal class DataBase
    {
        private readonly string _connectionString;

        public DataBase()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<DataBase>()
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            this._connectionString = configuration.GetConnectionString("Postgres");
        }
        public NpgsqlConnection GetConnection()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public void CloseConnection(NpgsqlConnection connection)
        {
            connection.Close();
        }
    }
}
