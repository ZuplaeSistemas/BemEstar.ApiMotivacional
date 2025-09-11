
using System.ComponentModel;
using BemEstar.ApiMotivacional.Models;

namespace BemEstar.ApiMotivacional.Service
{
    public class BaseService<T> : IService<T> where T : BaseModel
    {
        public static List<T> list  = new List<T>();
        public void Create(T model)
        {
            list.Add(model);
        }

        public void Delete(int id)
        {
            T item = this.ReadById(id);
            list.Remove(item);
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

        public void Update(T model)
        {
            T olditem = this.ReadById(model.Id);
            this.Delete(olditem.Id);
            this.Create(model);
        }
    }
}
