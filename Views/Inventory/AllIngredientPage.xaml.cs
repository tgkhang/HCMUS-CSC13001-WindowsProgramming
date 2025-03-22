using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POS_For_Small_Shop.ViewModels;


namespace POS_For_Small_Shop.Views.Inventory
{
    public sealed partial class AllIngredientPage : Page
    {
        public InventoryViewModel ViewModel { get; set; }

        public AllIngredientPage()
        {
            this.InitializeComponent();
            ViewModel = new InventoryViewModel();
            this.DataContext = ViewModel;
            ViewModel.LoadIngredients();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddIngredientPage));
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedIngredient != null)
            {
                Frame.Navigate(typeof(EditIngredientPage), ViewModel.SelectedIngredient);
            }
            else
            {
                ShowMessage("Please select an ingredient to edit.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedIngredient != null)
            {
                ViewModel.DeleteIngredient();
            }
            else
            {
                ShowMessage("Please select an ingredient to delete.");
            }
        }

        private async void ShowMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Notification",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
