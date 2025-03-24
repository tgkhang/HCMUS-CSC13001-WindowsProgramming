using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using System.Collections.ObjectModel;

namespace POS_For_Small_Shop.Views.MenuManagement
{
    public sealed partial class MenuItemListPage : Page
    {
        private IDao _dao;
        private List<MenuItem> _allMenuItems;
        private MenuItem _currentMenuItem;
        private bool _isEditMode = false;
        private string _searchText = "";

        public ObservableCollection<MenuItem> FilteredMenuItems { get; private set; } = new ObservableCollection<MenuItem>();

        public MenuItemListPage()
        {
            this.InitializeComponent();
            _dao = Service.GetKeyedSingleton<IDao>();
            LoadMenuItems();
            ItemListView.ItemsSource = FilteredMenuItems;
        }

        private void LoadMenuItems()
        {
            try
            {
                _allMenuItems = _dao.MenuItems.GetAll();
                ApplyFilters();
                UpdateEmptyState();
            }
            catch (NotImplementedException)
            {
                _allMenuItems = new List<MenuItem>();
                ApplyFilters();
                UpdateEmptyState();
            }
        }

        private void ApplyFilters()
        {
            var filteredItems = _allMenuItems;
            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                filteredItems = filteredItems.Where(item =>
                    item.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            FilteredMenuItems.Clear();
            foreach (var item in filteredItems)
            {
                FilteredMenuItems.Add(item);
            }
        }

        private void UpdateEmptyState()
        {
            EmptyStateText.Visibility = FilteredMenuItems.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
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

        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle selection change if necessary (not currently used)
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            _isEditMode = false;
            _currentMenuItem = new MenuItem();
            FormHeaderText.Text = "Add Menu Item";
            ItemDetailsPanel.Visibility = Visibility.Visible;
        }

        private void EditItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int menuItemId)
            {
                _currentMenuItem = _allMenuItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
                if (_currentMenuItem != null)
                {
                    NameTextBox.Text = _currentMenuItem.Name;
                    PriceTextBox.Text = _currentMenuItem.SellingPrice.ToString();
                    ImagePathTextBox.Text = _currentMenuItem.ImagePath ?? "";
                    FormHeaderText.Text = "Edit Menu Item";
                    _isEditMode = true;
                    ItemDetailsPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                ShowError("Name is required.");
                return;
            }

            _currentMenuItem.Name = NameTextBox.Text;
            _currentMenuItem.SellingPrice = float.Parse(PriceTextBox.Text);
            _currentMenuItem.ImagePath = ImagePathTextBox.Text;

            bool success;
            if (_isEditMode)
            {
                success = _dao.MenuItems.Update(_currentMenuItem.MenuItemID, _currentMenuItem);
            }
            else
            {
                success = _dao.MenuItems.Insert(_currentMenuItem);
                if (success)
                {
                    _currentMenuItem.MenuItemID = _allMenuItems.Count > 0 ? _allMenuItems.Max(item => item.MenuItemID) + 1 : 1;
                }
                _allMenuItems.Add(_currentMenuItem);
            }

            if (success)
            {
                ApplyFilters();
                UpdateEmptyState();
                ItemDetailsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ItemDetailsPanel.Visibility = Visibility.Collapsed;
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

        private async void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int menuItemId)
            {
                var itemToDelete = _allMenuItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
                if (itemToDelete != null)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Delete Menu Item",
                        Content = $"Are you sure you want to delete {itemToDelete.Name}? This action cannot be undone.",
                        PrimaryButtonText = "Delete",
                        CloseButtonText = "Cancel",
                        DefaultButton = ContentDialogButton.Close,
                        XamlRoot = this.XamlRoot
                    };

                    var result = await dialog.ShowAsync();

                    if (result == ContentDialogResult.Primary)
                    {
                        _dao.MenuItems.Delete(menuItemId);
                        _allMenuItems.Remove(itemToDelete);
                        ApplyFilters();
                        UpdateEmptyState();
                    }
                }
            }
        }
    }
}
