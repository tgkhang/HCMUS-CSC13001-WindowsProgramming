using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using PropertyChanged;

namespace POS_For_Small_Shop.ViewModels.ShiftPage
{
    [AddINotifyPropertyChangedInterface]
    public class NewOrderViewModel : INotifyPropertyChanged
    {
        private IDao _dao;
        private string _searchText = "";
        private int _selectedCategoryId = 0;
        private float _discountPercentage = 0;
        private string _orderNumber;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<MenuItem> AllMenuItems { get; private set; }
        public ObservableCollection<MenuItem> FilteredMenuItems { get; private set; }
        public ObservableCollection<OrderItemViewModel> OrderItems { get; private set; }

        // Order summary properties
        public float Subtotal { get; private set; }
        public float Tax { get; private set; }
        public float Discount { get; private set; }
        public float Total { get; private set; }

        public string OrderNumber
        {
            get => _orderNumber;
            private set => _orderNumber = value;
        }

        // Add a new property for the selected customer
        public Customer SelectedCustomer { get; private set; }

        // Commands
        public ICommand AddToOrderCommand { get; }
        public ICommand RemoveFromOrderCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand NewOrderCommand { get; }
        public ICommand CashPaymentCommand { get; }
        public ICommand CardPaymentCommand { get; }
        public ICommand CancelOrderCommand { get; }

        // Add a new command for selecting a customer
        public ICommand SelectCustomerCommand { get; }

        public NewOrderViewModel()
        {
            _dao = Service.GetKeyedSingleton<IDao>();

            // Initialize collections
            try
            {
                AllMenuItems = new ObservableCollection<MenuItem>(_dao.MenuItems.GetAll());
            }
            catch (NotImplementedException)
            {
                // If repository is not implemented, use empty collection
                AllMenuItems = new ObservableCollection<MenuItem>();
            }

            FilteredMenuItems = new ObservableCollection<MenuItem>();
            OrderItems = new ObservableCollection<OrderItemViewModel>();

            // Initialize commands
            AddToOrderCommand = new RelayCommand<MenuItem>(AddItemToOrder);
            RemoveFromOrderCommand = new RelayCommand<int>(RemoveItemFromOrder);
            IncreaseQuantityCommand = new RelayCommand<int>(IncreaseQuantity);
            DecreaseQuantityCommand = new RelayCommand<int>(DecreaseQuantity);
            NewOrderCommand = new RelayCommand<object>(_ => CreateNewOrder());
            CashPaymentCommand = new RelayCommand<object>(_ => ProcessCashPayment());
            CardPaymentCommand = new RelayCommand<object>(_ => ProcessCardPayment());
            CancelOrderCommand = new RelayCommand<object>(_ => CancelOrder());

            // Add this to the constructor after initializing other commands
            SelectCustomerCommand = new RelayCommand<Customer>(SelectCustomer);

            // Generate initial order number
            GenerateOrderNumber();

            // Apply initial filters
            ApplyFilters();
        }

        public void SetSearchText(string text)
        {
            _searchText = text;
            ApplyFilters();
        }

        public void SetCategory(int categoryId)
        {
            _selectedCategoryId = categoryId;
            ApplyFilters();
        }

        public void SetDiscountPercentage(float percentage)
        {
            _discountPercentage = percentage;
            UpdateOrderSummary();
        }

