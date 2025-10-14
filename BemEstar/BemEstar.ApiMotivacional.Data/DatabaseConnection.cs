using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace BemEstar.ApiMotivacional.Data
{
    /// <summary>
    /// Responsável por gerenciar a conexão com o banco de dados PostgreSQL.
    /// </summary>
    /// <remarks>
    /// Esta classe aplica o padrão de projeto <b>IDisposable</b>, garantindo que a 
    /// conexão com o banco seja corretamente aberta, fechada e liberada da memória.
    /// 
    /// Ela também encapsula o acesso ao banco, promovendo separação de responsabilidades
    /// e reutilização dentro da camada de dados.
    /// </remarks>
    public class DatabaseConnection : IDisposable
    {
        /// <summary>
        /// Objeto responsável por manter a conexão com o banco de dados PostgreSQL.
        /// </summary>
        private NpgsqlConnection _connection;

        /// <summary>
        /// Inicializa a instância da classe utilizando as configurações definidas em <see cref="DatabaseConfig"/>.
        /// </summary>
        /// <param name="config">Objeto contendo a string de conexão com o banco de dados.</param>
        public DatabaseConnection(DatabaseConfig config)
        {
            _connection = new NpgsqlConnection(config.ConnectionString);
        }

        /// <summary>
        /// Abre a conexão com o banco de dados caso ela ainda não esteja aberta.
        /// </summary>
        /// <returns>Instância aberta de <see cref="NpgsqlConnection"/>.</returns>
        public NpgsqlConnection Open()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            return _connection;
        }

        /// <summary>
        /// Fecha a conexão com o banco de dados caso ainda esteja aberta.
        /// </summary>
        public void Close()
        {
            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
        }

        /// <summary>
        /// Libera todos os recursos utilizados pela conexão.
        /// </summary>
        /// <remarks>
        /// Este método segue o padrão <b>IDisposable</b>, garantindo que 
        /// a conexão seja fechada e removida da memória corretamente.
        /// </remarks>
        public void Dispose()
        {
            Close();
            _connection.Dispose();
        }
    }
}
