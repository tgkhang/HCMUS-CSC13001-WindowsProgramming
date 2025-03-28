using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using PropertyChanged;

namespace POS_For_Small_Shop.ViewModels.ShiftPage
{
    [AddINotifyPropertyChangedInterface]
    public class NewOrderViewModel
    {
        private IDao _dao;
        private string _searchText = "";
        private int _selectedCategoryId = 0;
        private float _discountPercentage = 0;
        private string _orderNumber;

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
            AllMenuItems = new ObservableCollection<MenuItem>(_dao.MenuItems.GetAll());
            FilteredMenuItems = new ObservableCollection<MenuItem>();
            OrderItems = new ObservableCollection<OrderItemViewModel>();

            // Initialize commands
            AddToOrderCommand = new RelayCommand<MenuItem>(AddItemToOrder);
            RemoveFromOrderCommand = new RelayCommand<int>(RemoveItemFromOrder);
            IncreaseQuantityCommand = new RelayCommand<int>(IncreaseQuantity);
            DecreaseQuantityCommand = new RelayCommand<int>(DecreaseQuantity);
            //NewOrderCommand = new RelayCommand(CreateNewOrder);
            //CashPaymentCommand = new RelayCommand(ProcessCashPayment);
            //CardPaymentCommand = new RelayCommand(ProcessCardPayment);
            //CancelOrderCommand = new RelayCommand(CancelOrder);

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
        }

        private void GenerateOrderNumber()
        {
            // Generate a new order number based on current date/time
            OrderNumber = DateTime.Now.ToString("yyMMddHHmm");
        }

        private void CreateNewOrder()
        {
            // Clear the current order
            OrderItems.Clear();

            // Reset discount
            _discountPercentage = 0;

            // Generate a new order number
            GenerateOrderNumber();

            // Update order summary
            UpdateOrderSummary();
        }

        private void ProcessCashPayment()
        {
            if (OrderItems.Count > 0)
            {
                // In a real app, you would save the order to the database here
                // and handle the payment processing

                // For now, just create a new order
                CreateNewOrder();
            }
        }

        private void ProcessCardPayment()
        {
            if (OrderItems.Count > 0)
            {
                // In a real app, you would save the order to the database here
                // and handle the payment processing

                // For now, just create a new order
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
            UpdateOrderSummary();
        }

        // Add this method to clear the selected customer
        public void ClearSelectedCustomer()
        {
            SelectedCustomer = null;
            UpdateOrderSummary();
        }
    }
}

