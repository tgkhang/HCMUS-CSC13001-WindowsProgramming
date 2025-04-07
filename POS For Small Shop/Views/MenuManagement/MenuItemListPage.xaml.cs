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
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.ViewModels.MenuManagement;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.MenuManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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

            PopulateCategoryComboBox();
            CategoryComboBox.SelectedIndex = -1;
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
                    

                    PopulateCategoryComboBox();

                    for (int i = 0; i < CategoryComboBox.Items.Count; i++)
                    {
                        if (CategoryComboBox.Items[i] is Category category &&
                            category.CategoryID == menuItem.CategoryID)
                        {
                            CategoryComboBox.SelectedIndex = i;
                            break;
                        }
                    }

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
            if (CategoryComboBox.SelectedItem == null)
            {
                ShowError("Please select a category.");
                return;
            }

            ViewModel.CurrentMenuItem.Name = NameTextBox.Text;
            ViewModel.CurrentMenuItem.SellingPrice = price;
            ViewModel.CurrentMenuItem.ImagePath = ImagePathTextBox.Text;
            if (CategoryComboBox.SelectedItem is Category selectedCategory)
            {
                ViewModel.CurrentMenuItem.CategoryID = selectedCategory.CategoryID;
            }

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
        private void PopulateCategoryComboBox()
        {
            // Clear existing items
            CategoryComboBox.Items.Clear();

            // Add categories from the ViewModel
            foreach (var category in ViewModel.AllCategories)
            {
                CategoryComboBox.Items.Add(category);
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
