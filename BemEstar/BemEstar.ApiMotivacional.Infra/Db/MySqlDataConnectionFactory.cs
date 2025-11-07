using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BemEstar.ApiMotivacional.Infra.Config;
using MySql.Data.MySqlClient;

namespace BemEstar.ApiMotivacional.Infra.Db
{
    public class MySqlDataConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public MySqlDataConnectionFactory(AppConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySql");
        }

        //Padrão de projeto Factory Method MySql
        public IDbConnection GetConnection()
        {
            MySqlConnection connection = new(_connectionString);
            connection.Open();
            return connection;

        }
    }
}
