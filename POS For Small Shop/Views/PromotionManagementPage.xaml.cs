using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PromotionManagementPage : Page
    {
        public PromotionManagementViewModel ViewModel { get; set; } = new PromotionManagementViewModel();
        public DiscountType[] discountTypeValues => Enum.GetValues<DiscountType>();
        public PromotionManagementPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
            viewPromotionDetailsForm.ViewModel = ViewModel;
            //updatePromotionForm.ViewModel = ViewModel;

            // Set up event handlers

            addPromotionForm.AddPromotionRequested += SaveNewPromotion;
            updatePromotionForm.UpdateRequested += UpdatePromotion;


            addPromotionForm.CloseRequested += CloseAddFormPopup;
            viewPromotionDetailsForm.CloseRequested += CloseViewFormPopup;
            updatePromotionForm.CloseRequested += CloseUpdateFormPopup;
        }




        public void AdjustPopupPosition(Popup popup, FrameworkElement container)
        {
            double windowHeight = this.ActualHeight;
            double popupHeight = windowHeight - 50;

            container.Height = popupHeight;


            popup.HorizontalOffset = 0;
            popup.VerticalOffset = (windowHeight - popupHeight) / 2;
        }

        public void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (addPromotionPopup.IsOpen)
            {
                AdjustPopupPosition(addPromotionPopup, addPromotionFormContainer);
            }
            if (viewPromotionDetailsPopup.IsOpen)
            {
                AdjustPopupPosition(viewPromotionDetailsPopup, viewPromotionDetailsFormContainer);
            }
            if (updatePromotionPopup.IsOpen)
            {
                AdjustPopupPosition(updatePromotionPopup, updatePromotionFormContainer);
            }
        }

        public void CloseAddFormPopup()
        {
            addPromotionPopup.IsOpen = false;
        }

        public void CloseViewFormPopup()
        {
            viewPromotionDetailsPopup.IsOpen = false;
        }

        public void CloseUpdateFormPopup()
        {
            updatePromotionForm.ClearGridView();
            updatePromotionPopup.IsOpen = false;
        }

        public void CLoseDeleteFormPopup()
        {
            deletePromotionPopup.IsOpen = false;
        }

        public void AddPromotionButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustPopupPosition(addPromotionPopup, addPromotionFormContainer);
            addPromotionPopup.IsOpen = true;
            addPromotionForm.DataContext = ViewModel;
        }

        public void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Promotion promotion)
            {

                viewPromotionDetailsPopup.IsOpen = true;
                // Set the promotion data
                ViewModel.SelectedPromotion = promotion;
                ViewModel.setSelectedItems(promotion.ItemIDs);

                //Debug.WriteLine($"Promotion Name: {promotion.PromoName}");
                //Debug.WriteLine($"Promotion Discount Type: {promotion.Details.DiscountType}");
                //Debug.WriteLine($"Promotion Discount Value: {promotion.Details.DiscountValue}");
                //Debug.WriteLine($"Promotion Description: {promotion.Details.Description}");
                //Debug.WriteLine($"Promotion Items: {promotion.ItemIDs.Count}");
                //foreach (var item in promotion.ItemIDs)
                //{
                //    Debug.WriteLine($"Item: {item}");
                //}

                // Adjust position and show popup
                AdjustPopupPosition(viewPromotionDetailsPopup, viewPromotionDetailsFormContainer);

            }
        }

        public void UpdatePromotionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Promotion promotion)
            {

                // Set the promotion data
                ViewModel.SelectedPromotion = promotion;
                ViewModel.setSelectedItems(promotion.ItemIDs);
                updatePromotionForm.TempSelectedItems = ViewModel.SelectedItems;
                updatePromotionForm.DataContext = ViewModel;

                updatePromotionPopup.IsOpen = true;

                // Adjust position and show popup
                AdjustPopupPosition(updatePromotionPopup, updatePromotionFormContainer);

            }
        }

        public void DeletePromotionButton_Click(Object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Promotion promotion)
            {
                ViewModel.SelectedPromotion = promotion;
                deletePromotionPopup.IsOpen = true;


            }
        }

        public void SaveNewPromotion()
        {
            ViewModel.SelectedItems = addPromotionForm.TempSelectedItems;
            ViewModel.AddPromotion();
            CloseAddFormPopup();
        }

        public void UpdatePromotion()
        {
            ViewModel.SelectedItems = updatePromotionForm.TempSelectedItems;
            ViewModel.UpdateSelectedPromotion();
            updatePromotionForm.ClearGridView();
            CloseUpdateFormPopup();
        }

        public void ConfirmDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteSelectedPromotion();
            deletePromotionPopup.IsOpen = false;
        }

        public void CancelDeletenButton_Click(object obj, RoutedEventArgs e)
        {
            CLoseDeleteFormPopup();
        }

        public void Back_To_HomePage(object sender, RoutedEventArgs e)
        {
            // Navigate back to the home page
            DashboardWindow.Instance.NavigateToPage(typeof(HomePage));
        }

        public void HoverBorder_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Grid grid)
            {
                grid.Background = new SolidColorBrush(Colors.LightGray);
            }
        }

        public void HoverBorder_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Grid grid)
            {
                grid.Background = new SolidColorBrush(Colors.White);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is string itemName && !string.IsNullOrWhiteSpace(itemName))
            {
                ViewModel.SearchQuery = itemName;
            }
        }
    }
}
