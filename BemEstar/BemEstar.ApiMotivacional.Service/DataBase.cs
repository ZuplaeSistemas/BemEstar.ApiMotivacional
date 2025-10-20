using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace BemEstar.ApiMotivacional.Service
{
    internal class DataBase
    {
        private readonly string _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING_POSTGRES");
            

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
