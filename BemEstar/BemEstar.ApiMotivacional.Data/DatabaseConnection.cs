using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace BemEstar.ApiMotivacional.Data
{
    public class DatabaseConnection : IDisposable
    {
        private NpgsqlConnection _connection;

        public DatabaseConnection(DatabaseConfig config)
        {
            _connection = new NpgsqlConnection(config.ConnectionString);
        }

        //abre a conexão se estiver fechada
        public NpgsqlConnection Open()
        {
            if (_connection.State != ConnectionState.Open) 
                _connection.Open();
            return _connection;
        }

        //fecha a conexão se estiver aberta
        public void Close()
        {
            if ( _connection.State != ConnectionState.Closed) 
                _connection.Close();
        }

    
        public void Dispose()
        {
            Close();
            _connection.Dispose();
        }
    }
}
