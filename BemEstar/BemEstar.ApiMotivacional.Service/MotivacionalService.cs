using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BemEstar.ApiMotivacional.Infra.Db;
using BemEstar.ApiMotivacional.Infra.Repositories;
using BemEstar.ApiMotivacional.Models;
using Npgsql;

namespace BemEstar.ApiMotivacional.Service
{
    public class MotivacionalService : Service<Motivacional>
    {
        public MotivacionalService(MotivacionalRepository repository): base(repository){}

    }

}
