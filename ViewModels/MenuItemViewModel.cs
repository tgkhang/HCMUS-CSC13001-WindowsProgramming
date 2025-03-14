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
    public class MenuItemViewModel
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; }

        private IDao _dao;

        public MenuItemViewModel()
        {
            var _dao = Service.GetKeyedSingleton<IDao>("Category");
            var items = _dao.MenuItems.GetAll();
            MenuItems = new ObservableCollection<MenuItem>(items);
        }
    }
}
