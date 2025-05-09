﻿using System;
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
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using Windows.Foundation;
using POS_For_Small_Shop.Views.ShiftPage;
using Windows.Foundation.Collections;
using System.Text.RegularExpressions;
using System.Globalization;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OpenShiftPage : Page
    {
        private readonly Dictionary<string, Type> _pageMappings = new()
        {
            { "NewOrderPage", typeof(NewOrderPage) },
            { "ShiftCustomerPage", typeof(ShiftCustomerPage) },
            { "ShiftOrderHistoryPage", typeof(ShiftOrderHistoryPage) },
            { "CloseShiftPage", typeof(CloseShiftPage) },
            { "HomePage", typeof(HomePage) },
            { "HelpPage", typeof(HelpPage) },
            { "PaymentPage", typeof(PaymentPage) }
        };

        // avoid unnecessary navigation
        private string _currentPageTag = string.Empty;

        private IDao _dao;
        private bool _isInitialized = false;
        private float _openingCashAmount = 0;
        private bool _isShiftInitialized = false;

        private IShiftService _shiftService;

        public OpenShiftPage()
        {
            this.InitializeComponent();

            _dao = Service.GetKeyedSingleton<IDao>();
            _shiftService = Service.GetKeyedSingleton<IShiftService>();
            // Subscribe to the ShiftUpdated event
            _shiftService.ShiftUpdated += ShiftService_ShiftUpdated;

            // Show the opening cash dialog when the page is loaded
            this.Loaded += OpenShiftPage_Loaded;
        }

        private async void OpenShiftPage_Loaded(object sender, RoutedEventArgs e)
        {
            OpeningCashDialog.XamlRoot = this.XamlRoot;
            ContentDialogResult result = await OpeningCashDialog.ShowAsync();
            if (result == ContentDialogResult.None)
            {
                Frame.Navigate(typeof(HomePage));
            }
        }

        // Event handler for when the shift is updated
        private void ShiftService_ShiftUpdated(object sender, EventArgs e)
        {
            UpdateShiftInfo();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            // Unsubscribe from the event when navigating away to prevent memory leaks
            _shiftService.ShiftUpdated -= ShiftService_ShiftUpdated;
        }
        private void ShiftNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized && _isShiftInitialized)
            {
                // Only set the default navigation after shift is initialized
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
                Shift currentShift = new Shift
                {
                    ShiftID = 8386, // Mock ID - > auto real id
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    OpeningCash = _openingCashAmount,
                    ClosingCash = 0,
                    TotalSales = 0,
                    TotalOrders = 0,
                    Status = "Open"
                };

                currentShift.ShiftID = _dao.Shifts.CreateGetId(currentShift);

                _shiftService.SetCurrentShift(currentShift);
                _isShiftInitialized = true;

                UpdateShiftInfo();
                if (ShiftNavigation.MenuItems.Count > 0 && !_isInitialized)
                {
                    var firstItem = ShiftNavigation.MenuItems
                        .OfType<NavigationViewItem>()
                        .FirstOrDefault(item => item.Tag != null);

                    if (firstItem != null)
                    {
                        ShiftNavigation.SelectedItem = firstItem;
                        NavigateToPage(firstItem.Tag.ToString());
                    }
                    _isInitialized = true;
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error initializing shift: {ex.Message}");
            }
        }

        private void UpdateShiftInfo()
        {
            // We need to ensure this runs on the UI thread since it's updating UI elements
            DispatcherQueue.TryEnqueue(() =>
            {
                Shift currentShift = _shiftService.CurrentShift;
                if (currentShift != null)
                {
                    ShiftNumberText.Text = currentShift.ShiftID.ToString();
                    ShiftStartTimeText.Text = currentShift.StartTime.ToString("MMM d, h:mm tt");
                    ShiftTotalSalesText.Text = string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:#,##0} đ", currentShift.TotalSales);
                    ShiftOrderCountText.Text = currentShift.TotalOrders.ToString();
                    OpeningCashText.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0} đ", currentShift.OpeningCash);
                }
            });
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
        private void OpeningCashTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = OpeningCashTextBox.Text.Trim();

            // Remove non-numeric characters
            string numericOnly = Regex.Replace(input, "[^0-9]", "");

            // Parse the numeric value
            if (float.TryParse(numericOnly, out float amount))
            {
                _openingCashAmount = amount;

                // Format and display the amount
                FormattedCashTextBlock.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0} đ", amount);

                // Clear validation message
                ValidationMessageTextBlock.Text = "";
                ValidationMessageTextBlock.Visibility = Visibility.Collapsed;

                // Enable the primary button
                OpeningCashDialog.IsPrimaryButtonEnabled = true;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    _openingCashAmount = 0;
                    FormattedCashTextBlock.Text = "0 đ";
                    ValidationMessageTextBlock.Visibility = Visibility.Collapsed;
                    OpeningCashDialog.IsPrimaryButtonEnabled = true;
                }
                else
                {
                    // Show validation error
                    ValidationMessageTextBlock.Text = "Please enter a valid number";
                    ValidationMessageTextBlock.Visibility = Visibility.Visible;
                    OpeningCashDialog.IsPrimaryButtonEnabled = false;
                }
            }
        }

        private void OpeningCashDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            InitializeShift();
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