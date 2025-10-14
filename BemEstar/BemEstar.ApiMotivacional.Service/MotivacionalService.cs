using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BemEstar.ApiMotivacional.Data;
using BemEstar.ApiMotivacional.Models;
using Npgsql;

namespace BemEstar.ApiMotivacional.Service
{
    public class MotivacionalService : BaseService<Motivacional>
    {
        public MotivacionalService(DatabaseConfig config) : base ("motivacional", config){}

        public override List<Motivacional> Read()
        {
            string commantText = SqlQuery.SelectAll(TableName);
            
            using var dataReader = ExecuteReader(commantText);

            return MotivacionalList(dataReader);

        }

        public override void Create(Motivacional model)
        {
            string commandText = SqlQuery.Insert(TableName, new[] { "texto", "autor", "created_at" });
            var parameters = new Dictionary<string, object>
            {
                {"texto", model.Texto },
                { "autor", model.Autor },
                { "created_at", model.CreatedAt }

            };
            ExecuteNonQuery(commandText, parameters);

        }

        public override Motivacional ReadById(int id)
        {
            string commandText = SqlQuery.SelectById(TableName);
            var parameters = new Dictionary<string, object>() { {  "id", id } };

            using var dataReader  = ExecuteReader(commandText, parameters);

            return MotivacionalList(dataReader).FirstOrDefault();

        }


        public override void Update(Motivacional model)
        {
            string commandText = SqlQuery.Update(TableName, new[] { "texto", "autor", "created_at" });
            var parameters = new Dictionary<string, object>
            {
                { "texto", model.Texto },
                { "autor", model.Autor },
                { "created_at", model.CreatedAt },
                { "id", model.Id }
            };
            ExecuteNonQuery(commandText, parameters);
        }

        public override void Delete(int id)
        {
            string commandText = SqlQuery.Delete(TableName);
            var parameters = new Dictionary<string, object> { { "id", id } };
            ExecuteNonQuery(commandText, parameters);

        }

        //metodo auxiliares
        private List<Motivacional> MotivacionalList(NpgsqlDataReader dataReader)
        {
            var list = new List<Motivacional>();

            while (dataReader.Read())
            {
                var motivacional = new Motivacional
                {
                    Id = Convert.ToInt32(dataReader["id"]),
                    Texto = dataReader["texto"].ToString(),
                    Autor = dataReader["autor"].ToString(),
                    CreatedAt = Convert.ToDateTime(dataReader["created_at"])
                };

                list.Add(motivacional);
            }

            return list;
        }

      

    }
}
