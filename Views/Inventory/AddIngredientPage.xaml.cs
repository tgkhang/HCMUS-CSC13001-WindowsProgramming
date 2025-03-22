using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
            ViewModel.AddIngredient();
        }
    }
}


