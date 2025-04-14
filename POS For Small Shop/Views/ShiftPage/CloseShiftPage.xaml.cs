using System;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using POS_For_Small_Shop.Views;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;

namespace POS_For_Small_Shop.Views.ShiftPage
{
    public sealed partial class CloseShiftPage : Page
    {
        private IDao _dao;
        private Shift _currentShift;
        private float _expectedCash;
        private float _actualCash;
        private float _cashDifference;
        private IShiftService _shiftService;
        private TextBlock _messageTextBlock; 

        public CloseShiftPage()
        {
            this.InitializeComponent();

            // Get the DAO from the service
            _dao = Service.GetKeyedSingleton<IDao>();
            _shiftService = Service.GetKeyedSingleton<IShiftService>();

            _shiftService.ShiftUpdated += ShiftService_ShiftUpdated;

            // Create a text block for messages
            CreateMessageTextBlock();
            LoadCurrentShift();
        }

        private void CreateMessageTextBlock()
        {
            // Create a text block for displaying messages at the bottom of the page
            _messageTextBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0),
                FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                Visibility = Visibility.Collapsed
            };

            // Add it to the main grid at the bottom
            Grid mainGrid = (Grid)Content;
            if (mainGrid.RowDefinitions.Count >= 3)
            {
                // Add to the last row, below the action buttons
                Grid.SetRow(_messageTextBlock, 2);
                Grid.SetColumnSpan(_messageTextBlock, 2);
                mainGrid.Children.Add(_messageTextBlock);
            }
        }

        private void ShiftService_ShiftUpdated(object sender, EventArgs e)
        {
            LoadCurrentShift();
        }

        // Clean up event subscriptions when navigating away
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Unsubscribe from the event when navigating away to prevent memory leaks
            _shiftService.ShiftUpdated -= ShiftService_ShiftUpdated;
        }

        private void LoadCurrentShift()
        {
            _currentShift = _shiftService.CurrentShift;

            if (_currentShift == null)
            {
                DisplayMessage("No active shift found", Colors.Red);
                return;
            }

            if (_currentShift.Status == "Closed")
            {
                CloseShiftButton.IsEnabled = false;
                ActualCashBox.IsEnabled = false;
                DisplayMessage("This shift has already been closed.", Colors.Orange);
            }
            else
            {
                CloseShiftButton.IsEnabled = true;
                ActualCashBox.IsEnabled = true;
                HideMessage();
            }

            _expectedCash = _currentShift.OpeningCash + _currentShift.TotalSales;
            _actualCash = _expectedCash;
            _cashDifference = 0;

            UpdateUI();
        }

        private void UpdateUI()
        {
            // updating on the UI thread
            DispatcherQueue.TryEnqueue(() =>
            {
                var vietnameseCulture = new System.Globalization.CultureInfo("vi-VN");

                OpeningCashText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _currentShift.OpeningCash);
                ExpectedCashText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _expectedCash);
                ActualCashBox.Value = _actualCash;
                CashDifferenceText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _cashDifference);
                UpdateCashDifferenceColor();

                TotalSalesText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _currentShift.TotalSales);
                TotalOrdersText.Text = _currentShift.TotalOrders.ToString();
            });
        }

        private void UpdateCashDifferenceColor()
        {
            if (_cashDifference < 0)
            {
                CashDifferenceText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (_cashDifference > 0)
            {
                CashDifferenceText.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                // Use default text color
                CashDifferenceText.Foreground = (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
            }
        }

        private void ActualCashBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            try
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    try
                    {
                        double newValue = args.NewValue;


                        _actualCash = (float)newValue;
                        _cashDifference = _actualCash - _expectedCash;

                        if (CashDifferenceText != null)
                        {
                            var vietnameseCulture = new System.Globalization.CultureInfo("vi-VN");
                            CashDifferenceText.Text = string.Format(vietnameseCulture, "{0:#,##0} đ", _cashDifference);

                            if (_cashDifference < 0)
                            {
                                CashDifferenceText.Foreground = new SolidColorBrush(Colors.Red);
                            }
                            else if (_cashDifference > 0)
                            {
                                CashDifferenceText.Foreground = new SolidColorBrush(Colors.Green);
                            }
                            else
                            {
                                CashDifferenceText.Foreground = (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
                            }
                        }

                        if (Math.Abs(_cashDifference) > 10000)
                        {
                            var vietnameseCulture = new System.Globalization.CultureInfo("vi-VN");
                            if (_cashDifference < 0)
                            {
                                Debug.WriteLine($"Cash shortage: {Math.Abs(_cashDifference)}");
                            }
                            else
                            {
                                Debug.WriteLine($"Cash excess: {_cashDifference}");
                            }
                        }
                        else if (Math.Abs(_cashDifference) > 0)
                        {
                            Debug.WriteLine("Minor cash difference detected");
                        }
                    }
                    catch (Exception innerEx)
                    {
                        Debug.WriteLine($"Inner exception in ActualCashBox_ValueChanged: {innerEx.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Outer exception in ActualCashBox_ValueChanged: {ex.Message}");
            }
        }
        private void CloseShiftButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Disable the button to prevent multiple clicks
                CloseShiftButton.IsEnabled = false;

                _currentShift.EndTime = DateTime.Now;
                _currentShift.ClosingCash = (float)ActualCashBox.Value;
                _currentShift.Status = "Closed";

                bool success = _dao.Shifts.Update(_currentShift.ShiftID, _currentShift);

                if (!success)
                {
                    CloseShiftButton.IsEnabled = true;
                    DisplayMessage("Failed to close the shift. Please try again.", Colors.Red);
                    return;
                }

                // Update the shift service to notify all subscribers that the shift has been closed
                _shiftService.UpdateShift(_currentShift);
                DisplayMessage("The shift has been closed successfully.", Colors.Green);

                // Add a delay before navigating to home page to show the message
                DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, async () =>
                {
                    await System.Threading.Tasks.Task.Delay(2000);
                    NavigateToHomePage();
                });
            }
            catch (Exception ex)
            {
                CloseShiftButton.IsEnabled = true;
                DisplayMessage($"Error: {ex.Message}", Colors.Red);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the previous page
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            else
            {
                Frame.Navigate(typeof(NewOrderPage));
            }
        }

        private void DisplayMessage(string message, Windows.UI.Color color)
        {
            DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, () =>
            {
                Debug.WriteLine($"CloseShiftPage message: {message}");

                if (_messageTextBlock != null)
                {
                    _messageTextBlock.Text = message;
                    _messageTextBlock.Foreground = new SolidColorBrush(color);
                    _messageTextBlock.Visibility = Visibility.Visible;
                }
            });
        }

        private void HideMessage()
        {
            DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, () =>
            {
                if (_messageTextBlock != null)
                {
                    _messageTextBlock.Visibility = Visibility.Collapsed;
                }
            });
        }

        // Helper method to safely navigate to the home page
        private void NavigateToHomePage()
        {
            DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, () =>
            {
                var window = App.MainWindow;
                if (window != null && window.Content is Frame mainFrame)
                {
                    mainFrame.Navigate(typeof(HomePage));
                }
            });
        }
    }
}