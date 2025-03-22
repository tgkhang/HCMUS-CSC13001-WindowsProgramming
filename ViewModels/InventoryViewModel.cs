using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;

namespace POS_For_Small_Shop.ViewModels
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        private IDao _dao;

        // Danh sách nguyên liệu hiển thị trong ListView
        public ObservableCollection<Ingredient> Ingredients { get; set; }

        // Nguyên liệu được chọn để chỉnh sửa hoặc xóa
        private Ingredient _selectedIngredient;
        public Ingredient SelectedIngredient
        {
            get => _selectedIngredient;
            set
            {
                _selectedIngredient = value;
                OnPropertyChanged();
            }
        }

        // Dữ liệu cho việc thêm mới nguyên liệu
        private Ingredient _newIngredient;
        public Ingredient NewIngredient
        {
            get => _newIngredient;
            set
            {
                _newIngredient = value;
                OnPropertyChanged();
            }
        }

        // Danh sách Category để bind vào ComboBox
        public ObservableCollection<Category> Categories { get; set; }

        public InventoryViewModel()
        {
            _dao = Service.GetKeyedSingleton<IDao>();

            // Lấy dữ liệu từ MockDao
            Ingredients = new ObservableCollection<Ingredient>(_dao.Ingredients.GetAll());
            Categories = new ObservableCollection<Category>(_dao.Categories.GetAll());

            // Khởi tạo dữ liệu mới
            NewIngredient = new Ingredient();
        }

        // 🔥 Load lại dữ liệu từ database
        public void LoadIngredients()
        {
            Ingredients.Clear();
            foreach (var ingredient in _dao.Ingredients.GetAll())
            {
                Ingredients.Add(ingredient);
            }
        }

        public void AddIngredient()
        {
            if (NewIngredient != null)
            {
                _dao.Ingredients.Insert(NewIngredient);
                LoadIngredients(); // Load lại danh sách sau khi thêm
                NewIngredient = new Ingredient(); // Reset lại form sau khi thêm
            }
        }

        // ✏️ Chỉnh sửa nguyên liệu
        public void EditIngredient()
        {
            if (SelectedIngredient != null)
            {
                _dao.Ingredients.Update(SelectedIngredient.IngredientID, SelectedIngredient);

                // Cập nhật lại vị trí trong ObservableCollection
                var index = Ingredients.IndexOf(Ingredients.First(x => x.IngredientID == SelectedIngredient.IngredientID));
                if (index >= 0)
                {
                    Ingredients[index] = SelectedIngredient;
                }
            }
        }

        // ❌ Xóa nguyên liệu
        public void DeleteIngredient()
        {
            if (SelectedIngredient != null)
            {
                if (Ingredients.Contains(SelectedIngredient))
                {
                    _dao.Ingredients.Delete(SelectedIngredient.IngredientID);
                    Ingredients.Remove(SelectedIngredient);
                    SelectedIngredient = null;
                }
            }
        }

        // 🚀 Kích hoạt sự kiện PropertyChanged để cập nhật UI
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}