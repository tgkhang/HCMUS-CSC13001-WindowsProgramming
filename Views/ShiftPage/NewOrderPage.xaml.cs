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
    public sealed partial class NewOrderPage : Page
    {
        public NewOrderPage()
        {
            this.InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {

            var products = new List<Product>
            {
                new Product { Name = "Product 1", Price = "$10.00", Sale = "$8.00", ImageSource = "https://via.placeholder.com/100" },
                new Product { Name = "Product 2", Price = "$15.00", Sale = "", ImageSource = "https://via.placeholder.com/100" },
                new Product { Name = "Product 3", Price = "$20.00", Sale = "$18.00", ImageSource = "https://via.placeholder.com/100" },
                new Product { Name = "Product 1", Price = "$10.00", Sale = "$8.00", ImageSource = "https://via.placeholder.com/100" },
                new Product { Name = "Product 2", Price = "$15.00", Sale = "", ImageSource = "https://via.placeholder.com/100" },
                new Product { Name = "Product 3", Price = "$20.00", Sale = "$18.00", ImageSource = "https://via.placeholder.com/100" },
                new Product { Name = "Product 1", Price = "$10.00", Sale = "$8.00", ImageSource = "https://via.placeholder.com/100" },
                new Product { Name = "Product 2", Price = "$15.00", Sale = "", ImageSource = "https://via.placeholder.com/100" },
                new Product { Name = "Product 3", Price = "$20.00", Sale = "$18.00", ImageSource = "https://via.placeholder.com/100" },
                new Product { Name = "Product 1", Price = "$10.00", Sale = "$8.00", ImageSource = "https://via.placeholder.com/100" },
                new Product { Name = "Product 2", Price = "$15.00", Sale = "", ImageSource = "https://via.placeholder.com/100" },
                new Product { Name = "Product 3", Price = "$20.00", Sale = "$18.00", ImageSource = "https://via.placeholder.com/100" },
            };

            // Bind the list to the ItemsControl
            ProductItemsControl.ItemsSource = products;
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Sale { get; set; }
        public string ImageSource { get; set; }
    }
}