        private void ApplyFilters()
        {
            // Start with all menu items
            var filteredItems = AllMenuItems.ToList();

            // Apply category filter if a specific category is selected
            if (_selectedCategoryId > 0)
            {
                filteredItems = filteredItems.Where(item => item.CategoryID == _selectedCategoryId).ToList();
            }

            // Apply search filter if there's search text
            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                filteredItems = filteredItems.Where(item =>
                    item.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Update the observable collection
            FilteredMenuItems.Clear();
            foreach (var item in filteredItems)
            {
                FilteredMenuItems.Add(item);
            }
        }

        private void AddItemToOrder(MenuItem menuItem)
        {
            // Check if the item is already in the order
            var existingItem = OrderItems.FirstOrDefault(item => item.MenuItemID == menuItem.MenuItemID);

            if (existingItem != null)
            {
                // Increase quantity if already in order
                existingItem.Quantity++;
                // The Total is now updated in the OrderItemViewModel's Quantity setter
            }
            else
            {
                // Add new item to order
                var orderItem = new OrderItemViewModel
                {
                    MenuItemID = menuItem.MenuItemID,
                    Name = menuItem.Name,
                    UnitPrice = menuItem.SellingPrice,
                    Quantity = 1,
                    Total = menuItem.SellingPrice
                };

                OrderItems.Add(orderItem);
            }

            // Update order summary
            UpdateOrderSummary();
        }

        private void RemoveItemFromOrder(int menuItemId)
        {
            var orderItem = OrderItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
            if (orderItem != null)
            {
                OrderItems.Remove(orderItem);
                UpdateOrderSummary();
            }
        }

        private void IncreaseQuantity(int menuItemId)
        {
            var orderItem = OrderItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
            if (orderItem != null)
            {
                orderItem.Quantity++;
                // The Total is now updated in the OrderItemViewModel's Quantity setter
                UpdateOrderSummary();
            }
        }

        private void DecreaseQuantity(int menuItemId)
        {
            var orderItem = OrderItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
            if (orderItem != null)
            {
                if (orderItem.Quantity > 1)
                {
                    orderItem.Quantity--;
                    // The Total is now updated in the OrderItemViewModel's Quantity setter
                }
                else
                {
                    OrderItems.Remove(orderItem);
                }

                UpdateOrderSummary();
            }
        }

        private void UpdateOrderSummary()
        {
            // Calculate subtotal
            Subtotal = OrderItems.Sum(item => item.Total);

            // Calculate tax (8%)
            Tax = Subtotal * 0.08f;

            // Calculate discount
            Discount = Subtotal * (_discountPercentage / 100f);

            // Calculate total
            Total = Subtotal + Tax - Discount;

            // Notify property changed for all summary properties
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Discount));
            OnPropertyChanged(nameof(Total));
        }

        private void GenerateOrderNumber()
        {
            // Generate a new order number based on current date/time
            OrderNumber = DateTime.Now.ToString("yyMMddHHmm");
            OnPropertyChanged(nameof(OrderNumber));
        }

        private void CreateNewOrder()
        {
            // Clear the current order
            OrderItems.Clear();

            // Reset discount
            _discountPercentage = 0;

            // Reset selected customer
            SelectedCustomer = null;
            OnPropertyChanged(nameof(SelectedCustomer));

            // Generate a new order number
            GenerateOrderNumber();

            // Update order summary
            UpdateOrderSummary();
        }

