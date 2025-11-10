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

        public virtual void Delete(int id)
        {
            _repository.Delete(id);
        }

        public virtual bool Exists(int id)
        {
            return _repository.Exists(id);
        }

        public virtual List<T> Read()
        {
            return _repository.Read(); 
        }

        public virtual T ReadById(int id)
        {
            return _repository.ReadById(id);
        }

        public virtual void Update(T model)
        {
            _repository.Update(model);
        }
    }
}
