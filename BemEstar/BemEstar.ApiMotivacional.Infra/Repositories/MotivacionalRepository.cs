using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BemEstar.ApiMotivacional.Infra.Db;
using BemEstar.ApiMotivacional.Models;

namespace BemEstar.ApiMotivacional.Infra.Repositories
{
    public class MotivacionalRepository : RepositoryInDbPostgres
    {
        public MotivacionalRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}
