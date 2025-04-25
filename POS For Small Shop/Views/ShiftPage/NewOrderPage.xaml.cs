using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using POS_For_Small_Shop.ViewModels.ShiftPage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.ShiftPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewOrderPage : Page
    {
        public NewOrderViewModel ViewModel { get; } = new NewOrderViewModel();
        private IDao _dao;
        private List<Customer> _allCustomers;

        public NewOrderPage()
        {
            this.InitializeComponent();
            DataContext = ViewModel;

            // Initialize discount combo box
            DiscountComboBox.SelectedIndex = 0;

            // Get the DAO from the service
            _dao = Service.GetKeyedSingleton<IDao>();

            // Load customers
            LoadCustomers();

            // Setup ComboBox for discounts
            DiscountComboBox.ItemsSource = ViewModel.AvailablePromotions;
            DiscountComboBox.DisplayMemberPath = "PromoName";
            DiscountComboBox.SelectionChanged += (s, e) =>
            {
                ViewModel.SetDiscount((Promotion)DiscountComboBox.SelectedItem);
            };

            ViewModel.PaymentRequested += ViewModel_PaymentRequested;
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string parameter)
            {
                // Use the parameter
            }
        }

        private void ViewModel_PaymentRequested(object sender, NewOrderViewModel.PaymentType paymentType)
        {
            Frame.Navigate(typeof(PaymentPage), ViewModel);
        }
        private void LoadCustomers()
        {
            try
            {
                // Get all customers from the repository
                _allCustomers = _dao.Customers.GetAll();
            }
            catch (NotImplementedException)
            {
                // If the repository is not implemented yet, use mock data
                _allCustomers = new List<Customer>
                {
                    new Customer { CustomerID = 1, Name = "John Doe", Phone = "0123456789", Email = "john@example.com", Address = "123 Main St", LoyaltyPoints = 150 },
                    new Customer { CustomerID = 2, Name = "Jane Smith", Phone = "0987654321", Email = "jane@example.com", Address = "456 Oak Ave", LoyaltyPoints = 75 },
                    new Customer { CustomerID = 3, Name = "Bob Johnson", Phone = "0369852147", Email = "bob@example.com", Address = "789 Pine Rd", LoyaltyPoints = 200 }
                };
            }
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.SetSearchText(sender.Text);
            }
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string categoryIdStr)
            {
                // Parse the category ID from the button's tag
                if (int.TryParse(categoryIdStr, out int categoryId))
                {
                    ViewModel.SetCategory(categoryId);

                    // Get the parent StackPanel directly
                    // Find the parent StackPanel that contains all category buttons
                    if (button.Parent is StackPanel categoryStackPanel)
                    {
                        // Update button styles
                        foreach (var child in categoryStackPanel.Children)
                        {
                            if (child is Button categoryButton)
                            {
                                // Use direct style setting
                                if (categoryButton == button)
                                {
                                    categoryButton.Style = Application.Current.Resources["AccentButtonStyle"] as Style;
                                }
                                else
                                {
                                    categoryButton.Style = null; // This will use the default style
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ProductsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is MenuItem menuItem)
            {
                ViewModel.AddToOrderCommand.Execute(menuItem);
            }
        }

        //private void DiscountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (DiscountComboBox.SelectedItem is ComboBoxItem selectedItem &&
        //        selectedItem.Tag is string discountStr)
        //    {
        //        if (float.TryParse(discountStr, out float discount))
        //        {
        //            ViewModel.SetDiscount(null);
        //        }
        //    }
        //}

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int menuItemId)
            {
                ViewModel.IncreaseQuantityCommand.Execute(menuItemId);
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int menuItemId)
            {
                ViewModel.DecreaseQuantityCommand.Execute(menuItemId);
            }
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int menuItemId)
            {
                ViewModel.RemoveFromOrderCommand.Execute(menuItemId);
            }
        }

        // Simplified customer selection dialog
        private async void SelectCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_allCustomers == null || _allCustomers.Count == 0)
                {
                    await ShowMessage("No Customers", "There are no customers in the system. Please add customers first.");
                    return;
                }

                // Create a simple dialog with a list of customers
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Select Customer",
                    PrimaryButtonText = "Select",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.XamlRoot
                };

                // Create a grid to hold the customer selection UI
                Grid mainGrid = new Grid();
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                // Add a search box
                AutoSuggestBox searchBox = new AutoSuggestBox
                {
                    PlaceholderText = "Search customers by name, phone, or email...",
                    QueryIcon = new SymbolIcon(Symbol.Find),
                    Margin = new Thickness(0, 0, 0, 10)
                };
                Grid.SetRow(searchBox, 0);
                mainGrid.Children.Add(searchBox);

                // Create a ListView for customers
                ListView customerListView = new ListView
                {
                    SelectionMode = ListViewSelectionMode.Single,
                    MaxHeight = 400,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                Grid.SetRow(customerListView, 1);
                mainGrid.Children.Add(customerListView);

                // Create a filtered list of customers
                ObservableCollection<Customer> filteredCustomers = new ObservableCollection<Customer>(_allCustomers);
                customerListView.ItemsSource = filteredCustomers;

                // Set up the search functionality
                searchBox.TextChanged += (s, args) =>
                {
                    if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                    {
                        string searchText = searchBox.Text.ToLower();
                        filteredCustomers.Clear();

                        foreach (var customer in _allCustomers.Where(c =>
                            c.Name.ToLower().Contains(searchText) ||
                            (c.Phone != null && c.Phone.ToLower().Contains(searchText) ||
                            (c.Email != null && c.Email.ToLower().Contains(searchText)))))
                        {
                            filteredCustomers.Add(customer);
                        }
                    }
                };

                // Set the item template for the ListView
                customerListView.ItemTemplate = (DataTemplate)Resources["CustomerItemTemplate"];

                dialog.Content = mainGrid;

                // Enable/disable the primary button based on selection
                customerListView.SelectionChanged += (s, args) =>
                {
                    dialog.IsPrimaryButtonEnabled = customerListView.SelectedItem != null;
                };
                dialog.IsPrimaryButtonEnabled = false;

                // Show the dialog and handle the result
                var result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary && customerListView.SelectedItem is Customer selectedCustomer)
                {
                    ViewModel.SelectCustomerCommand.Execute(selectedCustomer);
                }
            }
            catch (Exception ex)
            {
                await ShowMessage("Error", $"An error occurred while selecting a customer: {ex.Message}");
            }
        }

        private void ClearCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClearSelectedCustomer();
        }

        private async Task ShowMessage(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
