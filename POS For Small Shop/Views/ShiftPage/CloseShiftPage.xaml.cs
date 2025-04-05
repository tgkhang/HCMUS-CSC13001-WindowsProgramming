using System;
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

        public CloseShiftPage()
        {
            this.InitializeComponent();

            // Get the DAO from the service
            _dao = Service.GetKeyedSingleton<IDao>();

            // Get the current shift
            LoadCurrentShift();
        }

        private void LoadCurrentShift()
        {
            //// In a real implementation, you would get the current open shift from the database

            //// For now, we'll create a mock shift
            //_currentShift = new Shift
            //{
            //    ShiftID = 1001,
            //    StartTime = DateTime.Now.AddHours(-8), // Started 8 hours ago
            //    OpeningCash = 500000, // 500,000 VND
            //    TotalSales = 0,
            //    TotalOrders = 0,
            //    Status = "Open"
            //};

            //// Calculate expected cash
            //_expectedCash = _currentShift.OpeningCash + _currentShift.TotalSales;
            //_actualCash = _expectedCash; // Default to expected
            //_cashDifference = 0;

            //// Update the UI
            //UpdateUI();
        }

        private void UpdateUI()
        {
            //var vietnameseCulture = new System.Globalization.CultureInfo("vi-VN");

            //OpeningCashText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _currentShift.OpeningCash);
            //ExpectedCashText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _expectedCash);
            //ActualCashBox.Value = _actualCash;
            //CashDifferenceText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _cashDifference);
            //TotalSalesText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _currentShift.TotalSales);
            //TotalOrdersText.Text = _currentShift.TotalOrders.ToString();
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

            //// Update the shift
            //_currentShift.EndTime = DateTime.Now;
            //_currentShift.ClosingCash = _actualCash;
            //_currentShift.Status = "Closed";

            //// Show confirmation dialog
            //ContentDialog dialog = new ContentDialog
            //{
            //    Title = "Shift Closed",
            //    Content = "The shift has been closed successfully.",
            //    CloseButtonText = "OK",
            //    XamlRoot = this.XamlRoot
            //};

            //await dialog.ShowAsync();

            //// Navigate back to the home page
            //Frame.Navigate(typeof(HomePage));
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
