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
    public sealed partial class DeleteIngredientPage : Page
    {
        public InventoryViewModel ViewModel { get; set; }

        public DeleteIngredientPage()
        {
            this.InitializeComponent();
            ViewModel = new InventoryViewModel();
            this.DataContext = ViewModel;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteIngredient();

            // Quay lại trang AllIngredients sau khi xóa xong
            Frame.Navigate(typeof(AllIngredientPage));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Hủy thao tác và quay về trang danh sách
            Frame.Navigate(typeof(AllIngredientPage));
        }
    }
}
