using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using PropertyChanged;
using POS_For_Small_Shop.Utils;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace POS_For_Small_Shop.ViewModels
{
    public class PromotionManagementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private IDao _dao;
        public ObservableCollection<Promotion> Promotions { get; set; }

        public ObservableCollection<MenuItem> AvaibleItems { get; set; }
        public ObservableCollection<MenuItem> SelectedItems { get; set; }
        public Promotion SelectedPromotion { get; set; }
        public Promotion NewPromotion { get; set; }

        public ICommand AddPromotionCommand { get; }
        public ICommand UpdatePromotionCommand { get; }
        public ICommand DeletePromotionCommand { get; }
        public ICommand SetSelectedItemsCommand { get; }
        public ICommand GetByPromotionNameCommand { get; }


        public PromotionManagementViewModel()
        {
            _dao = Service.GetKeyedSingleton<IDao>();

            Promotions = new ObservableCollection<Promotion>(_dao.Promotions.GetAll());
            AvaibleItems = new ObservableCollection<MenuItem>(_dao.MenuItems.GetAll());
            SelectedItems = new ObservableCollection<MenuItem>();
            NewPromotion = new Promotion();


            // Initialize the commands
            AddPromotionCommand = new RelayCommand(AddPromotion, () => true);
            UpdatePromotionCommand = new RelayCommand(UpdateSelectedPromotion, () => SelectedPromotion != null);
            DeletePromotionCommand = new RelayCommand(DeleteSelectedPromotion, () => SelectedPromotion != null);
        }

        public void AddPromotion()
        {
            NewPromotion.ItemIDs = SelectedItems.Select(item => item.MenuItemID).ToList();
            _dao.Promotions.Insert(NewPromotion);
            Promotions.Add(NewPromotion);
            NewPromotion = new Promotion();
        }


        public void setSelectedItems(List<int> menuIDs)
        {
            SelectedItems.Clear();
            foreach (int id in menuIDs)
            {
                SelectedItems.Add(_dao.MenuItems.GetById(id));
            }
        }

        public void UpdateSelectedPromotion()
        {
            if (SelectedPromotion != null)
            {
                // Update the ItemIDs list
                SelectedPromotion.ItemIDs = SelectedItems.Select(item => item.MenuItemID).ToList();

                // Update the database
                _dao.Promotions.Update(SelectedPromotion.PromoID, SelectedPromotion);

                // Find the index of the selected promotion in the ObservableCollection
                var index = Promotions.IndexOf(SelectedPromotion);
                if (index >= 0)
                {
                    Promotions[index] = SelectedPromotion; // Triggers UI update
                }
            }
        }

        public void DeleteSelectedPromotion()
        {
            if (SelectedPromotion != null)
            {
                bool isDeleted = _dao.Promotions.Delete(SelectedPromotion.PromoID);
                if (isDeleted)
                {
                    Promotions.Remove(SelectedPromotion);
                }
                else
                {
                    Debug.WriteLine($"Failed to delete Promotion: {SelectedPromotion.PromoName}");
                }
            }
        }

        public void SearchPromotionByName(string query)
        {
            //Debug.WriteLine($"Searching for promotions with name: {query}");
            var filteredResults = _dao.Promotions.GetAll()
                .Where(p => p.PromoName.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList(); 

            Promotions.Clear(); 

            foreach (var promo in filteredResults)
            {
                Promotions.Add(promo); 
            }
        }

    }
}
