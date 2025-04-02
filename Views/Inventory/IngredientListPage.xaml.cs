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

namespace POS_For_Small_Shop.Views.Inventory
{
    public sealed partial class IngredientListPage : Page
    {
        private IDao _dao;
        private List<Ingredient> _allIngredients;
        private Ingredient _currentIngredient;
        private bool _isEditMode = false;
        private string _searchText = "";

        public ObservableCollection<Ingredient> FilteredIngredients { get; private set; } = new ObservableCollection<Ingredient>();

        public IngredientListPage()
        {
            this.InitializeComponent();
            _dao = Service.GetKeyedSingleton<IDao>();
            LoadIngredients();
            IngredientListView.ItemsSource = FilteredIngredients;
        }

        private void LoadIngredients()
        {
            try
            {
                _allIngredients = _dao.Ingredients.GetAll();
                ApplyFilters();
                UpdateEmptyState();
            }
            catch (NotImplementedException)
            {
                _allIngredients = new List<Ingredient>();
                ApplyFilters();
                UpdateEmptyState();
            }
        }

        private void ApplyFilters()
        {
            var filteredItems = _allIngredients;
            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                filteredItems = filteredItems.Where(item =>
                    item.IngredientName.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            FilteredIngredients.Clear();
            foreach (var item in filteredItems)
            {
                FilteredIngredients.Add(item);
            }
        }

        private void UpdateEmptyState()
        {
            EmptyStateText.Visibility = FilteredIngredients.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
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

        private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            _isEditMode = false;
            _currentIngredient = new Ingredient();
            FormHeaderText.Text = "Add Ingredient";
            IngredientDetailsPanel.Visibility = Visibility.Visible;
        }

        private void IngredientListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle selection change if necessary (not currently used)
        }
        private void EditIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int ingredientId)
            {
                _currentIngredient = _allIngredients.FirstOrDefault(item => item.IngredientID == ingredientId);
                if (_currentIngredient != null)
                {
                    IngredientIDTextBox.Text = _currentIngredient.IngredientID.ToString();
                    IngredientNameTextBox.Text = _currentIngredient.IngredientName;
                    CategoryIDTextBox.Text = _currentIngredient.CategoryID.ToString();
                    StockTextBox.Text = _currentIngredient.Stock.ToString();
                    UnitTextBox.Text = _currentIngredient.Unit;
                    SupplierTextBox.Text = _currentIngredient.Supplier;
                    PurchasePriceTextBox.Text = _currentIngredient.PurchasePrice.ToString();
                    ExpiryDatePicker.Date = _currentIngredient.ExpiryDate ?? DateTimeOffset.Now;
                    FormHeaderText.Text = "Edit Ingredient";
                    _isEditMode = true;
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

            _currentIngredient.IngredientID = int.Parse(IngredientIDTextBox.Text);
            _currentIngredient.IngredientName = IngredientNameTextBox.Text;
            _currentIngredient.Stock = int.Parse(StockTextBox.Text);
            _currentIngredient.Unit = UnitTextBox.Text;
            _currentIngredient.Supplier = SupplierTextBox.Text;
            _currentIngredient.CategoryID = int.Parse(CategoryIDTextBox.Text);
            _currentIngredient.PurchasePrice = float.Parse(PurchasePriceTextBox.Text);
            _currentIngredient.ExpiryDate = ExpiryDatePicker.Date.DateTime;

            bool success;
            if (_isEditMode)
            {
                success = _dao.Ingredients.Update(_currentIngredient.IngredientID, _currentIngredient);
            }
            else
            {
                success = _dao.Ingredients.Insert(_currentIngredient);
                if (success)
                {
                    _currentIngredient.IngredientID = _allIngredients.Count > 0 ? _allIngredients.Max(item => item.IngredientID) + 1 : 1;
                }
                _allIngredients.Add(_currentIngredient);
            }

            if (success)
            {
                ApplyFilters();
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
                var itemToDelete = _allIngredients.FirstOrDefault(item => item.IngredientID == ingredientId);
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
                        _dao.Ingredients.Delete(ingredientId);
                        _allIngredients.Remove(itemToDelete);
                        ApplyFilters();
                        UpdateEmptyState();
                    }
                }
            }
        }
    }
}
