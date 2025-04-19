using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using PropertyChanged;

namespace POS_For_Small_Shop.ViewModels.ShiftPage
{
    [AddINotifyPropertyChangedInterface]
    public class ShiftOrderHistoryViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private IDao _dao;
        private IShiftService _shiftService;
        private string _orderType = "Completed";
        private string _searchText = "";
        private Shift _currentShift;

        public Visibility EmptyStateVisibility => FilteredShiftOrders.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        public ObservableCollection<Order> AllShiftOrders { get; private set; } = new ObservableCollection<Order>();
        public ObservableCollection<Order> FilteredShiftOrders { get; private set; } = new ObservableCollection<Order>();

        public ShiftOrderHistoryViewmodel()
        {
            AllShiftOrders = new ObservableCollection<Order>();
            FilteredShiftOrders = new ObservableCollection<Order>();
        }

        public void Initialize(IDao dao, IShiftService shiftService)
        {
            _dao = dao;
            _shiftService = shiftService;
            GetCurrentShift();
        }

        public void LoadOrders()
        {
            try
            {
                if (_currentShift == null || _dao == null)
                {
                    Debug.WriteLine("Cannot load orders: Current shift or DAO is null");
                    return;
                }

                var allOrders = _dao.Orders.getOrdersByShiftID(_currentShift.ShiftID);

                AllShiftOrders.Clear();

                foreach (var order in allOrders.Where(order => order.ShiftID == _currentShift.ShiftID))
                {
                    AllShiftOrders.Add(order);
                }
                ApplyFilters();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading orders: {ex.Message}");
            }
        }

        private void GetCurrentShift()
        {
            try
            {
                if (_shiftService == null)
                {
                    Debug.WriteLine("Cannot get current shift: ShiftService is null");
                    return;
                }

                _currentShift = _shiftService.CurrentShift;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting current shift: {ex.Message}");
                _currentShift = null;
            }
        }

        private void ApplyFilters()
        {
            var filteredOrders = AllShiftOrders.ToList();
            if (_orderType != "All")
            {
                filteredOrders = filteredOrders.Where(order => order.Status == _orderType).ToList();
            }

            // Apply search text filter
            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                filteredOrders = filteredOrders.Where(item =>
                    item.OrderID.ToString().Contains(_searchText, StringComparison.OrdinalIgnoreCase) ||
                    item.PaymentMethod.Contains(_searchText, StringComparison.OrdinalIgnoreCase) ||
                    item.Status.Contains(_searchText, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            FilteredShiftOrders.Clear();
            foreach (var item in filteredOrders)
            {
                FilteredShiftOrders.Add(item);
            }

            // Notify UI about empty state change
            OnPropertyChanged(nameof(EmptyStateVisibility));
        }

        public void SetSearchText(string text)
        {
            _searchText = text;
            ApplyFilters();
        }

        public void SetOrderType(string orderType)
        {
            _orderType = orderType;
            ApplyFilters();
        }
    }
}