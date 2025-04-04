using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using POS_For_Small_Shop.ViewModels;
using POS_For_Small_Shop.Data.Models;
using Microsoft.UI.Xaml.Navigation;

namespace POS_For_Small_Shop.Views.Inventory
{
    public sealed partial class ReadIngredientPage : Page
    {
        public InventoryViewModel ViewModel { get; set; } = new InventoryViewModel();

        public ReadIngredientPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
    }
}
