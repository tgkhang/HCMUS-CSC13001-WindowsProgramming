using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POS_For_Small_Shop.ViewModels;
using POS_For_Small_Shop.Data.Models;


namespace POS_For_Small_Shop.Views.Inventory
{
    public sealed partial class AllIngredientPage : Page
    {
        public AllIngredientPage()
        {
            this.InitializeComponent();
            this.DataContext = new InventoryViewModel();
        }

        private void IngredientList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Ingredient selectedIngredient)
            {
                this.Frame.Navigate(typeof(ReadIngredientPage), selectedIngredient);
            }
        }
    }
}