        private void ProcessCashPayment()
        {
            if (OrderItems.Count > 0)
            {
                try
                {
                    // Get the current active shift
                    var activeShift = _dao.Shifts.GetAll().FirstOrDefault(s => s.Status == "Open");
                    if (activeShift == null)
                    {
                        // Create a new shift if no active shift exists
                        activeShift = new Shift
                        {
                            StartTime = DateTime.Now,
                            OpeningCash = 0,
                            TotalSales = 0,
                            TotalOrders = 0,
                            Status = "Open"
                        };
                        _dao.Shifts.Insert(activeShift);
                    }

                    // Create a new Order object
                    var order = new Order
                    {
                        CustomerID = SelectedCustomer?.CustomerID,
                        ShiftID = activeShift.ShiftID,
                        TotalAmount = Subtotal,
                        Discount = Discount,
                        FinalAmount = Total,
                        PaymentMethod = "Cash",
                        Status = "Completed"
                    };

                    // Create order items
                    var orderItems = OrderItems.Select(item => new OrderDetail
                    {
                        MenuItemID = item.MenuItemID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Subtotal = item.Total
                    }).ToList();

                    // Save order to database
                    bool success = _dao.Orders.Insert(order);
                    if (success)
                    {
                        // Save order details
                        foreach (var item in orderItems)
                        {
                            item.OrderID = order.OrderID;
                            _dao.OrderDetails.Insert(item);
                        }

                        // Update customer loyalty points if applicable
                        if (SelectedCustomer != null)
                        {
                            SelectedCustomer.LoyaltyPoints += (int)(Total / 10); // 1 point per $10 spent
                            _dao.Customers.Update(SelectedCustomer.CustomerID, SelectedCustomer);
                        }

                        // Create a transaction record
                        var transaction = new Transaction
                        {
                            OrderID = order.OrderID,
                            AmountPaid = Total,
                            PaymentMethod = order.PaymentMethod
                        };
                        _dao.Transactions.Insert(transaction);

                        // Link order to the shift
                        var shiftOrder = new ShiftOrder
                        {
                            ShiftID = activeShift.ShiftID,
                            OrderID = order.OrderID
                        };
                        _dao.ShiftOrders.Insert(shiftOrder);

                        // Update shift statistics
                        activeShift.TotalSales += Total;
                        activeShift.TotalOrders += 1;
                        _dao.Shifts.Update(activeShift.ShiftID, activeShift);
                    }
                }
                catch (Exception)
                {
                    // Log error or handle exception
                    // In a real app, you might want to show an error message
                }

                // Create a new order
                CreateNewOrder();
            }
        }

        private void ProcessCardPayment()
        {
            if (OrderItems.Count > 0)
            {
                try
                {
                    // Get the current active shift
                    var activeShift = _dao.Shifts.GetAll().FirstOrDefault(s => s.Status == "Open");
                    if (activeShift == null)
                    {
                        // Create a new shift if no active shift exists
                        activeShift = new Shift
                        {
                            StartTime = DateTime.Now,
                            OpeningCash = 0,
                            TotalSales = 0,
                            TotalOrders = 0,
                            Status = "Open"
                        };
                        _dao.Shifts.Insert(activeShift);
                    }

                    // Create a new Order object
                    var order = new Order
                    {
                        CustomerID = SelectedCustomer?.CustomerID,
                        ShiftID = activeShift.ShiftID,
                        TotalAmount = Subtotal,
                        Discount = Discount,
                        FinalAmount = Total,
                        PaymentMethod = "Card",
                        Status = "Completed"
                    };

                    // Create order items
                    var orderItems = OrderItems.Select(item => new OrderDetail
                    {
                        MenuItemID = item.MenuItemID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Subtotal = item.Total
                    }).ToList();

                    // Save order to database
                    bool success = _dao.Orders.Insert(order);
                    if (success)
                    {
                        // Save order details
                        foreach (var item in orderItems)
                        {
                            item.OrderID = order.OrderID;
                            _dao.OrderDetails.Insert(item);
                        }

                        // Update customer loyalty points if applicable
                        if (SelectedCustomer != null)
                        {
                            SelectedCustomer.LoyaltyPoints += (int)(Total / 10); // 1 point per $10 spent
                            _dao.Customers.Update(SelectedCustomer.CustomerID, SelectedCustomer);
                        }
                    }
                }
                catch (Exception)
                {
                    // Log error or handle exception
                    // In a real app, you might want to show an error message
                }

                // Create a new order
                CreateNewOrder();
            }
        }

        private void CancelOrder()
        {
            // Simply clear the current order
            CreateNewOrder();
        }

        // Add this method to handle customer selection
        private void SelectCustomer(Customer customer)
        {
            SelectedCustomer = customer;
            // You could apply customer-specific discounts or loyalty points here
            OnPropertyChanged(nameof(SelectedCustomer));
            UpdateOrderSummary();
        }

        // Add this method to clear the selected customer
        public void ClearSelectedCustomer()
        {
            SelectedCustomer = null;
            OnPropertyChanged(nameof(SelectedCustomer));
            UpdateOrderSummary();
        }
    }
}