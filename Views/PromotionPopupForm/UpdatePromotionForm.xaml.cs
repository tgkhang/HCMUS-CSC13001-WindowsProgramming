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
using NetTopologySuite.Geometries;
using POS_For_Small_Shop.ViewModels;
using POS_For_Small_Shop.Data.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.PromotionPopupForm
{
    public sealed partial class UpdatePromotionForm : UserControl
    {

        public PromotionManagementViewModel ViewModel { get; set; }
        public Action? CloseRequested;
        public DiscountType[] discountTypeValues => Enum.GetValues<DiscountType>();

        public UpdatePromotionForm()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
            this.Loaded += UpdatePromotionForm_Loaded;
        }

        private void UpdatePromotionForm_Loaded(object sender, RoutedEventArgs e)
        {
            // Pre-select items when the form loads
            //if (this.DataContext?.SelectedItems != null && ViewModel?.AvaibleItems != null)
            //{
            //    foreach (var item in ViewModel.SelectedItems)
            //    {
            //        if (ViewModel.AvaibleItems.Contains(item))
            //        {
            //            ItemGridView.SelectedItems.Add(item);
            //        }
            //    }
            //}
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseRequested?.Invoke();
        }

        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            ItemGridView.SelectAll();
        }

        private void UnselectAllButton_Click(object sender, RoutedEventArgs e)
        {
            ItemGridView.SelectedItems.Clear();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic to save updated promotion
            // e.g., ViewModel.UpdatePromotion();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseButton_Click(sender, e);
        }

        private void AssignSelectedItem(object sender, SelectionChangedEventArgs e)
        {
            // Update ViewModel.SelectedPromotion.Items if needed
        }
    }
}
