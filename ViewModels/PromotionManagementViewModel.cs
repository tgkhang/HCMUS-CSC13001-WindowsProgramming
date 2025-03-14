using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using PropertyChanged;

namespace POS_For_Small_Shop.ViewModels
{
    class PromotionManagementViewModel
    {
        private IDao _dao;
        public ObservableHashSet<Promotion> Promotions { get; set; }

        public PromotionManagementViewModel()
        {
            _dao = Service.GetKeyedSingleton<IDao>();
            var promos = _dao.Promotions.GetAll();
            Promotions = new ObservableHashSet<Promotion>(promos);
        }
    }
}
