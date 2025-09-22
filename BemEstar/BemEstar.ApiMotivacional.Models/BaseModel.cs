using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BemEstar.ApiMotivacional.Models
{
    public class BaseModel
    {
        private static int _nextId = 1; // contador global

        public int Id { get; private set; }

        public BaseModel()
        {
            Id = _nextId++;
        }
    }
}
