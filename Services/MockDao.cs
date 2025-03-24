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
        public IRepository<Promotion> Promotions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Shift> Shifts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<OrderDetail> OrderDetails { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Transaction> Transactions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<CashFlow> CashFlows { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Notification> Notifications { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<ShiftOrder> ShiftOrders { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //Done
        public IRepository<MenuItem> MenuItems { get; set; } = new MockMenuItemRepository();
        public IRepository<Category> Categories { get; set; } = new MockCategoryRepository();
        public IRepository<Customer> Customers { get; set; } = new MockCustomerRepository();
        public IRepository<Order> Orders { get; set; } = new  MockOrderRepository();


        public IRepository<Ingredient> Ingredients { get; set; } = new MockIngredientRepository();

    }

    public class MockCategoryRepository : BaseMockRepository<Category>
    {
        public MockCategoryRepository() : base(x => x.CategoryID, (x, id) => x.CategoryID = id)
        {
            _items = new List<Category>
            {
                new Category { CategoryID = 1, Name = "Coffee", Description = "Hot coffee drinks" },
                new Category { CategoryID = 2, Name = "Tea", Description = "Various tea options" },
                new Category { CategoryID = 3, Name = "Cold Drinks", Description = "Iced coffee, smoothies, and cold beverages" },
                new Category { CategoryID = 4, Name = "Pastries", Description = "Fresh baked goods" },
                new Category { CategoryID = 5, Name = "Snacks", Description = "Light meals and snacks" }
            };
        }
    }

    public class MockCustomerRepository : BaseMockRepository<Customer>
    {
        public MockCustomerRepository() : base(x => x.CustomerID, (x, id) => x.CustomerID = id)
        {
            _items = new List<Customer>
            {
                new Customer { CustomerID = 1, Name = "John Doe", Phone = "0123456789", Email = "john@example.com", Address = "123 Main St", LoyaltyPoints = 150 },
                new Customer { CustomerID = 2, Name = "Jane Smith", Phone = "0987654321", Email = "jane@example.com", Address = "456 Oak Ave", LoyaltyPoints = 75 },
                new Customer { CustomerID = 3, Name = "Bob Johnson", Phone = "0369852147", Email = "bob@example.com", Address = "789 Pine Rd", LoyaltyPoints = 200 }
            };
        }
    }


    public class MockMenuItemRepository : BaseMockRepository<MenuItem>
    {
        public MockMenuItemRepository() : base(x => x.MenuItemID, (x, id) => x.MenuItemID = id)
        {
            _items = new List<MenuItem>
            {
                // Coffee (CategoryID = 1)
                new MenuItem { MenuItemID = 1, Name = "Espresso", CategoryID = 1, SellingPrice = 25000f, ImagePath = "Assets/espresso.png" },
                new MenuItem { MenuItemID = 2, Name = "Cappuccino", CategoryID = 1, SellingPrice = 35000f, ImagePath = "Assets/cappuccino.png" },
                new MenuItem { MenuItemID = 3, Name = "Latte", CategoryID = 1, SellingPrice = 35000f, ImagePath = "Assets/latte.png" },
                new MenuItem { MenuItemID = 4, Name = "Americano", CategoryID = 1, SellingPrice = 30000f, ImagePath = "Assets/americano.png" },
                new MenuItem { MenuItemID = 5, Name = "Mocha", CategoryID = 1, SellingPrice = 40000f, ImagePath = "Assets/mocha.png" },
            
                // Tea (CategoryID = 2)
                new MenuItem { MenuItemID = 6, Name = "Green Tea", CategoryID = 2, SellingPrice = 25000f, ImagePath = "Assets/green-tea.png" },
                new MenuItem { MenuItemID = 7, Name = "Black Tea", CategoryID = 2, SellingPrice = 25000f, ImagePath = "Assets/black-tea.png" },
                new MenuItem { MenuItemID = 8, Name = "Jasmine Tea", CategoryID = 2, SellingPrice = 30000f, ImagePath = "Assets/jasmine-tea.png" },
            
                // Cold Drinks (CategoryID = 3)
                new MenuItem { MenuItemID = 9, Name = "Iced Coffee", CategoryID = 3, SellingPrice = 30000f, ImagePath = "Assets/iced-coffee.png" },
                new MenuItem { MenuItemID = 10, Name = "Frappuccino", CategoryID = 3, SellingPrice = 45000f, ImagePath = "Assets/frappuccino.png" },
                new MenuItem { MenuItemID = 11, Name = "Lemonade", CategoryID = 3, SellingPrice = 25000f, ImagePath = "Assets/lemonade.png" },
            
                // Pastries (CategoryID = 4)
                new MenuItem { MenuItemID = 12, Name = "Croissant", CategoryID = 4, SellingPrice = 20000f, ImagePath = "Assets/croissant.png" },
                new MenuItem { MenuItemID = 13, Name = "Muffin", CategoryID = 4, SellingPrice = 22000f, ImagePath = "Assets/muffin.png" },
                new MenuItem { MenuItemID = 14, Name = "Danish", CategoryID = 4, SellingPrice = 25000f, ImagePath = "Assets/danish.png" },
            
                // Snacks (CategoryID = 5)
                new MenuItem { MenuItemID = 15, Name = "Sandwich", CategoryID = 5, SellingPrice = 35000f, ImagePath = "Assets/sandwich.png" },
                new MenuItem { MenuItemID = 16, Name = "Bagel", CategoryID = 5, SellingPrice = 30000f, ImagePath = "Assets/bagel.png" },
                new MenuItem { MenuItemID = 17, Name = "Salad", CategoryID = 5, SellingPrice = 40000f, ImagePath = "Assets/salad.png" },
            };
        }
    }

    public class MockOrderRepository : BaseMockRepository<Order>
    {
        public MockOrderRepository() : base(x => x.OrderID, (x, id) => x.OrderID = id)
        {
            _items = new List<Order>
        {
            new Order
            {
                OrderID = 1,
                CustomerID = 1,
                ShiftID = 1,
                TotalAmount = 100000f,
                Discount = 10000f,
                FinalAmount = 90000f,
                PaymentMethod = "Cash",
                Status = "Completed"
            },
            new Order
            {
                OrderID = 2,
                CustomerID = 2,
                ShiftID = 1,
                TotalAmount = 75000f,
                Discount = 5000f,
                FinalAmount = 70000f,
                PaymentMethod = "Credit Card",
                Status = "Pending"
            },
            new Order
            {
                OrderID = 3,
                CustomerID = 3,
                ShiftID = 2,
                TotalAmount = 120000f,
                Discount = 20000f,
                FinalAmount = 100000f,
                PaymentMethod = "Mobile Payment",
                Status = "Cancelled"
            }
        };
        }


    }

    public class MockIngredientRepository : IRepository<Ingredient>
    {
        private List<Ingredient> _ingredients = new List<Ingredient>
    {
        new Ingredient { IngredientID = 1, IngredientName = "Coffee Beans", CategoryID = 1, Stock = 10.0f, Unit = "kg", PurchasePrice = 20.0f, Supplier = "BachHoaXanh", ExpiryDate = DateTime.Now.AddMonths(6) },
        new Ingredient { IngredientID = 2, IngredientName = "Milk", CategoryID = 2, Stock = 5.0f, Unit = "L", PurchasePrice = 15.0f, Supplier = "Vinamilk", ExpiryDate = DateTime.Now.AddMonths(2) },
        new Ingredient { IngredientID = 3, IngredientName = "Sugar", CategoryID = 1, Stock = 8.0f, Unit = "kg", PurchasePrice = 5.0f, Supplier = "BachHoaXanh", ExpiryDate = DateTime.Now.AddMonths(12) },
        new Ingredient { IngredientID = 4, IngredientName = "Butter", CategoryID = 2, Stock = 3.0f, Unit = "kg", PurchasePrice = 25.0f, Supplier = "Vinamilk", ExpiryDate = DateTime.Now.AddMonths(1) },
        new Ingredient { IngredientID = 5, IngredientName = "Flour", CategoryID = 3, Stock = 12.0f, Unit = "kg", PurchasePrice = 10.0f, Supplier = "BachHoaXanh", ExpiryDate = DateTime.Now.AddMonths(6) }
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

