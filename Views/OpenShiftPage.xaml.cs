using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.ComponentModel;
using POS_For_Small_Shop.Views.ShiftPage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OpenShiftPage : Page
    {
        
        private readonly Dictionary<string, Type> pageMappings = new()
        {
            { "NewOrderPage", typeof(NewOrderPage) },
            { "ShiftCustomerPage", typeof(ShiftCustomerPage) },
            { "ShiftOrderHistoryPage", typeof(ShiftOrderHistoryPage) },
            {"TestPage", typeof(TestPage) },
        };

        public OpenShiftPage()
        {
            this.InitializeComponent();
        }

        private void Navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked) return;

            var item = (NavigationViewItem)sender.SelectedItem;
            if (item?.Tag is string tag && pageMappings.ContainsKey(tag))
            {
                Container.Navigate(pageMappings[tag]);
            }
        }

    }
}
