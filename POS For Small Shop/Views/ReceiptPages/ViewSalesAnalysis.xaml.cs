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
using POS_For_Small_Shop.ViewModels;
using System.Diagnostics;
using Newtonsoft.Json.Bson;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.ReceiptPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewSalesAnalysis : Page
    {

        public SalesAnalysisViewModel ViewModel { get; set; } = new(); // Storage the view model of the page

        public ViewSalesAnalysis()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel; // Set the data context of the page to the view model
        }

        public void SalesFilterChart_ButtonClick(object sender, RoutedEventArgs e)
        {
            // Ensure sender is a Button and has a valid Tag
            if (sender is Button button && button.Tag is string filterType)
            {
                ViewModel.LoadSalesChartData(filterType);
            }
            else
            {
                Debug.WriteLine("Error: Invalid button or missing tag.");
            }
        }

        public void OrdersFilterChart_ButtonClick(Object sender, RoutedEventArgs e)
        {
            // Ensure sender is a Button and has a valid Tag
            if (sender is Button button && button.Tag is string filterType)
            {
                ViewModel.LoadOrdersChartData(filterType);
            }
            else
            {
                Debug.WriteLine("Error: Invalid button or missing tag.");
            }
        }
    }
}
