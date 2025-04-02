using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Ingredient> Ingredients { get; set; }
        public Ingredient? SelectedIngredient { get; set; }

        private readonly IDao _dao;

        public RelayCommand LoadIngredientsCommand { get; }

        public InventoryViewModel()
        {
            _dao = Service.GetKeyedSingleton<IDao>();

            LoadIngredients();

            LoadIngredientsCommand = new RelayCommand(LoadIngredients);
        }
        public void LoadIngredients()
        {
            var items = _dao.Ingredients.GetAll().ToList();
            Ingredients = new ObservableCollection<Ingredient>(items);
        }

        public void AddIngredient(Ingredient ingredient)
        {
            if (ingredient != null)
            {
                var result = _dao.Ingredients.Insert(ingredient);
                if (result)
                {
                    Ingredients.Add(ingredient);
                }
            }
        }

        public void SaveIngredient(Ingredient ingredient)
        {
            if (ingredient != null)
            {
                var result = _dao.Ingredients.Update(ingredient.IngredientID, ingredient);
                if (result)
                {
                    var index = Ingredients.IndexOf(Ingredients.First(i => i.IngredientID == ingredient.IngredientID));
                    if (index >= 0)
                    {
                        Ingredients[index] = ingredient;
                    }
                }
            }
        }

        // Xóa nguyên liệu
        public void DeleteIngredient(Ingredient ingredient)
        {
            if (ingredient != null)
            {
                var result = _dao.Ingredients.Delete(ingredient.IngredientID);
                if (result)
                {
                    Ingredients.Remove(ingredient);
                }
            }
        }

    }
}
