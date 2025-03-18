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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.PromotionPopupForm
{
    public sealed partial class UpdatePromotionForm : UserControl
    {

        public PromotionManagementViewModel ViewModel { get; set; }
        public Action? CloseRequested;


        public UpdatePromotionForm()
        {
            this.InitializeComponent();
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
            // Logic to close or reset the form
        }

        private void AssignSelectedItem(object sender, SelectionChangedEventArgs e)
        {
            // Update ViewModel.SelectedPromotion.Items if needed
        }
    }
}
