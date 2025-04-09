using System;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.ShiftPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CloseShiftPage : Page
    {
        private IDao _dao;
        private Shift _currentShift;
        private float _expectedCash;
        private float _actualCash;
        private float _cashDifference;
        private IShiftService _shiftService;

        public CloseShiftPage()
        {
            this.InitializeComponent();

            // Get the DAO from the service
            _dao = Service.GetKeyedSingleton<IDao>();
            _shiftService = Service.GetKeyedSingleton<IShiftService>();

            // Get the current shift
            LoadCurrentShift();
        }

        private void LoadCurrentShift()
        {
            _currentShift = _shiftService.CurrentShift;

            if (_currentShift == null)
            {
                // Handle the case where there is no current shift
                return;
            }

            _expectedCash = _currentShift.OpeningCash + _currentShift.TotalSales;
            _actualCash = _expectedCash; 
            _cashDifference = 0;
            UpdateUI();
        }

        private void UpdateUI()
        {
            var vietnameseCulture = new System.Globalization.CultureInfo("vi-VN");

            OpeningCashText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _currentShift.OpeningCash);
            ExpectedCashText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _expectedCash);
            ActualCashBox.Value = _actualCash;
            CashDifferenceText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _cashDifference);
            TotalSalesText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _currentShift.TotalSales);
            TotalOrdersText.Text = _currentShift.TotalOrders.ToString();
        }

        private void ActualCashBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            //_actualCash = (float)args.NewValue;
            //_cashDifference = _actualCash - _expectedCash;

            //var vietnameseCulture = new System.Globalization.CultureInfo("vi-VN");
            //CashDifferenceText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _cashDifference);

            //// Change text color based on difference
            //if (_cashDifference < 0)
            //{
            //    CashDifferenceText.Foreground = new SolidColorBrush(Colors.Red);
            //}
            //else if (_cashDifference > 0)
            //{
            //    CashDifferenceText.Foreground = new SolidColorBrush(Colors.Green);
            //}
            //else
            //{
            //    // Use the default text color from theme
            //    CashDifferenceText.Foreground = (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
            //}
        }

        private async void CloseShiftButton_Click(object sender, RoutedEventArgs e)
        {
            //// In a real implementation, you would close the shift in the database

            _currentShift.EndTime = DateTime.Now;
            _currentShift.ClosingCash = _actualCash;
            _currentShift.Status = "Closed";


            bool success = _dao.Shifts.Update(_currentShift.ShiftID,_currentShift);

            if (!success)
            {
                // Show error message
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Failed to close the shift. Please try again.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
                return;
            }

            // Show confirmation dialog
            ContentDialog dialog = new ContentDialog
            {
                Title = "Shift Closed",
                Content = "The shift has been closed successfully.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();


            var window = App.MainWindow;
            if (window != null && window.Content is Frame mainFrame)
            {
                mainFrame.Navigate(typeof(HomePage));
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //// Navigate back to the previous page
            //if (Frame.CanGoBack)
            //{
            //    Frame.GoBack();
            //}
        }
    }
}
