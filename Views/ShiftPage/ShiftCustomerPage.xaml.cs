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
        public CustomerManagementViewModel ViewModel { get; } = new CustomerManagementViewModel();

        public ShiftCustomerPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
            // Initialize the ViewModel and connect the ListView
            ViewModel.Initialize();
            CustomerListView.ItemsSource = ViewModel.FilteredCustomers;
        }
        private void UpdateEmptyState()
        {
            EmptyStateText.Visibility = ViewModel.FilteredCustomers.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.SearchText = sender.Text;
                ViewModel.ApplyFilters();
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
            ViewModel.IsEditMode = false;
            ViewModel.CurrentCustomer = new Customer();
            FormHeaderText.Text = "Add Customer";
            CustomerDetailsPanel.Visibility = Visibility.Visible;
            // Clear form fields
            NameTextBox.Text = string.Empty;
            PhoneTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            AddressTextBox.Text = string.Empty;
            LoyaltyPointsBox.Value = 0;
        }

        private void EditCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int customerId)
            {
                Customer customer = ViewModel.AllCustomers.FirstOrDefault(c => c.CustomerID == customerId);
                if (customer != null)
                {
                    ViewModel.CurrentCustomer = customer;
                    ViewModel.IsEditMode = true;
                    FormHeaderText.Text = "Edit Customer";
                    CustomerDetailsPanel.Visibility = Visibility.Visible;
                    // Fill form fields
                    NameTextBox.Text = customer.Name;
                    PhoneTextBox.Text = customer.Phone;
                    EmailTextBox.Text = customer.Email;
                    AddressTextBox.Text = customer.Address;
                    LoyaltyPointsBox.Value = customer.LoyaltyPoints;
                }
            }
        }

        private async void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int customerId)
            {
                var customerToDelete = ViewModel.AllCustomers.FirstOrDefault(c => c.CustomerID == customerId);
                if (customerToDelete != null)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Delete Customer",
                        Content = $"Are you sure you want to delete {customerToDelete.Name}?",
                        PrimaryButtonText = "Delete",
                        CloseButtonText = "Cancel",
                        XamlRoot = this.XamlRoot
                    };
                    var result = await dialog.ShowAsync();
                    if (result == ContentDialogResult.Primary)
                    {
                        bool success = ViewModel.DeleteCustomer(customerId);
                        if (success)
                        {
                            ViewModel.LoadCustomers();
                            UpdateEmptyState();
                        }
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                ShowError("Please enter a name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                ShowError("Please enter a phone number.");
                return;
            }
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                ShowError("Please enter an email.");
                return;
            }
            if (string.IsNullOrWhiteSpace(AddressTextBox.Text))
            {
                ShowError("Please enter an address.");
                return;
            }
            if (LoyaltyPointsBox.Value < 0)
            {
                ShowError("Please enter a valid loyalty points value.");
                return;
            }

            ViewModel.CurrentCustomer.Name = NameTextBox.Text;
            ViewModel.CurrentCustomer.Phone = PhoneTextBox.Text;
            ViewModel.CurrentCustomer.Email = EmailTextBox.Text;
            ViewModel.CurrentCustomer.Address = AddressTextBox.Text;
            ViewModel.CurrentCustomer.LoyaltyPoints = (int)LoyaltyPointsBox.Value;

            bool success = ViewModel.SaveCustomer();
            if (success)
            {
                ViewModel.ApplyFilters();
                UpdateEmptyState();
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

