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

        public PromotionManagementPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
            addPromotionForm.ViewModel = ViewModel;
            addPromotionForm.CloseRequested += CloseAddFormPopup;
        }


        public void AddPromotionButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustPopupPosition();

            // Show the popup
            addPromotionPopup.IsOpen = true;

        }

        public void AdjustPopupPosition()
        {
            double windowHeight = this.ActualHeight;
            double popupHeight = windowHeight - 50;
            addPromotionFormContainer.Height = popupHeight;
            addPromotionPopup.HorizontalOffset = 0;
            addPromotionPopup.VerticalOffset = (windowHeight - popupHeight) / 2;
        }

        public void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustPopupPosition();
        }

        public void CloseAddFormPopup()
        {
            addPromotionPopup.IsOpen = false;
        }


    }
}
