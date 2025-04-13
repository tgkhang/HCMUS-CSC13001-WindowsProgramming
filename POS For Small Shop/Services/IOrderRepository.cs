using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Services
{
    public interface IOrderRepository<Order> : IRepository<Order>
    {
        public List<Order> getOrdersByShiftID(int shiftId);

    }
}
