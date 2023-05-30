using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandalFood.BLL.Interfaces
{
    public interface IValidator<T> where T : class
    {
        public void Validate(T entity);
    }
}
