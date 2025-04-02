using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Controls;

namespace POS_For_Small_Shop.Data.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrdersDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionDetails> PromotionDetails { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ShiftOrder> ShiftOrders { get; set; }
        public DbSet<CashFlow> CashFlows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=pos;Username=postgres;Password=123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(i => i.CategoryID);

            modelBuilder.Entity<MenuItem>()
                .HasOne<Category>()
            .WithMany()
                .HasForeignKey(m => m.CategoryID);

            modelBuilder.Entity<Order>()
                .HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerID)
                .IsRequired(false);

            modelBuilder.Entity<Order>()
                .HasOne<Shift>()
                .WithMany()
                .HasForeignKey(o => o.ShiftID);

            modelBuilder.Entity<OrderDetail>()
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey(od => od.OrderID);

            modelBuilder.Entity<OrderDetail>()
                .HasOne<MenuItem>()
                .WithMany()
                .HasForeignKey(od => od.MenuItemID);

            modelBuilder.Entity<Promotion>()
                .HasOne(p => p.Details)
                .WithOne(pd => pd.Promotion)
                .HasForeignKey<PromotionDetails>(pd => pd.PromoID);

            modelBuilder.Entity<Transaction>()
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey(t => t.OrderID);

            modelBuilder.Entity<ShiftOrder>()
                .HasOne<Shift>()
                .WithMany()
                .HasForeignKey(so => so.ShiftID);

            modelBuilder.Entity<ShiftOrder>()
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey(so => so.OrderID);

            modelBuilder.Entity<CashFlow>()
                .HasOne<Shift>()
                .WithMany()
                .HasForeignKey(cf => cf.ShiftID);


            SeedData(modelBuilder);
        }


        private void SeedData(ModelBuilder modelBuilder)
        {
            DateTime staticDateUtc = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // CashFlow
            modelBuilder.Entity<CashFlow>().HasData(
                new CashFlow { CashFlowID = 1, ShiftID = 1, TransactionType = "Cash", Amount = 1000f, Timestamp = staticDateUtc },
                new CashFlow { CashFlowID = 2, ShiftID = 1, TransactionType = "Card", Amount = 1500f, Timestamp = staticDateUtc },
                new CashFlow { CashFlowID = 3, ShiftID = 2, TransactionType = "Cash", Amount = 2000f, Timestamp = staticDateUtc },
                new CashFlow { CashFlowID = 4, ShiftID = 2, TransactionType = "Card", Amount = 2500f, Timestamp = staticDateUtc },
                new CashFlow { CashFlowID = 5, ShiftID = 3, TransactionType = "Cash", Amount = 3000f, Timestamp = staticDateUtc }
            );

            // Category
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, Name = "Beverages", Description = "Drinks" },
                new Category { CategoryID = 2, Name = "Appetizers", Description = "Starters" },
                new Category { CategoryID = 3, Name = "Main Courses", Description = "Main Dishes" },
                new Category { CategoryID = 4, Name = "Desserts", Description = "Sweets" },
                new Category { CategoryID = 5, Name = "Sides", Description = "Side Dishes" }
            );

            // Customer
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerID = 1, Name = "John Doe", Phone = "1234567890", Email = "haha@gmail.com", Address = "123 Main", LoyaltyPoints = 12 },
                new Customer { CustomerID = 2, Name = "Jane Smith", Phone = "0987654321", Email = "jane@gmail.com", Address = "456 Elm", LoyaltyPoints = 20 },
                new Customer { CustomerID = 3, Name = "Alice Johnson", Phone = "1122334455", Email = "alice@gmail.com", Address = "789 Oak", LoyaltyPoints = 15 },
                new Customer { CustomerID = 4, Name = "Bob Brown", Phone = "5544332211", Email = "bob@gmail.com", Address = "321 Pine", LoyaltyPoints = 10 },
                new Customer { CustomerID = 5, Name = "Charlie Davis", Phone = "9988776655", Email = "charlie@gmail.com", Address = "654 Maple", LoyaltyPoints = 25 }
            );

            // Ingredient
            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { IngredientID = 1, IngredientName = "Sugar", CategoryID = 1, Stock = 100, Unit = "kg", PurchasePrice = 1000, Supplier = "BachHoaXanh", ExpiryDate = staticDateUtc },
                new Ingredient { IngredientID = 2, IngredientName = "Salt", CategoryID = 1, Stock = 200, Unit = "kg", PurchasePrice = 500, Supplier = "BachHoaXanh", ExpiryDate = staticDateUtc },
                new Ingredient { IngredientID = 3, IngredientName = "Flour", CategoryID = 2, Stock = 150, Unit = "kg", PurchasePrice = 1200, Supplier = "BachHoaXanh", ExpiryDate = staticDateUtc },
                new Ingredient { IngredientID = 4, IngredientName = "Oil", CategoryID = 2, Stock = 100, Unit = "liter", PurchasePrice = 2000, Supplier = "BachHoaXanh", ExpiryDate = staticDateUtc },
                new Ingredient { IngredientID = 5, IngredientName = "Eggs", CategoryID = 3, Stock = 50, Unit = "dozen", PurchasePrice = 1500, Supplier = "BachHoaXanh", ExpiryDate = staticDateUtc }
            );

            // MenuItem
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { MenuItemID = 1, Name = "Coca Cola", CategoryID = 1, SellingPrice = 1000, ImagePath = "Assets/MenuItems/coca-cola.jpg" },
                new MenuItem { MenuItemID = 2, Name = "Pizza", CategoryID = 3, SellingPrice = 5000, ImagePath = "Assets/MenuItems/pizza.jpg" },
                new MenuItem { MenuItemID = 3, Name = "Burger", CategoryID = 3, SellingPrice = 4000, ImagePath = "Assets/MenuItems/burger.jpg" },
                new MenuItem { MenuItemID = 4, Name = "Salad", CategoryID = 4, SellingPrice = 2000, ImagePath = "Assets/MenuItems/salad.jpg" },
                new MenuItem { MenuItemID = 5, Name = "Ice Cream", CategoryID = 4, SellingPrice = 1500, ImagePath = "Assets/MenuItems/ice-cream.jpg" }
            );

            // Notification
            modelBuilder.Entity<Notification>().HasData(
                new Notification { NotificationID = 1, Message = "Hello", CreatedAt = staticDateUtc, IsRead = false },
                new Notification { NotificationID = 2, Message = "New Order Received", CreatedAt = staticDateUtc, IsRead = false },
                new Notification { NotificationID = 3, Message = "Low Stock Alert", CreatedAt = staticDateUtc, IsRead = false },
                new Notification { NotificationID = 4, Message = "Shift Ended", CreatedAt = staticDateUtc, IsRead = false },
                new Notification { NotificationID = 5, Message = "Promotion Ending Soon", CreatedAt = staticDateUtc, IsRead = false }
            );

            // Order
            modelBuilder.Entity<Order>().HasData(
                new Order { OrderID = 1, CustomerID = 1, ShiftID = 1, TotalAmount = 1000, Discount = 0, FinalAmount = 1000, PaymentMethod = "Cash", Status = "Pending" },
                new Order { OrderID = 2, CustomerID = 2, ShiftID = 1, TotalAmount = 1500, Discount = 100, FinalAmount = 1400, PaymentMethod = "Card", Status = "Completed" },
                new Order { OrderID = 3, CustomerID = 3, ShiftID = 2, TotalAmount = 2000, Discount = 200, FinalAmount = 1800, PaymentMethod = "Cash", Status = "Pending" },
                new Order { OrderID = 4, CustomerID = 4, ShiftID = 2, TotalAmount = 2500, Discount = 250, FinalAmount = 2250, PaymentMethod = "Card", Status = "Completed" },
                new Order { OrderID = 5, CustomerID = 5, ShiftID = 3, TotalAmount = 3000, Discount = 300, FinalAmount = 2700, PaymentMethod = "Cash", Status = "Pending" }
            );

            // OrderDetail
            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail { OrderDetailID = 1, OrderID = 1, MenuItemID = 1, Quantity = 1, UnitPrice = 1000, Subtotal = 1000 },
                new OrderDetail { OrderDetailID = 2, OrderID = 2, MenuItemID = 2, Quantity = 1, UnitPrice = 5000, Subtotal = 5000 },
                new OrderDetail { OrderDetailID = 3, OrderID = 3, MenuItemID = 3, Quantity = 1, UnitPrice = 4000, Subtotal = 4000 },
                new OrderDetail { OrderDetailID = 4, OrderID = 4, MenuItemID = 4, Quantity = 1, UnitPrice = 2000, Subtotal = 2000 },
                new OrderDetail { OrderDetailID = 5, OrderID = 5, MenuItemID = 5, Quantity = 1, UnitPrice = 1500, Subtotal = 1500 }
            );

            // Promotion
            modelBuilder.Entity<Promotion>().HasData(
                new Promotion { PromoID = 1, PromoName = "Summer Sale", StartDate = staticDateUtc, EndDate = staticDateUtc.AddDays(7) },
                new Promotion { PromoID = 2, PromoName = "Weekend Special", StartDate = staticDateUtc, EndDate = staticDateUtc.AddDays(2) },
                new Promotion { PromoID = 3, PromoName = "Holiday Offer", StartDate = staticDateUtc, EndDate = staticDateUtc.AddDays(14) },
                new Promotion { PromoID = 4, PromoName = "Loyalty Bonus", StartDate = staticDateUtc, EndDate = staticDateUtc.AddDays(30) },
                new Promotion { PromoID = 5, PromoName = "New Year Deal", StartDate = staticDateUtc, EndDate = staticDateUtc.AddDays(45) }
            );

            modelBuilder.Entity<PromotionDetails>().HasData(
                new PromotionDetails { PromoDetailsID = 1, PromoID = 1, DiscountType = DiscountType.Percentage, DiscountValue = 10 },
                new PromotionDetails { PromoDetailsID = 2, PromoID = 2, DiscountType = DiscountType.FixedAmount, DiscountValue = 500 },
                new PromotionDetails { PromoDetailsID = 3, PromoID = 3, DiscountType = DiscountType.Percentage, DiscountValue = 15 },
                new PromotionDetails { PromoDetailsID = 4, PromoID = 4, DiscountType = DiscountType.FixedAmount, DiscountValue = 1000 },
                new PromotionDetails { PromoDetailsID = 5, PromoID = 5, DiscountType = DiscountType.Percentage, DiscountValue = 20 }
            );

            // Ensure the relationship is configured
            modelBuilder.Entity<Promotion>()
                .HasOne(p => p.Details)
                .WithOne(pd => pd.Promotion)
                .HasForeignKey<PromotionDetails>(pd => pd.PromoID);

            // Shift
            modelBuilder.Entity<Shift>().HasData(
                new Shift { ShiftID = 1, StartTime = staticDateUtc, EndTime = staticDateUtc.AddHours(8), OpeningCash = 5000, ClosingCash = 10000, TotalSales = 5000, TotalOrders = 10, Status = "Closed" },
                new Shift { ShiftID = 2, StartTime = staticDateUtc.AddDays(1), EndTime = staticDateUtc.AddDays(1).AddHours(8), OpeningCash = 5000, ClosingCash = 12000, TotalSales = 7000, TotalOrders = 15, Status = "Closed" },
                new Shift { ShiftID = 3, StartTime = staticDateUtc.AddDays(2), EndTime = staticDateUtc.AddDays(2).AddHours(8), OpeningCash = 5000, ClosingCash = 11000, TotalSales = 6000, TotalOrders = 12, Status = "Closed" },
                new Shift { ShiftID = 4, StartTime = staticDateUtc.AddDays(3), EndTime = staticDateUtc.AddDays(3).AddHours(8), OpeningCash = 5000, ClosingCash = 13000, TotalSales = 8000, TotalOrders = 20, Status = "Closed" },
                new Shift { ShiftID = 5, StartTime = staticDateUtc.AddDays(4), EndTime = staticDateUtc.AddDays(4).AddHours(8), OpeningCash = 5000, ClosingCash = 14000, TotalSales = 9000, TotalOrders = 25, Status = "Closed" }
            );

            // ShiftOrder
            modelBuilder.Entity<ShiftOrder>().HasData(
                new ShiftOrder { ShiftOrderID = 1, ShiftID = 1, OrderID = 1 },
                new ShiftOrder { ShiftOrderID = 2, ShiftID = 1, OrderID = 2 },
                new ShiftOrder { ShiftOrderID = 3, ShiftID = 2, OrderID = 3 },
                new ShiftOrder { ShiftOrderID = 4, ShiftID = 2, OrderID = 4 },
                new ShiftOrder { ShiftOrderID = 5, ShiftID = 3, OrderID = 5 }
            );

            // Transaction
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction { TransactionID = 1, OrderID = 1, AmountPaid = 1000, PaymentMethod = "Cash" },
                new Transaction { TransactionID = 2, OrderID = 2, AmountPaid = 1400, PaymentMethod = "Card" },
                new Transaction { TransactionID = 3, OrderID = 3, AmountPaid = 1800, PaymentMethod = "Cash" },
                new Transaction { TransactionID = 4, OrderID = 4, AmountPaid = 2250, PaymentMethod = "Card" },
                new Transaction { TransactionID = 5, OrderID = 5, AmountPaid = 2700, PaymentMethod = "Cash" }
            );
        }


    }
}