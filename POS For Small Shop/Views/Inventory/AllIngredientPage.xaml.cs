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
