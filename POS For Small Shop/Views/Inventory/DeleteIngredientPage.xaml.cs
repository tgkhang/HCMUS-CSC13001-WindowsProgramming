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
            if (ViewModel.SelectedIngredient != null)
            {
                ViewModel.DeleteIngredient(ViewModel.SelectedIngredient);
            }
        }
    }
}
