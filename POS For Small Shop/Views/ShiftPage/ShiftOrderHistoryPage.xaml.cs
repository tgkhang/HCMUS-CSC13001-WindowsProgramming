using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using POS_For_Small_Shop.ViewModels.ShiftPage;
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
        public ShiftOrderHistoryViewmodel ViewModel { get; } = new ShiftOrderHistoryViewmodel();
        private IDao _dao;
        private string _searchText = "";

        public ShiftOrderHistoryPage()
        {
            this.InitializeComponent();
            _dao = Service.GetKeyedSingleton<IDao>();
            DataContext = ViewModel;

            ViewModel.Initialize(_dao, Service.GetKeyedSingleton<IShiftService>());
            ViewModel.LoadOrders();
            OrderListView.ItemsSource = ViewModel.FilteredShiftOrders;
            UpdateEmptyState();

        }
        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                _searchText = sender.Text;
                ViewModel.SetSearchText(_searchText);
                UpdateEmptyState();
            }
        }
        private void OrderListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderListView.SelectedItem is Order selectedOrder)
            {
                // Show the details panel
                OrderDetailsPanel.Visibility = Visibility.Visible;

                // TODO: Populate the details panel with the selected order information
                FormHeaderText.Text = $"Order #{selectedOrder.OrderID} Details";

                // Here you would populate the form fields with the order details
            }
            else
            {
                // Hide the details panel if nothing is selected
                OrderDetailsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag != null)
            {
                if (int.TryParse(button.Tag.ToString(), out int orderTypeId))
                {
                    string orderType;
                    switch (orderTypeId)
                    {
                        case 0:
                            orderType = "All";
                            break;
                        case 1:
                            orderType = "Pending";
                            break;
                        case 2:
                            orderType = "Completed";
                            break;
                        case 3:
                            orderType = "Canceled";
                            break;
                        default:
                            orderType = "All";
                            break;
                    }
                    ViewModel.SetOrderType(orderType);
                    if (button.Parent is StackPanel orderStackPanel)
                    {
                        foreach (var child in orderStackPanel.Children)
                        {
                            if (child is Button orderButton)
                            {
                                orderButton.Style = (orderButton == button)
                                    ? Application.Current.Resources["AccentButtonStyle"] as Style
                                    : null;
                            }
                        }
                    }
                    UpdateEmptyState();
                }
            }
        }

        private void UpdateEmptyState()
        {
           EmptyStateText.Visibility = (ViewModel.FilteredShiftOrders.Count == 0)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}