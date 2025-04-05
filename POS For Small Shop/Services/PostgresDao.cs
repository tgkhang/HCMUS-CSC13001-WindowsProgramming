using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services.Repository;

namespace POS_For_Small_Shop.Services
{
    public class PostgresDao : IDao
    {
        public IRepository<Category> Categories { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Promotion> Promotions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Shift> Shifts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Ingredient> Ingredients { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<MenuItem> MenuItems { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Order> Orders { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<OrderDetail> OrderDetails { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Transaction> Transactions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<CashFlow> CashFlows { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Notification> Notifications { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<ShiftOrder> ShiftOrders { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Customer> Customers { get; set; } = new PostgresCustomerRepository();
    }
}
