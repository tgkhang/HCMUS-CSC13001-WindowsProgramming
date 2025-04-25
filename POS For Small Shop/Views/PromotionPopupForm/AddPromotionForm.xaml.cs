using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using POS_For_Small_Shop.Data.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.PromotionPopupForm
{
    public sealed partial class AddPromotionForm : UserControl
    {
        public ObservableCollection<MenuItem> TempSelectedItems { get; set; } = new ObservableCollection<MenuItem>();

        public DiscountType[] discountTypeValues => Enum.GetValues<DiscountType>();

        public Action? CloseRequested;
        public Action? AddPromotionRequested;

        public AddPromotionForm()
        {
            this.InitializeComponent();
            this.Loaded += (sender, e) =>
            {
                StartDate.Date = null;
                EndDate.Date = null;
            };
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
            if (!ValidatePromotionInput())
                return;

            AddPromotionRequested?.Invoke();
            ClearAddPromotionForm();
        }

        private bool ValidatePromotionInput()
        {
            string promoName = PromotionName.Text?.Trim();
            string discountValueText = DiscountValue.Text?.Trim();
            var selectedItems = ItemGridView.SelectedItems;

            if (string.IsNullOrWhiteSpace(promoName))
            {
                ShowError("Promotion name is required.");
                return false;
            }

            if (DiscountType.SelectedItem == null)
            {
                ShowError("Please select a discount type.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(discountValueText) ||
                !float.TryParse(discountValueText, out float discountValue) || discountValue <= 0)
            {
                ShowError("Enter a valid positive discount value.");
                return false;
            }

            if (StartDate.Date == null || EndDate.Date == null)
            {
                ShowError("Start and end dates are required.");
                return false;
            }

            if (StartDate.Date > EndDate.Date)
            {
                ShowError("Start date cannot be after end date.");
                return false;
            }

            if (selectedItems == null || selectedItems.Count == 0)
            {
                ShowError("Please select at least one item.");
                return false;
            }

            // All good
            return true;
        }

        private void ShowError(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Validation Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot // 
            };

            _ = dialog.ShowAsync(); // Fire-and-forget
        }



        private void ClearAddPromotionForm()
        {
            PromotionName.Text = String.Empty;
            DiscountType.SelectedIndex = 0;
            DiscountValue.Text = String.Empty;
            Description.Text = String.Empty;
            StartDate.Date = null;
            EndDate.Date = null;
            ItemGridView.SelectedItems.Clear();
        }

        private void AssignSelectedItem(object sender, SelectionChangedEventArgs e)
        {
            TempSelectedItems = new ObservableCollection<MenuItem>(ItemGridView.SelectedItems.Cast<MenuItem>());
        }


        private void CancelButton_CLick(object sender, RoutedEventArgs e)
        {
            ClearAddPromotionForm();
            ClosePopUp();
        }

        private void ClosePopUp()
        {
            CloseRequested?.Invoke();
        }

    }
}
