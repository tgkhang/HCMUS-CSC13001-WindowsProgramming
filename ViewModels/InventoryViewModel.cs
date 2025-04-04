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

namespace POS_For_Small_Shop.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class InventoryViewModel
    {
        private IDao _dao;
        private string _searchText = "";
        public List<Ingredient> AllIngredients { get; private set; } = new List<Ingredient>();

        public ObservableCollection<Ingredient> FilteredIngredients { get; private set; } = new ObservableCollection<Ingredient>();
        public Ingredient CurrentIngredient { get; set; }
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
                ApplyFilters();
            }
            catch (NotImplementedException)
            {
                AllIngredients = new List<Ingredient>();
                ApplyFilters();
            }
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

            OnPropertyChanged(nameof(FilteredIngredients));
        }

        public bool SaveIngredient()
        {
            bool success;

            if (IsEditMode)
            {
                success = _dao.Ingredients.Update(CurrentIngredient.IngredientID, CurrentIngredient);
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
            bool success = _dao.MenuItems.Delete(ingredientId);

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
