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
        public IRepository<Category> Categories { get; set; } = new PostgresCategoryRepository();
        public IRepository<Promotion> Promotions { get; set; } = new PostgresPromotionRepository();
        public IRepository<Shift> Shifts { get; set; } = new PostgresShiftRepository();
        public IRepository<Ingredient> Ingredients { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IOrderRepository<Order> Orders { get; set; } = new PostgresOrderRepository();
        public IRepository<OrderDetail> OrderDetails { get; set; } = new PostgresOrderDetailRepository();
        public IRepository<Transaction> Transactions { get; set; } = new PostgresTransactionRepository();
        public IRepository<CashFlow> CashFlows { get; set; } = new PostgresCashFlowRepository();
        public IRepository<Notification> Notifications { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<ShiftOrder> ShiftOrders { get; set; } = new PostgresShiftOrderRepository();
        public IRepository<Customer> Customers { get; set; } = new PostgresCustomerRepository();
        public IRepository<MenuItem> MenuItems { get; set; } = new PostgresMenuItemRepository();
    }
}
