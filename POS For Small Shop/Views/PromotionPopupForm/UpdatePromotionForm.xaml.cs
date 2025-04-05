using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public sealed partial class UpdatePromotionForm : UserControl
    {
        public ObservableCollection<MenuItem> TempSelectedItems { get; set; } = new ObservableCollection<MenuItem>();
        public Action? CloseRequested;
        public Action? UpdateRequested;
        public Array discountTypeValues => Enum.GetValues(typeof(DiscountType));

        public UpdatePromotionForm()
        {
            this.InitializeComponent();
            this.Loaded += UpdateGridViewLoaded;
        }

        public void UpdateGridViewLoaded(object sender, RoutedEventArgs e)
        {
            UpdateItemGridView.DispatcherQueue.TryEnqueue(() =>
            {
                UpdateItemGridView.SelectedItems.Clear();
                foreach (var item in TempSelectedItems)
                {
                    UpdateItemGridView.SelectedItems.Add(item);
                }
            });
        }



        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemGridView.SelectAll();
        }

        private void UnselectAllButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemGridView.SelectedItems.Clear();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateRequested?.Invoke();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseRequested?.Invoke();
        }

        private void AssignSelectedItem(object sender, SelectionChangedEventArgs e)
        {
            TempSelectedItems = new ObservableCollection<MenuItem>(UpdateItemGridView.SelectedItems.Cast<MenuItem>());
        }
    }
}
