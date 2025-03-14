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
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void OpenShiftButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OpenShiftPage));
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReceiptButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PromotionButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PromotionManagementPage));
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
