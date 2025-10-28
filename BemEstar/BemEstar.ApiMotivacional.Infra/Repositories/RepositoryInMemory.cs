using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BemEstar.ApiMotivacional.Models;

namespace BemEstar.ApiMotivacional.Infra.Repositories
{
    public class RepositoryInMemory<T> : IRepository<T> where T : BaseModel
    {
        public static List<T> list {  get; set; } = new List<T>();
        private static int _currentId = 0;
        public int Create(T entity)
        {
            entity.Id = ++_currentId;
            list.Add(entity);
            return entity.Id;

        }

        public void Delete(int id)
        {
            T item = this.ReadById(id);
            list.Remove(item); 
        }

        public bool Exists(int id)
        {
            return list.Exists(i => i.Id == id);

        }

        public List<T> Read()
        {
            return list; 
        }

        public T ReadById(int id)
        {
            T item = list.FirstOrDefault(i => i.Id == id);
            return item;
        }

        public void Update(T entity)
        {
            T olditem = this.ReadById(entity.Id);
            this.Delete(olditem.Id);
            this.Create(entity);
        }
    }

}
