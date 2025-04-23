using System;
using System.Collections.Generic;
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
using POS_For_Small_Shop.Views.CustomerManagement;
using POS_For_Small_Shop.Views.MenuManagement;
using POS_For_Small_Shop.Views.Inventory;
using POS_For_Small_Shop.Views.ShiftPage;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using Windows.Foundation.Collections;
using POS_For_Small_Shop.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private DispatcherTimer _timer;
        public NotificationService NotificationService { get; set; }

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

            NotificationService = new NotificationService(DashboardWindow.Instance.CurrentFrame);
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
            Frame.Navigate(typeof(ReceiptManagementPage));
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
            // Handle logout
            try
            {
                // Default window position (0,0)
                int windowX = 0;
                int windowY = 0;

                // Create and get AppWindow for login window
                var loginScreen = new LoginWindow();
                var loginAppWindow = loginScreen.AppWindow;
                if (loginAppWindow == null) return; // Exit if AppWindow is null
                loginScreen.Activate(); // Focus the login window

                if (DashboardWindow.Instance != null)
                {
                    var currentAppWindow = DashboardWindow.Instance.AppWindow; // Get current window
                    if (currentAppWindow != null)
                    {
                        windowX = currentAppWindow.Position.X; // Get X position
                        windowY = currentAppWindow.Position.Y; // Get Y position
                    }
                    DashboardWindow.Instance.Close(); // Close dashboard
                    loginAppWindow.Move(new Windows.Graphics.PointInt32(windowX, windowY)); // Move login window
                }
            }
            catch
            {
                //Do nothing
            }
        }
    }
}
