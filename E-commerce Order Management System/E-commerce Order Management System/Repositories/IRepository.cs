using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Order_Management_System.Repositories
{
    public interface IRepository<T>
    {
        void Add(T item);
        T GetById(string id);
        List<T> GetAll();
        void Update(T item);
        void Delete(string id);
    }
}
