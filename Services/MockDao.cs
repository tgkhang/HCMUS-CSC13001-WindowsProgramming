using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Services
{
    public class MockDao : IDao
    {
        public IRepository<Category> Categories { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Customer> Customers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Promotion> Promotions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Shift> Shifts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public IRepository<Ingredient> Ingredients { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Order> Orders { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<OrderDetail> OrderDetails { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Transaction> Transactions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<CashFlow> CashFlows { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Notification> Notifications { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<ShiftOrder> ShiftOrders { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }



        //Done
        public IRepository<MenuItem> MenuItems { get; set; } = new MockMenuItemRepository();
        public IRepository<Ingredient> Ingredients { get; set; } = new MockIngredientRepository();
    }


    public class MockMenuItemRepository : IRepository<MenuItem>
    {
        private List<MenuItem> _menuItems = new List<MenuItem>
        {
            new MenuItem { MenuItemID = 1, Name = "Espresso", CategoryID = 1, SellingPrice = 2.50f, ImagePath = "Assets/espresso.png" },
            new MenuItem { MenuItemID = 2, Name = "Cappuccino", CategoryID = 1, SellingPrice = 3.00f, ImagePath = "Assets/cappuccino.png" },
            new MenuItem { MenuItemID = 3, Name = "Latte", CategoryID = 1, SellingPrice = 3.50f, ImagePath = "Assets/latte.png" },
            new MenuItem { MenuItemID = 4, Name = "Croissant", CategoryID = 2, SellingPrice = 2.00f, ImagePath = "Assets/croissant.png" },
            new MenuItem { MenuItemID = 5, Name = "Muffin", CategoryID = 2, SellingPrice = 2.20f, ImagePath = "Assets/muffin.png" },
        };

        public List<MenuItem> GetAll()
        {
            return _menuItems.ToList(); // Return a copy
        }

        public MenuItem GetById(int id)
        {
            return _menuItems.FirstOrDefault(x => x.MenuItemID == id);
        }



        public bool Insert(MenuItem item)
        {
            // Auto-generate ID
            item.MenuItemID = _menuItems.Count > 0 ? _menuItems.Max(x => x.MenuItemID) + 1 : 1;
            _menuItems.Add(item);
            return true;
        }

        public bool Update(int id, MenuItem item)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            existing.Name = item.Name;
            existing.CategoryID = item.CategoryID;
            existing.SellingPrice = item.SellingPrice;
            existing.ImagePath = item.ImagePath;
            return true;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            _menuItems.Remove(existing);
            return true;
        }


    }

    public class MockIngredientRepository : IRepository<Ingredient>
    {
        private List<Ingredient> _ingredients = new List<Ingredient>
        {
            new Ingredient { IngredientID = 1, IngredientName = "Coffee Beans", CategoryID = 1, Stock = 10, Unit = "kg", PurchasePrice = 5.0f, Supplier = "Supplier A", ExpiryDate = new DateTime(2025, 12, 31) },
            new Ingredient { IngredientID = 2, IngredientName = "Milk", CategoryID = 2, Stock = 20, Unit = "L", PurchasePrice = 1.5f, Supplier = "Supplier B", ExpiryDate = new DateTime(2025, 11, 30) }
        };

        public List<Ingredient> GetAll()
        {
            return _ingredients.ToList();
        }

        public Ingredient GetById(int id)
        {
            return _ingredients.FirstOrDefault(x => x.IngredientID == id);
        }

        public bool Insert(Ingredient item)
        {
            item.IngredientID = _ingredients.Count > 0 ? _ingredients.Max(x => x.IngredientID) + 1 : 1;
            _ingredients.Add(item);
            return true;
        }

        public bool Update(int id, Ingredient item)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            existing.IngredientName = item.IngredientName;
            existing.CategoryID = item.CategoryID;
            existing.Stock = item.Stock;
            existing.Unit = item.Unit;
            existing.PurchasePrice = item.PurchasePrice;
            existing.Supplier = item.Supplier;
            existing.ExpiryDate = item.ExpiryDate;

            return true;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            _ingredients.Remove(existing);
            return true;
        }
    }

}
