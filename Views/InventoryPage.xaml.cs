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
        public InventoryViewModel ViewModel { get; set; } = new InventoryViewModel();
        //private InventoryViewModel _viewModel;

        public InventoryPage()
        {
            this.InitializeComponent();
            ViewModel = new InventoryViewModel(); // Khởi tạo một lần duy nhất
            this.DataContext = ViewModel;

            // 🚀 Load dữ liệu khi mở trang
            ViewModel.LoadIngredients();
        }

        // 👉 Xử lý khi chọn mục trong NavigationView
        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem item)
            {
                switch (item.Tag)
                {
                    case "AllIngredients":
                        // Tải lại danh sách nguyên liệu
                        ViewModel.LoadIngredients();
                        break;
                    case "AddIngredient":
                        OpenAddIngredientForm();
                        break;
                    case "EditIngredient":
                        OpenEditIngredientForm();
                        break;
                    case "DeleteIngredient":
                        OpenDeleteIngredientForm();
                        break;
                }
            }
        }

        // 👉 Xử lý khi nhấn vào CommandBar
        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            OpenAddIngredientForm();
        }

        private void EditIngredient_Click(object sender, RoutedEventArgs e)
        {
            OpenEditIngredientForm();
        }

        private void DeleteIngredient_Click(object sender, RoutedEventArgs e)
        {
            OpenDeleteIngredientForm();
        }

        // 👉 Xử lý khi mở Popup Thêm nguyên liệu
        private void OpenAddIngredientForm()
        {
            AdjustPosition(addIngredient, addIngredientForm);
            addIngredient.IsOpen = true;
        }

        // 👉 Xử lý khi mở Popup Sửa nguyên liệu
        private void OpenEditIngredientForm()
        {
            if (IngredientListView.SelectedItem is Ingredient selected)
            {
                ViewModel.SelectedIngredient = selected;
                AdjustPosition(editIngredient, editIngredientForm);
                editIngredient.IsOpen = true;
            }
            else
            {
                ShowMessage("Please select an ingredient to edit.");
            }
        }

        // 👉 Xử lý khi mở Popup Xóa nguyên liệu
        private void OpenDeleteIngredientForm()
        {
            if (IngredientListView.SelectedItem is Ingredient selected)
            {
                ViewModel.SelectedIngredient = selected;
                AdjustPosition(deleteIngredient, deleteIngredientForm);
                deleteIngredient.IsOpen = true;
            }
            else
            {
                ShowMessage("Please select an ingredient to delete.");
            }
        }

        // 👉 Đóng Popup
        private void CloseAddIngredientForm()
        {
            addIngredient.IsOpen = false;
        }

        private void CloseEditIngredientForm()
        {
            editIngredient.IsOpen = false;
        }

        private void CloseDeleteIngredientForm()
        {
            deleteIngredient.IsOpen = false;
        }

        // 👉 Căn chỉnh vị trí Popup theo kích thước màn hình
        private void AdjustPosition(Popup popup, FrameworkElement container)
        {
            double windowHeight = this.ActualHeight;
            double popupHeight = container.ActualHeight;

            popup.HorizontalOffset = (this.ActualWidth - container.ActualWidth) / 2;
            popup.VerticalOffset = (windowHeight - popupHeight) / 2;
        }

        // 👉 Xử lý khi thay đổi kích thước trang
        private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (addIngredient.IsOpen)
            {
                AdjustPosition(addIngredient, addIngredientForm);
            }
            if (editIngredient.IsOpen)
            {
                AdjustPosition(editIngredient, editIngredientForm);
            }
            if (deleteIngredient.IsOpen)
            {
                AdjustPosition(deleteIngredient, deleteIngredientForm);
            }
        }

        // 👉 Hiển thị thông báo
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

