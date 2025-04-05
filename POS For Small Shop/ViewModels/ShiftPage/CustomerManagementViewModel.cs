using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using PropertyChanged;

namespace POS_For_Small_Shop.ViewModels.ShiftPage
{
    [AddINotifyPropertyChangedInterface]
    public class CustomerManagementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private IDao _dao;
        private string _searchText = "";

        public List<Customer> AllCustomers { get; private set; } = new List<Customer>();
        public ObservableCollection<Customer> FilteredCustomers { get; private set; } = new ObservableCollection<Customer>();
        public Customer CurrentCustomer { get; set; }
        public bool IsEditMode { get; set; } = false;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public CustomerManagementViewModel()
        {
            _dao = Service.GetKeyedSingleton<IDao>();
        }
        public void Initialize()
        {
            LoadCustomers();
        }
        public void LoadCustomers()
        {
            try
            {
                AllCustomers = _dao.Customers.GetAll();
                ApplyFilters();
            }
            catch (NotImplementedException)
            {
                AllCustomers = new List<Customer>();
                ApplyFilters();
            }
        }
        public void ApplyFilters()
        {
            var filteredItems = AllCustomers;
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredItems = filteredItems.Where(item =>
                    item.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    item.Phone.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    item.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            FilteredCustomers.Clear();
            foreach (var item in filteredItems)
            {
                FilteredCustomers.Add(item);
            }
        }
        public bool SaveCustomer()
        {
            bool success;

            if (IsEditMode)
            {
                success = _dao.Customers.Update(CurrentCustomer.CustomerID, CurrentCustomer);

                // Update the in-memory item 
                if (success)
                {
                    // Find and update the item in AllCustomers if it exists
                    var existingCustomer = AllCustomers.FirstOrDefault(c => c.CustomerID == CurrentCustomer.CustomerID);
                    if (existingCustomer != null)
                    {
                        // Update all properties
                        existingCustomer.Name = CurrentCustomer.Name;
                        existingCustomer.Phone = CurrentCustomer.Phone;
                        existingCustomer.Email = CurrentCustomer.Email;
                        existingCustomer.Address = CurrentCustomer.Address;
                        existingCustomer.LoyaltyPoints = CurrentCustomer.LoyaltyPoints;
                    }
                }
            }
            else
            {
                success = _dao.Customers.Insert(CurrentCustomer);
                if (success)
                {
                    CurrentCustomer.CustomerID = AllCustomers.Count > 0 ? AllCustomers.Max(c => c.CustomerID) + 1 : 1;
                    AllCustomers.Add(CurrentCustomer);
                }
            }

            return success;
        }
        public bool DeleteCustomer(int customerId)
        {
            bool success = _dao.Customers.Delete(customerId);
            if (success)
            {
                var customerToDelete = AllCustomers.FirstOrDefault(c => c.CustomerID == customerId);
                if (customerToDelete != null)
                {
                    AllCustomers.Remove(customerToDelete);
                    ApplyFilters();
                }
            }
            return success;
        }
    }
}
