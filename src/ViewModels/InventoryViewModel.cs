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

        private IDao _dao;

        public InventoryViewModel()
        {
            _dao = Service.GetKeyedSingleton<IDao>();

            var items = _dao.Ingredients.GetAll();
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
                    var index = Ingredients.IndexOf(ingredient);
                    if (index >= 0)
                    {
                        Ingredients[index] = ingredient;
                    }
                }
            }
        }

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
