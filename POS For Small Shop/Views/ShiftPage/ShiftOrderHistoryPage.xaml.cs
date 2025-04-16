using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Text;
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

                FormHeaderText.Text = $"Order #{selectedOrder.OrderID} Details";
                Receipt receipt = _dao.OrderDetails.getReceiptDetailByOrderId(selectedOrder.OrderID);
                if (receipt != null && selectedOrder.Status!="Pending")
                {
                    DisplayReceiptDetails(receipt);
                }
                else
                {
                    FormHeaderText.Text += " - Failed to load details";
                }
            }
            else
            {
                // Hide the details panel if nothing is selected
                OrderDetailsPanel.Visibility = Visibility.Collapsed;
            }
        }
        private void DisplayReceiptDetails(Receipt receipt)
        {
            ReceiptItemsPanel.Children.Clear();

            if (receipt?.Data?.AllOrderDetails?.Nodes == null)
                return;

            // Add header
            TextBlock headerText = new TextBlock
            {
                Text = $"Order #{receipt.Data.AllOrderDetails.Nodes[0].OrderId}",
                Style = Application.Current.Resources["SubtitleTextBlockStyle"] as Style,
                Margin = new Thickness(0, 0, 0, 10)
            };
            ReceiptItemsPanel.Children.Add(headerText);

            // Add items
            foreach (var item in receipt.Data.AllOrderDetails.Nodes)
            {
                Grid itemGrid = new Grid();
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                itemGrid.Margin = new Thickness(0, 5, 0, 5);

                // Item name
                TextBlock nameText = new TextBlock
                {
                    Text = item.MenuItemByMenuItemId.Name,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(nameText, 0);

                // Quantity
                TextBlock qtyText = new TextBlock
                {
                    Text = $"x{item.Quantity}",
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 10, 0)
                };
                Grid.SetColumn(qtyText, 1);

                // Subtotal
                TextBlock subtotalText = new TextBlock
                {
                    Text = $"₫{item.Subtotal:N0}",
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Right
                };
                Grid.SetColumn(subtotalText, 2);

                // Add elements to grid
                itemGrid.Children.Add(nameText);
                itemGrid.Children.Add(qtyText);
                itemGrid.Children.Add(subtotalText);

                // Add grid to panel
                ReceiptItemsPanel.Children.Add(itemGrid);
            }

         

            // Add totals
            float totalAmount = receipt.Data.AllOrderDetails.Nodes.Sum(item => item.Subtotal);

            // Get order from selected item for discount information
            Order selectedOrder = OrderListView.SelectedItem as Order;

            Grid totalGrid = new Grid();
            totalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            totalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            totalGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            totalGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            totalGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Subtotal row
            TextBlock subtotalLabel = new TextBlock { Text = "Subtotal:", Margin = new Thickness(0, 2, 0, 2) };
            Grid.SetRow(subtotalLabel, 0);
            Grid.SetColumn(subtotalLabel, 0);

            TextBlock subtotalValue = new TextBlock
            {
                Text = $"₫{totalAmount:N0}",
                TextAlignment = TextAlignment.Right,
                Margin = new Thickness(0, 2, 0, 2)
            };
            Grid.SetRow(subtotalValue, 0);
            Grid.SetColumn(subtotalValue, 1);

            // Discount row
            TextBlock discountLabel = new TextBlock
            {
                Text = "Discount:",
                Margin = new Thickness(0, 2, 0, 2)
            };
            Grid.SetRow(discountLabel, 1);
            Grid.SetColumn(discountLabel, 0);

            TextBlock discountValue = new TextBlock
            {
                Text = $"₫{selectedOrder?.Discount:N0}",
                TextAlignment = TextAlignment.Right,
                Margin = new Thickness(0, 2, 0, 2)
            };
            Grid.SetRow(discountValue, 1);
            Grid.SetColumn(discountValue, 1);

            // Total row
            TextBlock totalLabel = new TextBlock
            {
                Text = "Total:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 2, 0, 2)
            };
            Grid.SetRow(totalLabel, 2);
            Grid.SetColumn(totalLabel, 0);

            TextBlock totalValue = new TextBlock
            {
                Text = $"₫{selectedOrder?.FinalAmount:N0}",
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Right,
                Margin = new Thickness(0, 2, 0, 2)
            };
            Grid.SetRow(totalValue, 2);
            Grid.SetColumn(totalValue, 1);

            // Add elements to grid
            totalGrid.Children.Add(subtotalLabel);
            totalGrid.Children.Add(subtotalValue);
            totalGrid.Children.Add(discountLabel);
            totalGrid.Children.Add(discountValue);
            totalGrid.Children.Add(totalLabel);
            totalGrid.Children.Add(totalValue);

            // Add grid to panel
            ReceiptItemsPanel.Children.Add(totalGrid);
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            OrderDetailsPanel.Visibility = Visibility.Collapsed;
        }
    }
}