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
        private DataBase _dataBase;
        public MotivacionalService()
        {
            this._dataBase = new DataBase();
        }


        public override void Create(Motivacional model)
        {
            NpgsqlConnection connection = _dataBase.GetConnection();
            string commandText = "INSERT INTO motivacional (texto, autor, created_at) values (@texto, @autor, @created_at)";
            NpgsqlCommand insertCommand = new NpgsqlCommand(commandText, connection);
            
            insertCommand.Parameters.AddWithValue("texto", model.Texto);
            insertCommand.Parameters.AddWithValue("autor", model.Autor);
            insertCommand.Parameters.AddWithValue("created_at", model.CreatedAt);

            insertCommand.ExecuteNonQuery();
            _dataBase.CloseConnection(connection);

        }

        public override List<Motivacional> Read()
        {
            NpgsqlConnection connection = _dataBase.GetConnection();

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

            _dataBase.CloseConnection(connection);
            return list;


        }
        public override Motivacional ReadById(int id)
        {
            NpgsqlConnection connection = _dataBase.GetConnection();

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
            _dataBase.CloseConnection(connection);
            return motivacional;

        }

        public override void Delete(int id)
        {
            NpgsqlConnection connection = _dataBase.GetConnection();

            string commandText = "DELETE FROM motivacional WHERE id = @id";
            NpgsqlCommand deleteCommand = new NpgsqlCommand(commandText, connection);
            deleteCommand.Parameters.AddWithValue("id", id);

            deleteCommand.ExecuteNonQuery();
            _dataBase.CloseConnection(connection);

        }


        public override void Update(Motivacional model)
        {
            NpgsqlConnection connection = _dataBase.GetConnection();

            string commandText = "UPDATE motivacional SET texto = @texto, autor = @autor WHERE id = @id";
            NpgsqlCommand updateCommand = new NpgsqlCommand(commandText, connection);

            updateCommand.Parameters.AddWithValue("texto", model.Texto);
            updateCommand.Parameters.AddWithValue("autor", model.Autor);
            updateCommand.Parameters.AddWithValue("id", model.Id);

            updateCommand.ExecuteNonQuery();
            _dataBase.CloseConnection(connection);
        }
    }
}
