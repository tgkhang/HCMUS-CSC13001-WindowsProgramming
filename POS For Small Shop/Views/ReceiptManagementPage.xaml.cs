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
using Windows.UI.ApplicationSettings;
using POS_For_Small_Shop.Views.ReceiptPages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReceiptManagementPage : Page
    {
        public ReceiptManagementPage()
        {
            this.InitializeComponent();
            // Set the initial selected item
            ReceiptNavigationView.Loaded += ReceiptNavigationViewOnLoaded;
        }


        private void ReceiptNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            string selectedTag = args.SelectedItemContainer.Tag.ToString();
            switch (selectedTag)
            {
                case "Receipt":
                    ContentFrame.Navigate(typeof(ViewReceipts));
                    break;
                case "SalesAnalysis":
                    ContentFrame.Navigate(typeof(ViewSalesAnalysis));
                    break;
                case "GoToHomePage":
                    Frame.Navigate(typeof(HomePage));
                    break;
            }

        }

        private void ReceiptNavigationViewOnLoaded(object sender, RoutedEventArgs e)
        {
            // Set the initial selected item
            var selectedItem = ReceiptNavigationView.MenuItems[0] as NavigationViewItem;
            if (selectedItem != null)
            {
                ReceiptNavigationView.SelectedItem = selectedItem;
                // Navigate to the corresponding page
                ContentFrame.Navigate(typeof(ViewReceipts));
            }
        }
    }
}
