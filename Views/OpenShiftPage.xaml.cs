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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OpenShiftPage : Page
    {
        public OpenShiftPage()
        {
            this.InitializeComponent();
        }

        private void navi_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                string tag = args.SelectedItemContainer.Tag.ToString();
                NavigateToPage(tag);
            }
        }

        private void NavigateToPage(string pageTag)
        {
            switch (pageTag)
            {
                case "NewOrderPage":
                    ContentFrame.Navigate(typeof(NewOrderPage));
                    break;
                case "AddCustomerPage":
                    ContentFrame.Navigate(typeof(AddCustomerPage));
                    break;
                case "OrdersPage":
                    ContentFrame.Navigate(typeof(OrdersPage));
                    break;
                case "CloseShiftPage":
                   // ContentFrame.Navigate(typeof(CloseShiftPage));
                    break;
                case "AccountPage":
                   // ContentFrame.Navigate(typeof(AccountPage));
                    break;
                case "CartPage":
                    //ContentFrame.Navigate(typeof(CartPage));
                    break;
                case "HelpPage":
                 //   ContentFrame.Navigate(typeof(HelpPage));
                    break;
                default:
                    break;
            }
        }
    }
}
