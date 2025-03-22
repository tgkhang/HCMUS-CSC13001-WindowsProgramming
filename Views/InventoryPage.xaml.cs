using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.UI.Xaml.Navigation;
using POS_For_Small_Shop.ViewModels;
using Microsoft.UI.Xaml.Controls.Primitives;
using POS_For_Small_Shop.Views.Inventory;


namespace POS_For_Small_Shop.Views
{
    public sealed partial class InventoryPage : Page
    {
        public InventoryViewModel ViewModel { get; set; }

        public InventoryPage()
        {
            this.InitializeComponent();

            // Khởi tạo ViewModel và gán vào DataContext
            ViewModel = new InventoryViewModel();
            this.DataContext = ViewModel;

            // 🚀 Load dữ liệu khi mở trang
            ViewModel.LoadIngredients();
        }

        // 👉 Xử lý thêm nguyên liệu
        private async void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AddIngredient();
                await ShowMessage("Ingredient added successfully! ✅");
            }
            catch (Exception ex)
            {
                await ShowMessage($"Failed to add ingredient: {ex.Message}");
            }
        }

        // 👉 Xử lý chỉnh sửa nguyên liệu
        private async void EditIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedIngredient != null)
            {
                try
                {
                    ViewModel.EditIngredient();
                    await ShowMessage("Ingredient updated successfully! ✅");
                }
                catch (Exception ex)
                {
                    await ShowMessage($"Failed to update ingredient: {ex.Message}");
                }
            }
            else
            {
                await ShowMessage("Please select an ingredient to edit. 🚨");
            }
        }

        // 👉 Xử lý xóa nguyên liệu
        private async void DeleteIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedIngredient != null)
            {
                try
                {
                    ViewModel.DeleteIngredient();
                    await ShowMessage("Ingredient deleted successfully! ✅");
                }
                catch (Exception ex)
                {
                    await ShowMessage($"Failed to delete ingredient: {ex.Message}");
                }
            }
            else
            {
                await ShowMessage("Please select an ingredient to delete. 🚨");
            }
        }

        // 👉 Hiển thị thông báo
        private async System.Threading.Tasks.Task ShowMessage(string message)
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

