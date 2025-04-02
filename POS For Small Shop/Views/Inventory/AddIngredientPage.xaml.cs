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
using POS_For_Small_Shop.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.Inventory
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
