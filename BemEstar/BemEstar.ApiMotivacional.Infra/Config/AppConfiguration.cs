using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BemEstar.ApiMotivacional.Infra.Config
{
    public class AppConfiguration
    {
        private readonly IConfiguration _configuration;
        public AppConfiguration(IConfiguration configuration) 
        {
           this._configuration = configuration;
        }

        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("Postgres");

        }
    }
}
