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
        //public IRepository<Promotion> Promotions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Shift> Shifts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Ingredient> Ingredients { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Order> Orders { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<OrderDetail> OrderDetails { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Transaction> Transactions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<CashFlow> CashFlows { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<Notification> Notifications { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRepository<ShiftOrder> ShiftOrders { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }



        //Done
        public IRepository<MenuItem> MenuItems { get; set; } = new MockMenuItemRepository();
        public IRepository<Promotion> Promotions { get; set; } = new MockPromotionRepository();

    }


    public class MockMenuItemRepository : IRepository<MenuItem>
    {
        private List<MenuItem> _menuItems = new List<MenuItem>
        {
            new MenuItem { MenuItemID = 1, Name = "Espresso", CategoryID = 1, SellingPrice = 2.50f, ImagePath = "Assets/Images/espresso.jpg" },
            new MenuItem { MenuItemID = 2, Name = "Cappuccino", CategoryID = 1, SellingPrice = 3.00f, ImagePath = "Assets/Images/cappuccino.jpg" },
            new MenuItem { MenuItemID = 3, Name = "Latte", CategoryID = 1, SellingPrice = 3.50f, ImagePath = "Assets/Images/latte.jpg" },
            new MenuItem { MenuItemID = 4, Name = "Croissant", CategoryID = 2, SellingPrice = 2.00f, ImagePath = "Assets/Images/croissant.jpg" },
            new MenuItem { MenuItemID = 5, Name = "Muffin", CategoryID = 2, SellingPrice = 2.20f, ImagePath = "Assets/Images/muffin.jpg" },
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

    public class MockPromotionRepository : IRepository<Promotion>
    {
        private List<Promotion> _promotions = new List<Promotion>
    {
        new Promotion
        {
            PromoID = 1,
            PromoName = "Summer Sale",
            StartDate = DateTime.UtcNow.AddDays(2),
            EndDate = DateTime.UtcNow.AddDays(7),
            ItemIDs = new List<int> { 1, 2 }, 
            Details = new PromotionDetails
            {
                PromoDetailsID = 1,
                PromoID = 1,
                DiscountType = DiscountType.Percentage,
                DiscountValue = 10,
                Description = "10% off summer drinks"
            }
        },
        new Promotion
        {
            PromoID = 2,
            PromoName = "Weekend Special",
            StartDate = DateTime.UtcNow.AddDays(-5),
            EndDate = DateTime.UtcNow.AddDays(-1),
            ItemIDs = new List<int> { 3 }, 
            Details = new PromotionDetails
            {
                PromoDetailsID = 2,
                PromoID = 2,
                DiscountType = DiscountType.FixedAmount,
                DiscountValue = 500,
                Description = "$5 off espressos"
            }
        },
        new Promotion
        {
            PromoID = 3,
            PromoName = "Holiday Offer",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(14),
            ItemIDs = new List<int> { 1, 3 }, 
            Details = new PromotionDetails
            {
                PromoDetailsID = 3,
                PromoID = 3,
                DiscountType = DiscountType.Percentage,
                DiscountValue = 15,
                Description = "15% off holiday specials"
            }
        }
    };

        public List<Promotion> GetAll()
        {
            return _promotions.ToList();
        }

        public Promotion GetById(int id)
        {
            return _promotions.FirstOrDefault(p => p.PromoID == id); 
        }

        public bool Insert(Promotion item)
        {
            if (item == null || _promotions.Any(p => p.PromoID == item.PromoID))
                return false; 

            if (item.Details != null)
                item.Details.PromoID = item.PromoID;

            _promotions.Add(item);
            return true;
        }

        public bool Update(int id, Promotion item)
        {
            if (item == null)
                return false;

            var existing = _promotions.FirstOrDefault(p => p.PromoID == id);
            if (existing == null)
                return false; 

            // Update fields
            existing.PromoName = item.PromoName;
            existing.StartDate = item.StartDate;
            existing.EndDate = item.EndDate;
            existing.ItemIDs = item.ItemIDs ?? new List<int>(); 
            existing.Details = item.Details ?? existing.Details; 

            // Ensure PromoID consistency in Details
            if (existing.Details != null)
                existing.Details.PromoID = id;

            return true;
        }

        public bool Delete(int id)
        {
            var promotion = _promotions.FirstOrDefault(p => p.PromoID == id);
            if (promotion == null)
                return false; 

            _promotions.Remove(promotion);
            return true;
        }
    }
}
