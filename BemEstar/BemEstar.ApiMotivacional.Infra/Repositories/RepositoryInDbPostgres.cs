using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BemEstar.ApiMotivacional.Infra.Db;
using BemEstar.ApiMotivacional.Models;
using Npgsql;

namespace BemEstar.ApiMotivacional.Infra.Repositories
{
    public class RepositoryInDbPostgres : BaseRepository<Motivacional>
    {
        public RepositoryInDbPostgres(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}
