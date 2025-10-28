using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BemEstar.ApiMotivacional.Infra.Repositories;

namespace BemEstar.ApiMotivacional.Service
{
    public class Service<T> : IService<T>
    {
        private IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            this._repository = repository;
        }
        public virtual int Create(T model)
        {
            return _repository.Create(model);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public List<T> Read()
        {
            return _repository.Read(); 
        }

        public T ReadById(int id)
        {
            return _repository.ReadById(id);
        }

        public void Update(T model)
        {
            _repository.Update(model);
        }
    }
}
