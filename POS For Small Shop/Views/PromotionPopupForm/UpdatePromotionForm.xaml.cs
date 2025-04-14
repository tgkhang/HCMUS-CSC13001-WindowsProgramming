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
            UpdateItemGridView.Loaded += (s, e) =>
            {
                UpdateGridViewLoaded();
            };
        }

        public void UpdateGridViewLoaded()
        {
                UpdateItemGridView.SelectedItems.Clear();
                var itemsSource = UpdateItemGridView.ItemsSource as IEnumerable<MenuItem>;
                if (itemsSource == null)
                {
                    Debug.WriteLine("ItemsSource is null or empty");
                    return;
                }

                foreach (var tempItem in TempSelectedItems)
                {
                    // Find matching item in ItemsSource by MenuItemID
                    var matchingItem = itemsSource.FirstOrDefault(i => i.MenuItemID == tempItem.MenuItemID);
                    if (matchingItem != null)
                    {
                        UpdateItemGridView.SelectedItems.Add(matchingItem);
                        // Scroll to ensure the item is rendered
                        UpdateItemGridView.ScrollIntoView(matchingItem);
                    }
                    else
                    {
                        Debug.WriteLine($"Item {tempItem.Name} not found in ItemsSource");
                    }
                }

                // Force visual refresh
                UpdateItemGridView.InvalidateArrange();
                UpdateItemGridView.InvalidateMeasure();
        }

        public void ClearGridView()
        {
            UpdateItemGridView.SelectedItems.Clear();
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
