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
            AddPromotionRequested?.Invoke();
            ClearAddPromotionForm();
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
