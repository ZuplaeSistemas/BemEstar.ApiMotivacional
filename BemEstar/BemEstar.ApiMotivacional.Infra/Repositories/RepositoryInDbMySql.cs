using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BemEstar.ApiMotivacional.Infra.Db;
using BemEstar.ApiMotivacional.Models;
using MySql.Data.MySqlClient;
using Npgsql;

namespace BemEstar.ApiMotivacional.Infra.Repositories
{
    public class RepositoryInDbMySql : BaseRepository<Motivacional>
    {
        public RepositoryInDbMySql(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}
