using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.ViewModels.MenuManagement;

namespace POS_For_Small_Shop.Views.MenuManagement
{
    public sealed partial class MenuItemListPage : Page
    {
        public MenuItemListViewModel ViewModel { get; } = new MenuItemListViewModel();

        public MenuItemListPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;

            // Initialize the ViewModel and connect the ListView
            ViewModel.Initialize();
            ItemListView.ItemsSource = ViewModel.FilteredMenuItems;
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.SearchText = sender.Text;
                ViewModel.ApplyFilters();
                UpdateEmptyState();
            }
        }

        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle selection change if necessary
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IsEditMode = false;
            ViewModel.CurrentMenuItem = new MenuItem();
            FormHeaderText.Text = "Add Menu Item";
            ItemDetailsPanel.Visibility = Visibility.Visible;

            // Clear form fields
            NameTextBox.Text = string.Empty;
            PriceTextBox.Text = string.Empty;
            ImagePathTextBox.Text = string.Empty;
        }

        private void EditItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int menuItemId)
            {
                MenuItem menuItem = ViewModel.AllMenuItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
                if (menuItem != null)
                {
                    ViewModel.CurrentMenuItem = menuItem;
                    NameTextBox.Text = menuItem.Name;
                    PriceTextBox.Text = menuItem.SellingPrice.ToString();
                    ImagePathTextBox.Text = menuItem.ImagePath ?? "";
                    FormHeaderText.Text = "Edit Menu Item";
                    ViewModel.IsEditMode = true;
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

            if (!float.TryParse(PriceTextBox.Text, out float price))
            {
                ShowError("Please enter a valid price.");
                return;
            }

            ViewModel.CurrentMenuItem.Name = NameTextBox.Text;
            ViewModel.CurrentMenuItem.SellingPrice = price;
            ViewModel.CurrentMenuItem.ImagePath = ImagePathTextBox.Text;

            bool success = ViewModel.SaveMenuItem();

            if (success)
            {
                ViewModel.ApplyFilters();
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
                var itemToDelete = ViewModel.AllMenuItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
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
                        bool success = ViewModel.DeleteMenuItem(menuItemId);
                        if (success)
                        {
                            ViewModel.ApplyFilters();
                            UpdateEmptyState();
                        }
                    }
                }
            }
        }

        private void UpdateEmptyState()
        {
            EmptyStateText.Visibility = ViewModel.FilteredMenuItems.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}