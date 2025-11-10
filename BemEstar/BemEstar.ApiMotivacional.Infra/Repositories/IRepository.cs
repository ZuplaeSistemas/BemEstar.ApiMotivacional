using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BemEstar.ApiMotivacional.Infra.Repositories
{
    public interface IRepository<T>
    {
        int Create(T entity);
        List<T> Read();
        T ReadById(int id);
        void Update(T entity);
        void Delete(int id);
        bool Exists(int id);

    }
}
