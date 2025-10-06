using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BemEstar.ApiMotivacional.Models;
using Npgsql;

namespace BemEstar.ApiMotivacional.Service
{
    public class MotivacionalService : BaseService<Motivacional>
    {
        private readonly string _connectionString = "Host=18.220.9.40;Port=5432;Database=motivacional;Username=postgres;Password=123456";

      
        public override void Create(Motivacional model)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string commandText = "INSERT INTO motivacional (texto, autor, created_at) values (@texto, @autor, @created_at)";
            NpgsqlCommand insertCommand = new NpgsqlCommand(commandText, connection);
            
            insertCommand.Parameters.AddWithValue("texto", model.Texto);
            insertCommand.Parameters.AddWithValue("autor", model.Autor);
            insertCommand.Parameters.AddWithValue("created_at", model.CreatedAt);

            insertCommand.ExecuteNonQuery();
            connection.Close();

        }

        public override List<Motivacional> Read()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string commandText = "SELECT * FROM motivacional";
            NpgsqlCommand selectCommand = new NpgsqlCommand(commandText, connection);

            NpgsqlDataReader dataReader = selectCommand.ExecuteReader();

            List<Motivacional> list = new List<Motivacional>();

            while (dataReader.Read())
            {
                Motivacional motivacional = new Motivacional();
                motivacional.Id = Convert.ToInt32(dataReader["id"]);
                motivacional.Texto = dataReader["texto"].ToString();
                motivacional.Autor = dataReader["autor"].ToString();
                motivacional.CreatedAt = Convert.ToDateTime(dataReader["created_at"]);

                list.Add(motivacional);
            }

            connection.Close();
            return list;


        }
        public override Motivacional ReadById(int id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string commandText = "SELECT * FROM motivacional WHERE id = @id";
            NpgsqlCommand selectCommand = new NpgsqlCommand(commandText, connection);
            selectCommand.Parameters.AddWithValue("id", id);

            NpgsqlDataReader dataReader = selectCommand.ExecuteReader();

            Motivacional motivacional = new Motivacional();
            if (dataReader.Read())
            {
                motivacional.Id = Convert.ToInt32(dataReader["id"]);
                motivacional.Texto = dataReader["texto"].ToString();
                motivacional.Autor = dataReader["autor"].ToString();
                motivacional.CreatedAt = Convert.ToDateTime(dataReader["created_at"]);
            }
            connection.Close();
            return motivacional;

        }

        public override void Delete(int id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            string commandText = "DELETE FROM motivacional WHERE id = @id";
            NpgsqlCommand deleteCommand = new NpgsqlCommand(commandText, connection);
            deleteCommand.Parameters.AddWithValue("id", id);

            deleteCommand.ExecuteNonQuery();
            connection.Close();

        }


        public override void Update(Motivacional model)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string commandText = "UPDATE motivacional SET texto = @texto, autor = @autor WHERE id = @id";
            NpgsqlCommand updateCommand = new NpgsqlCommand(commandText, connection);

            updateCommand.Parameters.AddWithValue("texto", model.Texto);
            updateCommand.Parameters.AddWithValue("autor", model.Autor);
            updateCommand.Parameters.AddWithValue("id", model.Id);

            updateCommand.ExecuteNonQuery();
            connection.Close();
        }
    }
}
