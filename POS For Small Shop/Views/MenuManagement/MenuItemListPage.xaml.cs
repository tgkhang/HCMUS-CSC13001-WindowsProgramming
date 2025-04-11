using System;
using System.Collections.Generic;
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
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.ViewModels.MenuManagement;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views.MenuManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuItemListPage : Page
    {
        public MenuItemListViewModel ViewModel { get; } = new MenuItemListViewModel();

        public MenuItemListPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;

            // Initialize the ViewModel and connect the ListView
            ViewModel.Initialize();
            ItemListView.ItemsSource = ViewModel.FilteredMenuItems;
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.SearchText = sender.Text;
                ViewModel.ApplyFilters();
                UpdateEmptyState();
            }
        }

        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle selection change if necessary
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IsEditMode = false;
            ViewModel.CurrentMenuItem = new MenuItem();
            FormHeaderText.Text = "Add Menu Item";
            ItemDetailsPanel.Visibility = Visibility.Visible;

            // Clear form fields
            NameTextBox.Text = string.Empty;
            PriceTextBox.Text = string.Empty;
            ImagePathTextBox.Text = string.Empty;
            // Reset image preview
            PreviewImage.Source = null;
            PreviewImage.Visibility = Visibility.Collapsed;

            PopulateCategoryComboBox();
            CategoryComboBox.SelectedIndex = -1;
        }

        private void EditItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int menuItemId)
            {
                MenuItem menuItem = ViewModel.AllMenuItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
                if (menuItem != null)
                {
                    ViewModel.CurrentMenuItem = menuItem;
                    NameTextBox.Text = menuItem.Name;
                    PriceTextBox.Text = menuItem.SellingPrice.ToString();
                    ImagePathTextBox.Text = menuItem.ImagePath ?? "";
                    FormHeaderText.Text = "Edit Menu Item";
                    ViewModel.IsEditMode = true;

                    // Handle image preview
                    if (!string.IsNullOrEmpty(menuItem.ImagePath))
                    {
                        try
                        {
                            // Try to create a BitmapImage from the path
                            BitmapImage bitmapImage = new BitmapImage();

                            // Check if the path is already in ms-appdata format
                            if (menuItem.ImagePath.StartsWith("ms-appdata:"))
                            {
                                PreviewImage.Source = new BitmapImage(new Uri(menuItem.ImagePath));
                            }
                            // Check if it's a local file path with MenuItemImages
                            else if (menuItem.ImagePath.Contains("\\MenuItemImages\\"))
                            {
                                // Extract the filename from the path
                                string fileName = System.IO.Path.GetFileName(menuItem.ImagePath);
                                string msAppDataPath = $"ms-appdata:///local/MenuItemImages/{fileName}";
                                PreviewImage.Source = new BitmapImage(new Uri(msAppDataPath));
                            }
                            // Try as a direct URI
                            else
                            {
                                PreviewImage.Source = new BitmapImage(new Uri(menuItem.ImagePath));
                            }

                            // Show the preview image
                            PreviewImage.Visibility = Visibility.Visible;
                        }
                        catch (Exception ex)
                        {
                            // If there's an error loading the image, hide the preview
                            Debug.WriteLine($"Error loading image preview: {ex.Message}");
                            PreviewImage.Source = null;
                            PreviewImage.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        // No image path, hide the preview
                        PreviewImage.Source = null;
                        PreviewImage.Visibility = Visibility.Collapsed;
                    }


                    PopulateCategoryComboBox();

                    for (int i = 0; i < CategoryComboBox.Items.Count; i++)
                    {
                        if (CategoryComboBox.Items[i] is Category category &&
                            category.CategoryID == menuItem.CategoryID)
                        {
                            CategoryComboBox.SelectedIndex = i;
                            break;
                        }
                    }

                    ItemDetailsPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                ShowError("Name is required.");
                return;
            }

            if (!float.TryParse(PriceTextBox.Text, out float price))
            {
                ShowError("Please enter a valid price.");
                return;
            }
            if (CategoryComboBox.SelectedItem == null)
            {
                ShowError("Please select a category.");
                return;
            }

            ViewModel.CurrentMenuItem.Name = NameTextBox.Text;
            ViewModel.CurrentMenuItem.SellingPrice = price;
            ViewModel.CurrentMenuItem.ImagePath = ImagePathTextBox.Text;
            if (CategoryComboBox.SelectedItem is Category selectedCategory)
            {
                ViewModel.CurrentMenuItem.CategoryID = selectedCategory.CategoryID;
            }

            bool success = ViewModel.SaveMenuItem();
          
            if (success)
            {
                ViewModel.LoadMenuItems();
                ViewModel.ApplyFilters();
                UpdateEmptyState();
                ItemDetailsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ItemDetailsPanel.Visibility = Visibility.Collapsed;
        }

        private async void ShowError(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int menuItemId)
            {
                var itemToDelete = ViewModel.AllMenuItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
                if (itemToDelete != null)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Delete Menu Item",
                        Content = $"Are you sure you want to delete {itemToDelete.Name}? This action cannot be undone.",
                        PrimaryButtonText = "Delete",
                        CloseButtonText = "Cancel",
                        DefaultButton = ContentDialogButton.Close,
                        XamlRoot = this.XamlRoot
                    };

                    var result = await dialog.ShowAsync();

                    if (result == ContentDialogResult.Primary)
                    {
                        bool success = ViewModel.DeleteMenuItem(menuItemId);
                        if (success)
                        {
                            ViewModel.ApplyFilters();
                            UpdateEmptyState();
                        }
                    }
                }
            }
        }

        private void UpdateEmptyState()
        {
            EmptyStateText.Visibility = ViewModel.FilteredMenuItems.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        private void PopulateCategoryComboBox()
        {
            // Clear existing items
            CategoryComboBox.Items.Clear();

            // Add categories from the ViewModel
            foreach (var category in ViewModel.AllCategories)
            {
                CategoryComboBox.Items.Add(category);
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void PickAPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            //disable the button to avoid double-clicking
            var senderButton = sender as Button;
            senderButton.IsEnabled = false;

            // Clear previous returned file name, if it exists, between iterations of this scenario
            ImagePathTextBox.Text = "";

            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var window = App.MainWindow;

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);


            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Get the local folder for storing images
                var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Debug.WriteLine($"Local folder path: {localFolder.Path}");

                // Create a subfolder for menu item images if it doesn't exist
                var imagesFolder = await localFolder.CreateFolderAsync("MenuItemImages",
                    Windows.Storage.CreationCollisionOption.OpenIfExists);

                // Create a unique filename with timestamp
                string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmss}_{file.Name}";

                // Copy the file to our app's local storage
                var copiedFile = await file.CopyAsync(imagesFolder, uniqueFileName,
                    Windows.Storage.NameCollisionOption.GenerateUniqueName);
                Debug.WriteLine($"File copied to: {copiedFile.Path}");

                ImagePathTextBox.Text = copiedFile.Path;

                // Store the path in ms-appdata format
                string imagePath = $"ms-appdata:///local/MenuItemImages/{copiedFile.Name}";
                Debug.WriteLine($"Image path: {imagePath}");

                PreviewImage.Source = new BitmapImage(new Uri(imagePath));
                PreviewImage.Visibility = Visibility.Visible;
            }
            else
            {
                Debug.WriteLine("File picking cancelled by user");
                ImagePathTextBox.Text = "Operation cancelled.";
            }

            //re-enable the button
            senderButton.IsEnabled = true;
        }
    }
}
