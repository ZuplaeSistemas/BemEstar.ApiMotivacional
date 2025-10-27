using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BemEstar.ApiMotivacional.Infra.Db;
using BemEstar.ApiMotivacional.Models;
using Npgsql;

namespace BemEstar.ApiMotivacional.Service
{
    public class MotivacionalService : BaseService<Motivacional>
    {
         private readonly IDbConnectionFactory _dataBase;
        public MotivacionalService(IDbConnectionFactory dataBase)
        {
            this._dataBase = dataBase;
        }

        public override void Create(Motivacional model)
        {
            using NpgsqlConnection connection = (NpgsqlConnection)this._dataBase.GetConnection();

            string commandText = "INSERT INTO motivacional (texto, autor, created_at) values (@texto, @autor, @created_at)";
            using NpgsqlCommand insertCommand = new NpgsqlCommand(commandText, connection);
            
            insertCommand.Parameters.AddWithValue("texto", model.Texto);
            insertCommand.Parameters.AddWithValue("autor", model.Autor);
            insertCommand.Parameters.AddWithValue("created_at", model.CreatedAt);

            insertCommand.ExecuteNonQuery();
        }

        public override List<Motivacional> Read()
        {
            using NpgsqlConnection connection = (NpgsqlConnection)this._dataBase.GetConnection();

            string commandText = "SELECT * FROM motivacional";
            using NpgsqlCommand selectCommand = new NpgsqlCommand(commandText, connection);

            using NpgsqlDataReader dataReader = selectCommand.ExecuteReader();

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
            return list;
        }
        public override Motivacional ReadById(int id)
        {
           using NpgsqlConnection connection = (NpgsqlConnection)this._dataBase.GetConnection();

            string commandText = "SELECT * FROM motivacional WHERE id = @id";
            using NpgsqlCommand selectCommand = new NpgsqlCommand(commandText, connection);
            selectCommand.Parameters.AddWithValue("id", id);

            using NpgsqlDataReader dataReader = selectCommand.ExecuteReader();

            Motivacional motivacional = new Motivacional();
            if (dataReader.Read())
            {
                motivacional.Id = Convert.ToInt32(dataReader["id"]);
                motivacional.Texto = dataReader["texto"].ToString();
                motivacional.Autor = dataReader["autor"].ToString();
                motivacional.CreatedAt = Convert.ToDateTime(dataReader["created_at"]);
            }
            return motivacional;

        }

        public override void Delete(int id)
        {
            using NpgsqlConnection connection = (NpgsqlConnection)this._dataBase.GetConnection();

            string commandText = "DELETE FROM motivacional WHERE id = @id";
            using NpgsqlCommand deleteCommand = new NpgsqlCommand(commandText, connection);
            deleteCommand.Parameters.AddWithValue("id", id);

            deleteCommand.ExecuteNonQuery();

        }

        public override void Update(Motivacional model)
        {
            using NpgsqlConnection connection = (NpgsqlConnection)this._dataBase.GetConnection();

            string commandText = "UPDATE motivacional SET texto = @texto, autor = @autor WHERE id = @id";
            using NpgsqlCommand updateCommand = new NpgsqlCommand(commandText, connection);

            updateCommand.Parameters.AddWithValue("texto", model.Texto);
            updateCommand.Parameters.AddWithValue("autor", model.Autor);
            updateCommand.Parameters.AddWithValue("id", model.Id);

            updateCommand.ExecuteNonQuery();
        }
    }
}
