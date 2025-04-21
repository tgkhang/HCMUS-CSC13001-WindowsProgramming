using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using PropertyChanged;

namespace POS_For_Small_Shop.ViewModels.Inventory
{
    [AddINotifyPropertyChangedInterface]
    public class InventoryViewModel : INotifyPropertyChanged
    {
        private IDao _dao;
        private string _searchText = "";
        public List<Ingredient> AllIngredients { get; private set; } = new List<Ingredient>();


        public List<Category> AllCategories { get; private set; } = new List<Category>();

        public ObservableCollection<Ingredient> FilteredIngredients { get; private set; } = new ObservableCollection<Ingredient>();

        public ObservableCollection<string> AllUnits { get; } = new ObservableCollection<string>
        {
            "kg", "g", "L", "ml", "pcs"
        };

        public Ingredient CurrentIngredient { get; set; }
        
        public bool IsEditMode { get; set; } = false;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                ApplyFilters();
            }
        }
        public InventoryViewModel()
        {
            _dao = Service.GetKeyedSingleton<IDao>();
        }

        public void Initialize()
        {
            LoadIngredient();
        }

        public void LoadIngredient()
        {
            try
            {
                AllIngredients = _dao.Ingredients.GetAll();
                AllCategories = _dao.Categories.GetAll();
                ApplyFilters();
            }
            catch (NotImplementedException)
            {
                AllIngredients = new List<Ingredient>();
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
            var filteredItems = AllIngredients;
            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                filteredItems = filteredItems.Where(item =>
                    item.IngredientName.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            FilteredIngredients.Clear();
            foreach (var item in filteredItems)
            {
                FilteredIngredients.Add(item);
            }

        }

        public bool SaveIngredient()
        {
            bool success;

            if (IsEditMode)
            {
                success = _dao.Ingredients.Update(CurrentIngredient.IngredientID, CurrentIngredient);
                if (success)
                {
                    var existingIngredient = AllIngredients.FirstOrDefault(item => item.IngredientID == CurrentIngredient.IngredientID);
                    if (existingIngredient != null)
                    {
                        existingIngredient.IngredientName = CurrentIngredient.IngredientName;
                        existingIngredient.CategoryID = CurrentIngredient.CategoryID;
                        existingIngredient.Stock = CurrentIngredient.Stock;
                        existingIngredient.Unit = CurrentIngredient.Unit;
                        existingIngredient.Supplier = CurrentIngredient.Supplier;
                        existingIngredient.PurchasePrice = CurrentIngredient.PurchasePrice;
                        existingIngredient.ExpiryDate = CurrentIngredient.ExpiryDate;
                    }
                }
            }
            else
            {
                success = _dao.Ingredients.Insert(CurrentIngredient);
                if (success)
                {
                    CurrentIngredient.IngredientID = AllIngredients.Count > 0 ?
                        AllIngredients.Max(item => item.IngredientID) + 1 : 1;
                    AllIngredients.Add(CurrentIngredient);
                }
            }

            return success;
        }

        public bool DeleteIngredient(int ingredientId)
        {
            bool success = _dao.Ingredients.Delete(ingredientId);

            if (success)
            {
                var itemToRemove = AllIngredients.FirstOrDefault(item => item.IngredientID == ingredientId);
                if (itemToRemove != null)
                {
                    AllIngredients.Remove(itemToRemove);
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
