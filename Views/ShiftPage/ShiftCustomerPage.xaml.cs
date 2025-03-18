using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using POS_For_Small_Shop.ViewModels.ShiftPage;

namespace POS_For_Small_Shop.Views.ShiftPage
{
    public sealed partial class ShiftCustomerPage : Page
    {
        private IDao _dao;
        private List<Customer> _allCustomers;
        private Customer _currentCustomer;
        private bool _isEditMode = false;
        private string _searchText = "";

        public ObservableCollection<Customer> FilteredCustomers { get; private set; } = new ObservableCollection<Customer>();

        public ShiftCustomerPage()
        {
            this.InitializeComponent();

            // Get the DAO from the service
            _dao = Service.GetKeyedSingleton<IDao>();

            // Load customers
            LoadCustomers();

            // Set the DataContext for binding
            CustomerListView.ItemsSource = FilteredCustomers;
        }

        private void LoadCustomers()
        {
            try
            {
                // Get all customers from the repository
                _allCustomers = _dao.Customers.GetAll();

                // Apply filters and update the observable collection
                ApplyFilters();

                // Update UI based on whether we have customers
                UpdateEmptyState();
            }
            catch (NotImplementedException)
            {
                // If the repository is not implemented yet, use mock data
                _allCustomers = new List<Customer>
                {
                //    new Customer { CustomerID = 1, Name = "John Doe", Phone = "0123456789", Email = "john@example.com", Address = "123 Main St", LoyaltyPoints = 150 },
                //    new Customer { CustomerID = 2, Name = "Jane Smith", Phone = "0987654321", Email = "jane@example.com", Address = "456 Oak Ave", LoyaltyPoints = 75 },
                //    new Customer { CustomerID = 3, Name = "Bob Johnson", Phone = "0369852147", Email = "bob@example.com", Address = "789 Pine Rd", LoyaltyPoints = 200 }
                //
                };

                ApplyFilters();
                UpdateEmptyState();
            }
        }

        private void ApplyFilters()
        {
            // Start with all customers
            var filteredItems = _allCustomers;

            // Apply search filter if there's search text
            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                filteredItems = filteredItems.Where(customer =>
                    customer.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase) ||
                    (customer.Phone != null && customer.Phone.Contains(_searchText, StringComparison.OrdinalIgnoreCase)) ||
                    (customer.Email != null && customer.Email.Contains(_searchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            // Update the observable collection
            FilteredCustomers.Clear();
            foreach (var customer in filteredItems)
            {
                FilteredCustomers.Add(customer);
            }
        }

        private void UpdateEmptyState()
        {
            if (FilteredCustomers.Count == 0)
            {
                EmptyStateText.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyStateText.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                _searchText = sender.Text;
                ApplyFilters();
                UpdateEmptyState();
            }
        }

        private void CustomerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This is only used to highlight the selected customer
            // Actual editing is done through the Edit button
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            // Set up the form for adding a new customer
            _isEditMode = false;
            _currentCustomer = new Customer();

            // Clear form fields
            NameTextBox.Text = "";
            PhoneTextBox.Text = "";
            EmailTextBox.Text = "";
            AddressTextBox.Text = "";
            LoyaltyPointsBox.Value = 0;

            // Update UI
            FormHeaderText.Text = "Add Customer";
            CustomerDetailsPanel.Visibility = Visibility.Visible;
        }

        private void EditCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int customerId)
            {
                // Find the customer
                _currentCustomer = _allCustomers.FirstOrDefault(c => c.CustomerID == customerId);

                if (_currentCustomer != null)
                {
                    // Set up the form for editing
                    _isEditMode = true;

                    // Populate form fields
                    NameTextBox.Text = _currentCustomer.Name;
                    PhoneTextBox.Text = _currentCustomer.Phone ?? "";
                    EmailTextBox.Text = _currentCustomer.Email ?? "";
                    AddressTextBox.Text = _currentCustomer.Address ?? "";
                    LoyaltyPointsBox.Value = _currentCustomer.LoyaltyPoints;

                    // Update UI
                    FormHeaderText.Text = "Edit Customer";
                    CustomerDetailsPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private async void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int customerId)
            {
                // Find the customer
                var customerToDelete = _allCustomers.FirstOrDefault(c => c.CustomerID == customerId);

                if (customerToDelete != null)
                {
                    // Show confirmation dialog
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Delete Customer",
                        Content = $"Are you sure you want to delete {customerToDelete.Name}? This action cannot be undone.",
                        PrimaryButtonText = "Delete",
                        CloseButtonText = "Cancel",
                        DefaultButton = ContentDialogButton.Close,
                        XamlRoot = this.XamlRoot
                    };

                    var result = await dialog.ShowAsync();

                    if (result == ContentDialogResult.Primary)
                    {
                        try
                        {
                            // Delete from repository
                            bool success = _dao.Customers.Delete(customerId);

                            if (success)
                            {
                                // Remove from local list
                                _allCustomers.Remove(customerToDelete);

                                // Update UI
                                ApplyFilters();
                                UpdateEmptyState();
                            }
                        }
                        catch (NotImplementedException)
                        {
                            // If the repository is not implemented yet, just remove from local list
                            _allCustomers.Remove(customerToDelete);

                            // Update UI
                            ApplyFilters();
                            UpdateEmptyState();
                        }
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate form
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                ShowError("Name is required.");
                return;
            }

            // Update customer object
            _currentCustomer.Name = NameTextBox.Text;
            _currentCustomer.Phone = PhoneTextBox.Text;
            _currentCustomer.Email = EmailTextBox.Text;
            _currentCustomer.Address = AddressTextBox.Text;
            _currentCustomer.LoyaltyPoints = (int)LoyaltyPointsBox.Value;

            try
            {
                bool success;

                if (_isEditMode)
                {
                    // Update existing customer
                    success = _dao.Customers.Update(_currentCustomer.CustomerID, _currentCustomer);
                }
                else
                {
                    // Add new customer
                    success = _dao.Customers.Insert(_currentCustomer);

                    if (success && _currentCustomer.CustomerID == 0)
                    {
                        // If the repository doesn't set the ID, set it manually for mock data
                        _currentCustomer.CustomerID = _allCustomers.Count > 0 ?
                            _allCustomers.Max(c => c.CustomerID) + 1 : 1;
                    }

                    // Add to local list
                    _allCustomers.Add(_currentCustomer);
                }

                if (success)
                {
                    // Update UI
                    ApplyFilters();
                    UpdateEmptyState();

                    // Hide form
                    CustomerDetailsPanel.Visibility = Visibility.Collapsed;
                }
            }
            catch (NotImplementedException)
            {
                // If the repository is not implemented yet, just update local list
                if (!_isEditMode)
                {
                    // Set ID for new customer
                    _currentCustomer.CustomerID = _allCustomers.Count > 0 ?
                        _allCustomers.Max(c => c.CustomerID) + 1 : 1;

                    // Add to local list
                    _allCustomers.Add(_currentCustomer);
                }

                // Update UI
                ApplyFilters();
                UpdateEmptyState();

                // Hide form
                CustomerDetailsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide form without saving
            CustomerDetailsPanel.Visibility = Visibility.Collapsed;
        }

        private async void ShowError(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}

