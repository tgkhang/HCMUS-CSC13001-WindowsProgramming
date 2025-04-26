using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.UI.Xaml.Navigation;
using POS_For_Small_Shop.ViewModels;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Diagnostics;

namespace POS_For_Small_Shop.Views.Inventory
{
    public sealed partial class InventoryPage : Page
    {
        private readonly Dictionary<string, Type> _pageMappings = new()
        {
            { "IngredientListPage", typeof(IngredientListPage) },
            { "HomePage", typeof(HomePage) },
        };

        private string _currentPageTag = string.Empty;
        private IDao _dao;
        private bool _isInitialized = false;

        public InventoryPage()
        {
            this.InitializeComponent();
            _dao = Service.GetKeyedSingleton<IDao>();
        }

        private void InventoryNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
            {
                if (InventoryNavigation.MenuItems.Count > 0)
                {
                    var firstItem = InventoryNavigation.MenuItems
                        .OfType<NavigationViewItem>()
                        .FirstOrDefault(item => item.Tag != null);

                    if (firstItem != null)
                    {
                        InventoryNavigation.SelectedItem = firstItem;
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
            if (tag == _currentPageTag) return;

            try
            {
                if (_pageMappings.TryGetValue(tag, out Type pageType))
                {
                    if (tag == "HomePage")
                    {
                        Frame.Navigate(typeof(HomePage));
                        return;
                    }

                    Container.Navigate(pageType);
                    _currentPageTag = tag;
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
            var menuItem = InventoryNavigation.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Tag?.ToString() == tag);

            if (menuItem != null)
            {
                InventoryNavigation.SelectedItem = menuItem;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateInventoryList();

                if (Container.Content is IRefreshable refreshable)
                {
                    refreshable.Refresh();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error refreshing Inventory data: {ex.Message}");
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

        private void UpdateInventoryList()
        {
            // Refresh and update Inventory data.
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is string itemName && !string.IsNullOrWhiteSpace(itemName))
            {
                this.Container.Navigate(typeof(IngredientListPage), itemName);
            }
            else
            {
                this.Container.Navigate(typeof(IngredientListPage));
            }
        }

    }
}
