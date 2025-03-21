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
    public sealed partial class AddIngredientForm : UserControl
    {
        public InventoryViewModel ViewModel { get; set; }

        public AddIngredientForm()
        {
            this.InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddIngredient();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}

