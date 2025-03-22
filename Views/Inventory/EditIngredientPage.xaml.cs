using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using POS_For_Small_Shop.ViewModels;

namespace POS_For_Small_Shop.Views.Inventory
{
    public sealed partial class EditIngredientPage : Page
    {
        public InventoryViewModel ViewModel { get; set; }

        public EditIngredientPage()
        {
            this.InitializeComponent();
            ViewModel = new InventoryViewModel();
            this.DataContext = ViewModel;
        }

        private void SaveIngredient_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveIngredient();
        }
    }
}