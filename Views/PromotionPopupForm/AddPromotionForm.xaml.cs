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
using POS_For_Small_Shop.Data.Models;
using System.Diagnostics;
using System.Collections.ObjectModel;
using POS_For_Small_Shop.Views;
using System.Windows.Input;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.PromotionPopupForm
{
    public sealed partial class AddPromotionForm : UserControl
    {
        public PromotionManagementViewModel ViewModel { get; set; }

        public DiscountType[] discountTypeValues => Enum.GetValues<DiscountType>();

        public Action? CloseRequested;

        public AddPromotionForm()
        {
            this.InitializeComponent();
        }


        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            ItemGridView.SelectAll();
        }

        private void UnselectAllButton_Click(object sender, RoutedEventArgs e)
        {
            ItemGridView.SelectedItems.Clear();
        }

        private void SaveButton_CLick(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("");
            //Debug.WriteLine($"Promotion Name: {ViewModel.NewPromotion.PromoName}");
            //Debug.WriteLine($"Promotion Discount Type: {ViewModel.NewPromotion.Details.DiscountType}");
            //Debug.WriteLine($"Promotion Discount Value: {ViewModel.NewPromotion.Details.DiscountValue}");
            //Debug.WriteLine($"Promotion Description: {ViewModel.NewPromotion.Details.Description}");
            //Debug.WriteLine($"Promotion Items: {ViewModel.SelectedItems.Count}");
            //foreach (var item in ViewModel.SelectedItems)
            //{
            //    Debug.WriteLine($"Item: {item.Name}");
            //}
            //Debug.WriteLine($"Promotion StartDate: {ViewModel.NewPromotion.StartDate.ToString()}");
            //Debug.WriteLine($"Promotion EndDate: {ViewModel.NewPromotion.EndDate.ToString()}");

            ViewModel.NewPromotion.ItemIDs = ViewModel.SelectedItems.Select(x => x.MenuItemID).ToList();

            ViewModel.AddPromotion();
            ClosePopUp();
            ClearAddPromotionForm();

        }

        private void ClearAddPromotionForm()
        {
            PromotionName.Text = "";
            DiscountType.SelectedIndex = 0;
            DiscountValue.Text = "";
            Description.Text = "";
            StartDate.Date = null;
            EndDate.Date = null;
            ItemGridView.SelectedItems.Clear();
        }

        private void AssignSelectedItem(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedItems = new ObservableCollection<MenuItem>(ItemGridView.SelectedItems.Cast<MenuItem>());
        }


        private void CancelButton_CLick(object sender, RoutedEventArgs e)
        {
            ClosePopUp();
            ClearAddPromotionForm();
        }

        private void ClosePopUp()
        {
            CloseRequested?.Invoke();
        }

    }
}
