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
        // Ánh xạ các Tag trong NavigationView với các Page tương ứng
        private readonly Dictionary<string, Type> _pageMappings = new()
        {
            { "AllIngredientPage", typeof(AllIngredientPage) },
            { "AddIngredientPage", typeof(AddIngredientPage) },
            { "EditIngredientPage", typeof(EditIngredientPage) },
            { "DeleteIngredientPage", typeof(DeleteIngredientPage) },
        };

        public InventoryPage()
        {
            this.InitializeComponent();
            // Mở mặc định trang AllIngredientsPage khi khởi động
            Container.Navigate(typeof(AllIngredientPage));
        }

        // 👉 Xử lý sự kiện khi chọn mục trong NavigationView
        private void Navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked) return;

            // Lấy ra NavigationViewItem được chọn
            var item = (NavigationViewItem)sender.SelectedItem;
            if (item?.Tag is string tag && _pageMappings.ContainsKey(tag))
            {
                // Chuyển trang theo tag
                Container.Navigate(_pageMappings[tag]);
            }
        }
    }
}

