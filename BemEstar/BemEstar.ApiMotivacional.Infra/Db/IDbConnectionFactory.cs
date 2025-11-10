using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BemEstar.ApiMotivacional.Infra.Db
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
        IDbCommand CreateCommand(string query, IDbConnection connection);
    }
}
