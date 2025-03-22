using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using POS_For_Small_Shop.ViewModels;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Views.Inventory
{
    public sealed partial class DeleteIngredientPage : Page
    {
        public InventoryViewModel ViewModel { get; set; }

        public DeleteIngredientPage()
        {
            this.InitializeComponent();
            ViewModel = new InventoryViewModel();
            this.DataContext = ViewModel;
        }

        private void DeleteIngredient_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteIngredient();
        }
    }
}
