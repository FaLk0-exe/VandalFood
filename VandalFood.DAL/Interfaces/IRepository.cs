using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandalFood.DAL.Interfaces
{
    public interface IRepository<T>
    {
        public T Get(int id);
        public IEnumerable<T> Get();
        public void Update(T entity);
        public void Delete(T entity);
        public void Create(T entity);
    }
}
