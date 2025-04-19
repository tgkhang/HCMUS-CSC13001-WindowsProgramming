using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using POS_For_Small_Shop.ViewModels;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.ReceiptPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewReceipts : Page
    {
        public ReceiptManagementViewModel ViewModel { get; set; } = new ReceiptManagementViewModel();
        public ViewReceipts()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
            if (OrderDetailsPanel.Visibility == Visibility.Collapsed)
            {
                Grid.SetColumnSpan(ShiftsListViewPanel, 2);
            }
        }

        /// <summary>
        ///     This method is called to load the list of shifts when expanding the expander.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ShowOrder_OnExpanding(Expander sender, ExpanderExpandingEventArgs args) {
            foreach (var item in ShiftsListView.Items)
            {
                if (ShiftsListView.ContainerFromItem(item) is ListViewItem container)
                {
                    if (container.ContentTemplateRoot is Expander expander && expander != sender)
                    {
                        expander.IsExpanded = false;
                    }
                }
            }
            if (sender.Tag is int shiftID)
            {
                ViewModel.GetOrderByShiftIDCommand.Execute(shiftID);
            }

        }

        /// <summary>
        ///     This method is called when the user clicks on the button to show order details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowOrderDetails_ButtonCLick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var orderID = (int)button.Tag;
                ViewModel.GetOrderByOrderIDCommand.Execute(orderID);
                OrderDetailsPanel.Visibility = Visibility.Visible;
                Grid.SetColumnSpan(ShiftsListViewPanel, 1);

                Debug.WriteLine($"Order ID: {ViewModel.SelectedOrder.OrderID}");
                Debug.WriteLine($"Order Details: {ViewModel.OrderDetailsWithMenuItems.Count} items");
            }
        }

        /// <summary>
        ///     This method is called when the user clicks on the button to hide order details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HideOrderDetails_ButtonCLick(object sender, RoutedEventArgs e)
        {
            OrderDetailsPanel.Visibility = Visibility.Collapsed;
            Grid.SetColumnSpan(ShiftsListViewPanel, 2);
        }

        /// <summary>
        ///     This method is called when the user clicks on the button to print or save as PDF.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PrintSaveAsPDF_Click(object sender, RoutedEventArgs e)
        {
            // Implement the logic to print or save as PDF
            // You can use a library like PdfSharp or any other PDF library to create a PDF
            // and then save it to the desired location.
            Debug.WriteLine("Print/Save as PDF clicked");
        }
    }

}
