using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POS_For_Small_Shop.Data.Models;
using System.Globalization;
using POS_For_Small_Shop.Services;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Navigation;
using POS_For_Small_Shop.ViewModels;
using POS_For_Small_Shop.ViewModels.Inventory;
using System.Diagnostics;

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

        //private void PurchasePriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (sender is TextBox textBox)
        //    {
        //        string text = textBox.Text.Replace(",", "."); // Replace comma with period
        //                                                      // Allow only numbers, one decimal point, and optional minus sign
        //        if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"^-?\d*\.?\d*$"))
        //        {
        //            text = text.Substring(0, text.Length - 1); // Remove invalid character
        //        }
        //        if (text != textBox.Text)
        //        {
        //            int cursorPosition = textBox.SelectionStart;
        //            textBox.Text = text;
        //            textBox.SelectionStart = cursorPosition > text.Length ? text.Length : cursorPosition;
        //        }
        //    }
        //}

        private void UpdateEmptyState()
        {
            EmptyStateText.Visibility = ViewModel.FilteredIngredients.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
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
            // Handle selection change if necessary (not currently used)
        }

        private void UnitComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle selection change if necessary (not currently used)
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
            IngredientNameTextBox.Text = string.Empty;
            StockTextBox.Text = string.Empty;
            SupplierTextBox.Text = string.Empty;
            PurchasePriceTextBox.Text = string.Empty;
            ExpiryDatePicker.Date = DateTime.Now;

            PopulateCategoryComboBox();
            CategoryComboBox.SelectedIndex = -1;

            UnitComboBox.SelectedIndex = -1;
        }

        private void EditIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int ingredientId)
            {
                Ingredient ingredients = ViewModel.AllIngredients.FirstOrDefault(item => item.IngredientID == ingredientId);
                if (ingredients != null)
                {
                    ViewModel.CurrentIngredient = ingredients;

                    IngredientNameTextBox.Text = ingredients.IngredientName;
                    StockTextBox.Text = ingredients.Stock.ToString();
                    SupplierTextBox.Text = ingredients.Supplier;
                    PurchasePriceTextBox.Text = ingredients.PurchasePrice.ToString();
                    //PurchasePriceTextBox.Text = ingredients.PurchasePrice.ToString(CultureInfo.InvariantCulture);
                    ExpiryDatePicker.Date = ingredients.ExpiryDate ?? DateTime.Now;

                    FormHeaderText.Text = "Edit Ingredient";
                    ViewModel.IsEditMode = true;

                    PopulateCategoryComboBox();
                    for (int i = 0; i < CategoryComboBox.Items.Count; i++)
                    {
                        if (CategoryComboBox.Items[i] is Category category &&
                            category.CategoryID == ingredients.CategoryID)
                        {
                            CategoryComboBox.SelectedIndex = i;
                            break;
                        }
                    }

                    for (int i = 0; i < UnitComboBox.Items.Count; i++)
                    {
                        if (UnitComboBox.Items[i] is string unit &&
                            unit == ingredients.Unit)
                        {
                            UnitComboBox.SelectedIndex = i;
                            break;
                        }
                    }
                    IngredientDetailsPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(IngredientNameTextBox.Text))
            {
                ShowError("Name is required.");
                return;
            }

            if (!int.TryParse(StockTextBox.Text, out int stock))
            {
                ShowError("Please enter a valid stock.");
                return;
            }
            if (!float.TryParse(PurchasePriceTextBox.Text, out float price))
            {
                ShowError("Please enter a valid price.");
                return;
            }
            //if (!float.TryParse(PurchasePriceTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out float price))
            //{
            //    ShowError("Please enter a valid price.");
            //    return;
            //}
            if (CategoryComboBox.SelectedItem == null)
            {
                ShowError("Please select a category.");
                return;
            }

            if (UnitComboBox.SelectedItem == null)
            {
                ShowError("Please select a unit.");
                return;
            }

            ViewModel.CurrentIngredient.IngredientName = IngredientNameTextBox.Text;
            ViewModel.CurrentIngredient.Stock = stock;
            ViewModel.CurrentIngredient.Unit = UnitComboBox.SelectedItem.ToString();
            ViewModel.CurrentIngredient.Supplier = SupplierTextBox.Text;
            ViewModel.CurrentIngredient.PurchasePrice = price;
            ViewModel.CurrentIngredient.ExpiryDate = ExpiryDatePicker.Date.DateTime;

            if (CategoryComboBox.SelectedItem is Category selectedCategory)
            {
                ViewModel.CurrentIngredient.CategoryID = selectedCategory.CategoryID;
            }

            bool success = ViewModel.SaveIngredient();

            if (success)
            {
                ViewModel.LoadIngredient();
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is string itemName && !string.IsNullOrWhiteSpace(itemName))
            {
                ViewModel.SearchText = itemName;
            }
        }
    }
}
