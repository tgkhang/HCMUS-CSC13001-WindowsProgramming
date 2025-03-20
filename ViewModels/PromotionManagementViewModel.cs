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

        public PromotionManagementViewModel()
        {
            _dao = Service.GetKeyedSingleton<IDao>();

            Promotions = new ObservableCollection<Promotion>(_dao.Promotions.GetAll());
            AvaibleItems = new ObservableCollection<MenuItem>(_dao.MenuItems.GetAll());
            SelectedItems = new ObservableCollection<MenuItem>();
            NewPromotion = new Promotion();
           
        }

        public void AddPromotion()
        {
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

    }
}
