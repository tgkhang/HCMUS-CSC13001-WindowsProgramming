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
using Microsoft.UI;
using System.Diagnostics;
using POS_For_Small_Shop.Views.PromotionPopupForm;
using POS_For_Small_Shop.Data.Models;
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
            addPromotionForm.ViewModel = ViewModel;
            viewPromotionDetailsForm.ViewModel = ViewModel;
            //updatePromotionForm.ViewModel = ViewModel;

            // Set up event handlers



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
            if(updatePromotionPopup.IsOpen)
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
            updatePromotionPopup.IsOpen = false;
        }

        public void AddPromotionButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustPopupPosition(addPromotionPopup, addPromotionFormContainer);
            addPromotionPopup.IsOpen = true;
            addPromotionForm.DataContext = ViewModel;
        }

        public async void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
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
                updatePromotionForm.DataContext = ViewModel;
                updatePromotionPopup.IsOpen = true;

                // Adjust position and show popup
                AdjustPopupPosition(updatePromotionPopup, updatePromotionFormContainer);
                
            }
        }




    }
}
