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
using POS_For_Small_Shop.Services;
using POS_For_Small_Shop.Views.ShiftPage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.MenuManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuManagementPage : Page
    {

        // Dictionary to map page tags to their corresponding types
        private readonly Dictionary<string, Type> _pageMappings = new()
            {
                { "MenuItemListPage", typeof(MenuItemListPage) },
                { "HomePage", typeof(HomePage) },
            };

        private string _currentPageTag = string.Empty;
        private IDao _dao;
        private bool _isInitialized = false;
        public MenuManagementPage()
        {
            this.InitializeComponent();

            // Get the DAO from the service
            _dao = Service.GetKeyedSingleton<IDao>();
        }

        private void MenuItemNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
            {

                if (MenuItemNavigation.MenuItems.Count > 0)
                {
                    var firstItem = MenuItemNavigation.MenuItems
                        .OfType<NavigationViewItem>()
                        .FirstOrDefault(item => item.Tag != null);

                    if (firstItem != null)
                    {
                        MenuItemNavigation.SelectedItem = firstItem;
                        NavigateToPage(firstItem.Tag.ToString());
                    }
                }
                _isInitialized = true;
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
            var menuItem = MenuItemNavigation.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Tag?.ToString() == tag);

            if (menuItem != null)
            {
                MenuItemNavigation.SelectedItem = menuItem;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Refresh the MenuItem list
                UpdateMenuItemList();

                if (Container.Content is IRefreshable refreshable)
                {
                    refreshable.Refresh();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error refreshing MenuItem data: {ex.Message}");
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

        private void UpdateMenuItemList()
        {
            //refresh and update MenuItem data.
        }
    }
}
