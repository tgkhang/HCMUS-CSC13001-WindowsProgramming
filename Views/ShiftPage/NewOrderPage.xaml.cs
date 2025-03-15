using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;

namespace POS_For_Small_Shop.Views
{
    public sealed partial class NewOrderPage : Page
    {
        private IDao _dao;
        private List<MenuItem> _allMenuItems;
        private int _selectedCategoryId = 0; // 0 means all categories
        private string _searchText = "";

        public ObservableCollection<MenuItem> FilteredMenuItems { get; private set; } = new ObservableCollection<MenuItem>();
        public ObservableCollection<OrderItemViewModel> OrderItems { get; private set; } = new ObservableCollection<OrderItemViewModel>();

        // Order summary properties
        public float Subtotal { get; private set; }
        public float Tax { get; private set; }
        public float Discount { get; private set; }
        public float Total { get; private set; }
        private float _discountPercentage = 0;

        public NewOrderPage()
        {
            this.InitializeComponent();

            // Get the DAO from the service
            _dao = Service.GetKeyedSingleton<IDao>();

            // Load menu items from repository
            LoadMenuItems();

            // Initialize discount combo box
            DiscountComboBox.SelectedIndex = 0;

            // Set a default order number (in a real app, this would come from the database)
            OrderNumberText.Text = DateTime.Now.ToString("yyMMddHHmm");
        }

        private void LoadMenuItems()
        {
            // Get all menu items from the repository
            _allMenuItems = _dao.MenuItems.GetAll();

            // Apply filters and update the observable collection
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            // Start with all menu items
            var filteredItems = _allMenuItems;

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

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                _searchText = sender.Text;
                ApplyFilters();
            }
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string categoryIdStr)
            {
                // Parse the category ID from the button's tag
                if (int.TryParse(categoryIdStr, out int categoryId))
                {
                    _selectedCategoryId = categoryId;
                    ApplyFilters();

                    // Update button styles
                    foreach (var child in ((StackPanel)((ScrollViewer)((Button)sender).Parent).Content).Children)
                    {
                        if (child is Button categoryButton)
                        {
                            categoryButton.Style = categoryButton == button
                                ? (Style)Resources["AccentButtonStyle"]
                                : (Style)Resources["DefaultButtonStyle"];
                        }
                    }
                }
            }
        }

        private void ProductsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is MenuItem menuItem)
            {
                AddItemToOrder(menuItem);
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
                existingItem.Total = existingItem.Quantity * existingItem.UnitPrice;
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

            // Notify property changed
            Bindings.Update();
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int menuItemId)
            {
                var orderItem = OrderItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
                if (orderItem != null)
                {
                    orderItem.Quantity++;
                    orderItem.Total = orderItem.Quantity * orderItem.UnitPrice;
                    UpdateOrderSummary();
                }
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int menuItemId)
            {
                var orderItem = OrderItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
                if (orderItem != null)
                {
                    if (orderItem.Quantity > 1)
                    {
                        orderItem.Quantity--;
                        orderItem.Total = orderItem.Quantity * orderItem.UnitPrice;
                    }
                    else
                    {
                        OrderItems.Remove(orderItem);
                    }

                    UpdateOrderSummary();
                }
            }
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int menuItemId)
            {
                var orderItem = OrderItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
                if (orderItem != null)
                {
                    OrderItems.Remove(orderItem);
                    UpdateOrderSummary();
                }
            }
        }

        private void DiscountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DiscountComboBox.SelectedItem is ComboBoxItem selectedItem &&
                selectedItem.Tag is string discountStr)
            {
                if (float.TryParse(discountStr, out float discount))
                {
                    _discountPercentage = discount;
                    UpdateOrderSummary();
                }
            }
        }

        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            // Clear the current order
            OrderItems.Clear();

            // Reset discount
            DiscountComboBox.SelectedIndex = 0;

            // Generate a new order number
            OrderNumberText.Text = DateTime.Now.ToString("yyMMddHHmm");

            // Update order summary
            UpdateOrderSummary();
        }

        private async void CashPayment_Click(object sender, RoutedEventArgs e)
        {
            if (OrderItems.Count == 0)
            {
                await ShowMessageDialog("Error", "Please add items to the order before proceeding to payment.");
                return;
            }

            // In a real app, you would save the order to the database here
            // and navigate to a payment confirmation page

            await ShowMessageDialog("Payment", "Cash payment processed successfully!");

            // Create a new order after successful payment
            NewOrder_Click(sender, e);
        }

        private async void CardPayment_Click(object sender, RoutedEventArgs e)
        {
            if (OrderItems.Count == 0)
            {
                await ShowMessageDialog("Error", "Please add items to the order before proceeding to payment.");
                return;
            }

            // In a real app, you would integrate with a payment processor here

            await ShowMessageDialog("Payment", "Card payment processed successfully!");

            // Create a new order after successful payment
            NewOrder_Click(sender, e);
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            // Simply clear the current order
            NewOrder_Click(sender, e);
        }

        private async Task ShowMessageDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }

    // ViewModel for order items
    public class OrderItemViewModel : INotifyPropertyChanged
    {
        public int MenuItemID { get; set; }
        public string Name { get; set; }
        public float UnitPrice { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
                }
            }
        }

        private float _total;
        public float Total
        {
            get => _total;
            set
            {
                if (_total != value)
                {
                    _total = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Total)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

