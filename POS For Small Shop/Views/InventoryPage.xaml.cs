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
using Windows.Foundation;
using Windows.Foundation.Collections;
using POS_For_Small_Shop.Views.Inventory;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InventoryPage : Page
    {
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
            Container.Navigate(typeof(AllIngredientPage));
        }

        private void Navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked) return;

            var item = (NavigationViewItem)sender.SelectedItem;
            if (item?.Tag is string tag && _pageMappings.ContainsKey(tag))
            {
                Container.Navigate(_pageMappings[tag]);
            }
        }
    }
}
