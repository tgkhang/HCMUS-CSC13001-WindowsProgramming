using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using POS_For_Small_Shop.Views.ShiftPage;

namespace POS_For_Small_Shop.Views
{
    public sealed partial class OpenShiftPage : Page
    {
        // Dictionary to map page tags to their corresponding types
        private readonly Dictionary<string, Type> _pageMappings = new()
        {
            { "NewOrderPage", typeof(NewOrderPage) },
            { "ShiftCustomerPage", typeof(ShiftCustomerPage) },
            { "ShiftOrderHistoryPage", typeof(ShiftOrderHistoryPage) },
            { "CloseShiftPage", typeof(CloseShiftPage) },
            { "HomePage", typeof(HomePage) },
            { "HelpPage", typeof(HelpPage) },
        };

        // Cache the current page to avoid unnecessary navigation
        private string _currentPageTag = string.Empty;

        private IDao _dao;
        private Shift _currentShift;
        private bool _isInitialized = false;

        public OpenShiftPage()
        {
            this.InitializeComponent();

            // Get the DAO from the service
            _dao = Service.GetKeyedSingleton<IDao>();

            // Initialize or get the current shift
            InitializeShift();
        }

        private void ShiftNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
            {
                // Set the default selected item and navigate to it
                if (ShiftNavigation.MenuItems.Count > 0)
                {
                    // Find the first menu item with a tag
                    var firstItem = ShiftNavigation.MenuItems
                        .OfType<NavigationViewItem>()
                        .FirstOrDefault(item => item.Tag != null);

                    if (firstItem != null)
                    {
                        ShiftNavigation.SelectedItem = firstItem;
                        NavigateToPage(firstItem.Tag.ToString());
                    }
                }
                _isInitialized = true;
            }
        }

        private void InitializeShift()
        {
            try
            {
                // Try to get the current open shift from the repository
                // For now, we'll create a mock shift
                _currentShift = new Shift
                {
                    ShiftID = 1001,
                    StartTime = DateTime.Now,
                    OpeningCash = 500000, // 500,000 VND
                    TotalSales = 0,
                    TotalOrders = 0,
                    Status = "Open"
                };

                // Update the UI with shift information
                UpdateShiftInfo();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur
                ShowError($"Error initializing shift: {ex.Message}");
            }
        }

        private void UpdateShiftInfo()
        {
            if (_currentShift != null)
            {
                ShiftNumberText.Text = _currentShift.ShiftID.ToString();
                ShiftStartTimeText.Text = _currentShift.StartTime.ToString("MMM d, h:mm tt");
                ShiftTotalSalesText.Text = string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:#,##0} đ", _currentShift.TotalSales);
                ShiftOrderCountText.Text = _currentShift.TotalOrders.ToString();
            }
        }

        private void Navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked) return;

            if (args.InvokedItemContainer is NavigationViewItem item && item.Tag is string tag)
            {
                NavigateToPage(tag);
            }
        }

        private void NavigateToPage(string tag)
        {
            // Skip navigation if we're already on this page
            if (tag == _currentPageTag) return;

            try
            {
                if (_pageMappings.TryGetValue(tag, out Type pageType))
                {
                    // Special case for HomePage - we want to navigate back to home
                    if (tag == "HomePage")
                    {
                        Frame.Navigate(typeof(HomePage));
                        return;
                    }

                    // For all other pages, navigate within the container
                    Container.Navigate(pageType);
                    _currentPageTag = tag;

                    // Update the selected item in the navigation view
                    UpdateSelectedNavItem(tag);
                }
                else
                {
                    ShowError($"Page '{tag}' not found in page mappings.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error navigating to page: {ex.Message}");
            }
        }

        private void UpdateSelectedNavItem(string tag)
        {
            // First check main menu items
            var menuItem = ShiftNavigation.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Tag?.ToString() == tag);

            if (menuItem != null)
            {
                ShiftNavigation.SelectedItem = menuItem;
                return;
            }

            // Then check footer menu items
            var footerItem = ShiftNavigation.FooterMenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Tag?.ToString() == tag);

            if (footerItem != null)
            {
                ShiftNavigation.SelectedItem = footerItem;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // In a real implementation, you would refresh the shift data from the database
                // For now, just update the UI
                UpdateShiftInfo();

                // Also refresh the current page if it implements IRefreshable
                if (Container.Content is IRefreshable refreshable)
                {
                    refreshable.Refresh();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error refreshing shift data: {ex.Message}");
            }
        }

        private async void PrintShiftReportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // In a real implementation, you would generate and print a shift report
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Print Report",
                    Content = "Generating shift report...",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Error printing report: {ex.Message}");
            }
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

    // Interface for pages that can be refreshed
    public interface IRefreshable
    {
        void Refresh();
    }
}

