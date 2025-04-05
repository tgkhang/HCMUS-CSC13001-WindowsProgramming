using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.ShiftPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShiftOrderHistoryPage : Page
    {
        private IDao _dao;
        private List<Order> _allOrders;
        //TO DO
        //allorder in this shift only

        private Order _currentOrder;
        private string _searchText = "";


        public ObservableCollection<Order> FilteredOrders { get; private set; } = new ObservableCollection<Order>();


        public ShiftOrderHistoryPage()
        {
            this.InitializeComponent();

            _dao = Service.GetKeyedSingleton<IDao>();
            LoadOrders();
            OrderListView.ItemsSource = FilteredOrders;

        }

        private void LoadOrders()
        {
            try
            {
                //get all order
                _allOrders = _dao.Orders.GetAll();
                //aply search or filter
                ApplyFilters();
                //Updatee Ui base on custonmer
                UpdateEmptyState();
            }
            catch (NotImplementedException)
            {
                _allOrders = new List<Order>();
                ApplyFilters();
                UpdateEmptyState();
            }
        }
        private void ApplyFilters()
        {
            var filtered = _allOrders;

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                filtered = filtered.Where(order =>
                    order.OrderID.ToString().Contains(_searchText, StringComparison.OrdinalIgnoreCase) ||
                    order.Status.Contains(_searchText, StringComparison.OrdinalIgnoreCase) ||
                    order.PaymentMethod.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            FilteredOrders.Clear();
            foreach (var order in filtered)
            {
                FilteredOrders.Add(order);
            }
        }

        private void UpdateEmptyState()
        {
            EmptyStateText.Visibility = FilteredOrders.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
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

        private void OrderListView_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
