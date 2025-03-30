using System;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POS_For_Small_Shop.Views.MenuManagement;
using POS_For_Small_Shop.Views.ShiftPage;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;

namespace POS_For_Small_Shop.Views
{
    public sealed partial class HomePage : Page
    {
        private DispatcherTimer _timer;

        public HomePage()
        {
            this.InitializeComponent();
            
            // Initialize and start the timer for updating the date/time
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            
            // Update the date/time immediately
            UpdateDateTime();
        }

        private void Timer_Tick(object sender, object e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            var now = DateTime.Now;
            DateTimeText.Text = now.ToString("dddd, MMMM d, yyyy | h:mm tt");
        }

        private void OpenShiftButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OpenShiftPage));
        }

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Customers Management page
            Frame.Navigate(typeof(CustomerManagementPage));
        }

        private void ReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Receipt Management page
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Settings page
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Menu Items page
            Frame.Navigate(typeof(MenuManagementPage));
        }

        private void PromotionButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Promotion page
            Frame.Navigate(typeof(PromotionManagementPage));
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Inventory page
            Frame.Navigate(typeof(InventoryPage));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new LoginWindow();
            screen.Activate();
            DashboardWindow.Instance.Close();
        }
    }
}

