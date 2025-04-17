using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Printing;
using POS_For_Small_Shop.ViewModels.ShiftPage;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using POS_For_Small_Shop.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.ShiftPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PaymentPage : Page
    {
        public PaymentViewModel ViewModel { get; private set; }
        private bool _isPaid = false;
        private PrintManager _printManager;
        private PrintDocument _printDocument;
        private IPrintDocumentSource _printDocumentSource;
        private IQRService _qrService;

        public PaymentPage()
        {
            this.InitializeComponent();
            _qrService= Service.GetKeyedSingleton<IQRService>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is NewOrderViewModel orderViewModel)
            {
                // Initialize view model from order view model
                ViewModel = new PaymentViewModel(orderViewModel);
                DataContext = ViewModel;
            }
        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get a file save picker
                var savePicker = new FileSavePicker();

                // Initialize the file picker with the window handle
                var window = GetWindow();
                if (window != null)
                {
                    InitializeWithWindow.Initialize(savePicker, window);
                }

                // Set properties for the save picker
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("PDF Document", new[] { ".pdf" });
                savePicker.SuggestedFileName = $"Receipt_{ViewModel.OrderNumber}";

                // Get the file to save
                StorageFile file = await savePicker.PickSaveFileAsync();

                if (file != null)
                {
                    // Show a message indicating the file is being saved
                    ContentDialog savingDialog = new ContentDialog
                    {
                        Title = "Saving Receipt",
                        Content = "Your receipt is being saved...",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };

                    // Show the dialog
                    await savingDialog.ShowAsync();

                    // TODO: Implement actual PDF generation
                    // For now, we'll just show a success dialog

                    ContentDialog successDialog = new ContentDialog
                    {
                        Title = "Receipt Saved",
                        Content = $"Your receipt has been saved to {file.Path}",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };

                    await successDialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                // Show error message
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"An error occurred: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await errorDialog.ShowAsync();
            }
        }

        private IntPtr GetWindow()
        {
            var window = App.MainWindow;
            if (window != null)
            {
                var hwnd = WindowNative.GetWindowHandle(window);
                return hwnd;
            }

            return IntPtr.Zero;
        }

        private async void CashPayment_Click(object sender, RoutedEventArgs e)
        {
            if (_isPaid)
            {
                await ShowMessage("Payment Completed", "This order has already been paid.");
                return;
            }

            ContentDialog confirmDialog = new ContentDialog
            {
                Title = "Confirm Cash Payment",
                Content = $"Confirm payment of {ViewModel.Total} by cash?",
                PrimaryButtonText = "Confirm",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var result = await confirmDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                bool success = ViewModel.ProcessCashPayment();

                if (success)
                {
                    _isPaid = true;
                    await ShowMessage("Payment Successful", "Cash payment has been processed successfully.");

                    // Navigate back to the order page
                    Frame.Navigate(typeof(NewOrderPage));
                }
                else
                {
                    await ShowMessage("Payment Failed", "There was an error processing the cash payment. Please try again.");
                }
            }
        }

        private async void QRCodePayment_Click(object sender, RoutedEventArgs e)
        {
            if (_isPaid)
            {
                await ShowMessage("Payment Completed", "This order has already been paid.");
                return;
            }

            try
            {
                QRCodeLoadingRing.IsActive = true;
                QRCodeImage.Source = null; // Clear previous image 
                QROrderInfoText.Text = string.Empty;
                QRCodePanel.Visibility = Visibility.Visible;

                string orderInfo = $"Order #{ViewModel.OrderNumber}";

                // Generate QR code using QRService
                var qrCodeImage = await _qrService.GenerateQRCodeImageAsync((decimal)ViewModel.Total, orderInfo);

                if (qrCodeImage != null)
                {
                    // Set the QR code image to the Image control
                    QRCodeImage.Source = qrCodeImage;
                    //QRAmountText.Text = $"Amount: {ViewModel.Total:C}"; //default currency formatter
                    QROrderInfoText.Text = $"Order: #{ViewModel.OrderNumber}";
                }
                else
                {
                    await ShowMessage("QR Code Error", "Failed to generate QR code. Please try again or use a different payment method.");
                    QRCodePanel.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                await ShowMessage("Error", $"An error occurred: {ex.Message}");
                QRCodePanel.Visibility = Visibility.Collapsed;
            }
            finally
            {
                QRCodeLoadingRing.IsActive = false;
            }
        }
        private async void PaymentComplete_Click(object sender, RoutedEventArgs e)
        {
            bool success = ViewModel.ProcessQRCodePayment();

            if (success)
            {
                _isPaid = true;
                QRCodePanel.Visibility = Visibility.Collapsed;
                await ShowMessage("Payment Successful", "QR code payment has been processed successfully.");
                Frame.Navigate(typeof(NewOrderPage));
            }
            else
            {
                await ShowMessage("Payment Failed", "There was an error processing the QR code payment. Please try again.");
            }
        }

        private async void CancelQRCode_Click(object sender, RoutedEventArgs e)
        {
            // Hide QR code panel
            QRCodePanel.Visibility = Visibility.Collapsed;

            // Ask if the payment was completed
            ContentDialog confirmDialog = new ContentDialog
            {
                Title = "QR Code Payment",
                Content = "Was the payment completed successfully?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var result = await confirmDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                bool success = ViewModel.ProcessQRCodePayment();

                if (success)
                {
                    _isPaid = true;
                    await ShowMessage("Payment Successful", "QR code payment has been processed successfully.");

                    // Navigate back to the order page
                    Frame.Navigate(typeof(NewOrderPage));
                }
                else
                {
                    await ShowMessage("Payment Failed", "There was an error processing the QR code payment. Please try again.");
                }
            }
        }

        private async Task ShowMessage(string title, string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
