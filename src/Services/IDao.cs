using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Services
{
    public interface IDao
    {
        IRepository<Category> Categories { get; set; }
        IRepository<Customer> Customers { get; set; }
        IRepository<Promotion> Promotions { get; set; }
        IRepository<Shift> Shifts { get; set; }
        IRepository<Ingredient> Ingredients { get; set; }
        IRepository<MenuItem> MenuItems { get; set; }
        IRepository<Order> Orders { get; set; }
        IRepository<OrderDetail> OrderDetails { get; set; }
        IRepository<Transaction> Transactions { get; set; }
        IRepository<CashFlow> CashFlows { get; set; }
        IRepository<Notification> Notifications { get; set; }
        IRepository<ShiftOrder> ShiftOrders { get; set; }
    }
}
