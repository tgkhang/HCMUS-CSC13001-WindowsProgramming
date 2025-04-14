using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.UI.Xaml;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using System.Collections.ObjectModel;
using PropertyChanged;

namespace POS_For_Small_Shop.ViewModels.MenuManagement
{
    [AddINotifyPropertyChangedInterface]
    public class MenuItemListViewModel : INotifyPropertyChanged
    {
        private IDao _dao;
        private string _searchText = "";
        private int _selectedCategoryFilter = 0;

        public List<MenuItem> AllMenuItems { get; private set; } = new List<MenuItem>();
        public List<Category> AllCategories { get; private set; } = new List<Category>();
        public ObservableCollection<MenuItem> FilteredMenuItems { get; private set; } = new ObservableCollection<MenuItem>();
        public MenuItem CurrentMenuItem { get; set; }
        public bool IsEditMode { get; set; } = false;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        public int SelectedCategoryFilter
        {
            get => _selectedCategoryFilter;
            set
            {
                _selectedCategoryFilter = value;
                OnPropertyChanged(nameof(SelectedCategoryFilter));
            }
        }

        public MenuItemListViewModel()
        {
            _dao = Service.GetKeyedSingleton<IDao>();
        }

        public void Initialize()
        {
            LoadMenuItems();//load menuitem and categories
        }

        public void LoadMenuItems()
        {
            try
            {
                AllMenuItems = _dao.MenuItems.GetAll();
                AllCategories = _dao.Categories.GetAll();
                ApplyFilters();
            }
            catch (NotImplementedException)
            {
                AllMenuItems = new List<MenuItem>();
                AllCategories = new List<Category>();
                ApplyFilters();
            }
        }

        // Get category name by ID helper method
        public string GetCategoryNameById(int categoryId)
        {
            var category = AllCategories.FirstOrDefault(c => c.CategoryID == categoryId);
            return category?.Name ?? "Unknown";
        }

        // Get category by ID helper method
        public Category GetCategoryById(int categoryId)
        {
            return AllCategories.FirstOrDefault(c => c.CategoryID == categoryId);
        }

        public void ApplyFilters()
        {
            var filteredItems = AllMenuItems;
            if (_selectedCategoryFilter > 0)
            {
                filteredItems = filteredItems.Where(item => item.CategoryID == _selectedCategoryFilter).ToList();
            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredItems = filteredItems.Where(item =>
                item.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            FilteredMenuItems.Clear();
            foreach (var item in filteredItems)
            {
                FilteredMenuItems.Add(item);
            }
        }

        public bool SaveMenuItem()
        {
            bool success;

            if (IsEditMode)
            {
                //Debug.WriteLine($"Image path: {CurrentMenuItem.ImagePath}");
                //Debug.WriteLine($"Selling price: {CurrentMenuItem.SellingPrice}");
                //Debug.WriteLine($"Name: {CurrentMenuItem.Name}");
                success = _dao.MenuItems.Update(CurrentMenuItem.MenuItemID, CurrentMenuItem);
                if (success)
                {
                    var existingItem = AllMenuItems.FirstOrDefault(item => item.MenuItemID == CurrentMenuItem.MenuItemID);
                    if (existingItem != null)
                    {
                        existingItem.Name = CurrentMenuItem.Name;
                        existingItem.SellingPrice = CurrentMenuItem.SellingPrice;
                        existingItem.CategoryID = CurrentMenuItem.CategoryID;
                        existingItem.ImagePath = CurrentMenuItem.ImagePath;
                    }
                }
            }
            else
            {
                success = _dao.MenuItems.Insert(CurrentMenuItem);
                if (success)
                {
                    CurrentMenuItem.MenuItemID = AllMenuItems.Count > 0 ? AllMenuItems.Max(item => item.MenuItemID) + 1 : 1;
                    AllMenuItems.Add(CurrentMenuItem);
                }
            }

            return success;
        }

        public bool DeleteMenuItem(int menuItemId)
        {
            bool success = _dao.MenuItems.Delete(menuItemId);

            if (success)
            {
                var menuItemToRemove = AllMenuItems.FirstOrDefault(item => item.MenuItemID == menuItemId);
                if (menuItemToRemove != null)
                {
                    AllMenuItems.Remove(menuItemToRemove);
                    ApplyFilters();
                }
            }

            return success;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}