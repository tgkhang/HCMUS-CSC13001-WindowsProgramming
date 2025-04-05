using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_For_Small_Shop.Services
{
    public interface IRepository<T>
    {
        List<T> GetAll(); // Phân trang, Sắp xếp, Lọc, Tìm kiếm
        T GetById(int id);
        bool Insert(T item);
        bool Update(int id, T item);
        bool Delete(int id);

        //more features
    }
}
