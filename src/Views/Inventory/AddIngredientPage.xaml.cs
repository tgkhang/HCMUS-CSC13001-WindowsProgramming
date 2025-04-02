using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.ViewModels;


namespace POS_For_Small_Shop.Views.Inventory
{
    public sealed partial class AddIngredientPage : Page
    {
        public InventoryViewModel ViewModel { get; set; }

        public AddIngredientPage()
        {
            this.InitializeComponent();
            ViewModel = new InventoryViewModel();
            this.DataContext = ViewModel;
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            var ingredient = new Ingredient
            {
                IngredientName = IngredientNameTextBox.Text,
                CategoryID = int.TryParse(CategoryIDTextBox.Text, out int categoryId) ? categoryId : 0,
                Stock = int.TryParse(StockTextBox.Text, out int stock) ? stock : 0,
                Unit = UnitTextBox.Text,
                PurchasePrice = float.TryParse(PurchasePriceTextBox.Text, out float price) ? price : 0f,
                Supplier = SupplierTextBox.Text,
                ExpiryDate = DateTime.TryParse(ExpiryDateTextBox.Text, out DateTime expiry) ? expiry : DateTime.MinValue
            };

            ViewModel.AddIngredient(ingredient);
        }
    }
}


