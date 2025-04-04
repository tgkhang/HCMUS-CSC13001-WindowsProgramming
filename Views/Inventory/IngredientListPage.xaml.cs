using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Navigation;
using POS_For_Small_Shop.ViewModels;
using POS_For_Small_Shop.ViewModels.MenuManagement;

namespace POS_For_Small_Shop.Views.Inventory
{
    public sealed partial class IngredientListPage : Page
    {
        public InventoryViewModel ViewModel { get; } = new InventoryViewModel();

        public IngredientListPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;

            ViewModel.Initialize();
            IngredientListView.ItemsSource = ViewModel.FilteredIngredients;
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

        private void UpdateEmptyState()
        {
            EmptyStateText.Visibility = ViewModel.FilteredIngredients.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void IngredientListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle selection change if necessary (not currently used)
        }

        private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IsEditMode = false;
            ViewModel.CurrentIngredient = new Ingredient();
            FormHeaderText.Text = "Add Ingredient";
            IngredientDetailsPanel.Visibility = Visibility.Visible;

            // Clear form fields
            IngredientIDTextBox.Text = string.Empty;
            IngredientNameTextBox.Text = string.Empty;
            CategoryIDTextBox.Text = string.Empty;
            StockTextBox.Text = string.Empty;
            UnitTextBox.Text = string.Empty;
            SupplierTextBox.Text = string.Empty;
            PurchasePriceTextBox.Text = string.Empty;
            ExpiryDatePicker.Date = DateTimeOffset.Now;
        }

        private void EditIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int ingredientId)
            {
                Ingredient ingredients = ViewModel.AllIngredients.FirstOrDefault(item => item.IngredientID == ingredientId);
                if (ingredients != null)
                {
                    ViewModel.CurrentIngredient = ingredients;
                    IngredientIDTextBox.Text = ingredients.IngredientID.ToString();
                    IngredientNameTextBox.Text = ingredients.IngredientName;
                    CategoryIDTextBox.Text = ingredients.CategoryID.ToString();
                    StockTextBox.Text = ingredients.Stock.ToString();
                    UnitTextBox.Text = ingredients.Unit;
                    SupplierTextBox.Text = ingredients.Supplier;
                    PurchasePriceTextBox.Text = ingredients.PurchasePrice.ToString();
                    ExpiryDatePicker.Date = ingredients.ExpiryDate ?? DateTimeOffset.Now;
                    FormHeaderText.Text = "Edit Ingredient";
                    ViewModel.IsEditMode = true;
                    IngredientDetailsPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(IngredientNameTextBox.Text))
            {
                ShowError("Ingredient name is required.");
                return;
            }

            if (!float.TryParse(PurchasePriceTextBox.Text, out float price))
            {
                ShowError("Please enter a valid price.");
                return;
            }

            ViewModel.CurrentIngredient.IngredientID = int.Parse(IngredientIDTextBox.Text);
            ViewModel.CurrentIngredient.IngredientName = IngredientNameTextBox.Text;
            ViewModel.CurrentIngredient.Stock = int.Parse(StockTextBox.Text);
            ViewModel.CurrentIngredient.Unit = UnitTextBox.Text;
            ViewModel.CurrentIngredient.Supplier = SupplierTextBox.Text;
            ViewModel.CurrentIngredient.CategoryID = int.Parse(CategoryIDTextBox.Text);
            ViewModel.CurrentIngredient.PurchasePrice = float.Parse(PurchasePriceTextBox.Text);
            ViewModel.CurrentIngredient.ExpiryDate = ExpiryDatePicker.Date.DateTime;

            bool success = ViewModel.SaveIngredient();

            if (success)
            {
                ViewModel.ApplyFilters();
                UpdateEmptyState();
                IngredientDetailsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IngredientDetailsPanel.Visibility = Visibility.Collapsed;
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

        private async void DeleteIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int ingredientId)
            {
                var itemToDelete = ViewModel.AllIngredients.FirstOrDefault(item => item.IngredientID == ingredientId);
                if (itemToDelete != null)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Delete Ingredient",
                        Content = $"Are you sure you want to delete {itemToDelete.IngredientName}? This action cannot be undone.",
                        PrimaryButtonText = "Delete",
                        CloseButtonText = "Cancel",
                        DefaultButton = ContentDialogButton.Close,
                        XamlRoot = this.XamlRoot
                    };

                    var result = await dialog.ShowAsync();

                    if (result == ContentDialogResult.Primary)
                    {
                        bool success = ViewModel.DeleteIngredient(ingredientId);
                        if (success)
                        {
                            ViewModel.ApplyFilters();
                            UpdateEmptyState();
                        }
                    }
                }
            }
        }
    }
}
