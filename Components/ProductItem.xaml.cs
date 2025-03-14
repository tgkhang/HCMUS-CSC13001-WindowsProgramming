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
using Microsoft.UI.Xaml.Media.Imaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Components
{
    public sealed partial class ProductItem : UserControl
    {
        public ProductItem()
        {
            this.InitializeComponent();
        }

        // Product Name Property
        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register(nameof(Name), typeof(string),
                typeof(ProductItem), new PropertyMetadata("Product Name", OnNameChanged));

        private static void OnNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProductItem control)
                control.ProductName.Text = e.NewValue.ToString();
        }

        // Price Property
        public string Price
        {
            get => (string)GetValue(PriceProperty);
            set => SetValue(PriceProperty, value);
        }
        public static readonly DependencyProperty PriceProperty =
            DependencyProperty.Register(nameof(Price), typeof(string),
                typeof(ProductItem), new PropertyMetadata("$0.00", OnPriceChanged));

        private static void OnPriceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProductItem control)
                control.ProductPrice.Text = e.NewValue.ToString();
        }

        // Sale Price Property
        public string Sale
        {
            get => (string)GetValue(SaleProperty);
            set => SetValue(SaleProperty, value);
        }
        public static readonly DependencyProperty SaleProperty =
            DependencyProperty.Register(nameof(Sale), typeof(string),
                typeof(ProductItem), new PropertyMetadata("", OnSaleChanged));

        private static void OnSaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProductItem control)
            {
                string saleValue = e.NewValue.ToString();
                if (!string.IsNullOrEmpty(saleValue))
                {
                    control.SalePrice.Text = saleValue;
                    control.SalePrice.Visibility = Visibility.Visible;
                }
                else
                {
                    control.SalePrice.Visibility = Visibility.Collapsed;
                }
            }
        }

        // Image Source Property
        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(string),
                typeof(ProductItem), new PropertyMetadata("", OnImageChanged));

        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProductItem control)
                control.ProductImage.Source = new BitmapImage(new System.Uri(e.NewValue.ToString()));
        }
    }
}
