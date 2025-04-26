using Microsoft.UI.Xaml.Controls;
using POS_For_Small_Shop.Views;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;
using POS_For_Small_Shop.Services;
using POS_For_Small_Shop.Views.Inventory;
using static SkiaSharp.HarfBuzz.SKShaper;
using Windows.UI.Notifications;

namespace POS_For_Small_Shop.Services
{

    public class Notification
    {
        public string Message { get; set; }
        public string Target { get; set; } // The target page to navigate to (PromotionPage, InventoryPage, etc.)
        public string ItemName { get; set; } // The name of the item related to the notification
    }

    [AddINotifyPropertyChangedInterface]
    public class NotificationService
    {
        public ObservableCollection<Notification> Notifications { get; set; } = new ObservableCollection<Notification>();

        private Frame _frame; // Storage the frame to navigate to the page
        private IDao _dao = Service.GetKeyedSingleton<IDao>(); // Storage the dao to access the database

        public NotificationService(Frame frame)
        {
            _frame = frame;

            LoadPromotionNotification();
            LoadInventoryNotification();

        }

        // Load the notification of promotion
        public void LoadPromotionNotification()
        {
            var promotions = _dao.Promotions.GetAll();
            var today = DateTime.Today;

            foreach (var promo in promotions)
            {
                if (promo.StartDate <= today && promo.EndDate >= today)
                {
                    Notifications.Add(new Notification
                    {
                        Message = $"Promotion \"{promo.PromoName}\" is active today.",
                        Target = "Promotion",
                        ItemName = promo.PromoName
                    });
                }

                if (promo.EndDate == today)
                {
                    Notifications.Add(new Notification
                    {
                        Message = $"Promotion \"{promo.PromoName}\" expires today!",
                        Target = "Promotion",
                        ItemName = promo.PromoName
                    });
                }

                if (promo.EndDate == today.AddDays(-1))
                {
                    Notifications.Add(new Notification
                    {
                        Message = $"Promotion \"{promo.PromoName}\" just expired yesterday.",
                        Target = "Promotion",
                        ItemName = promo.PromoName
                    });
                }
            }
        }

        // Loadd the notification of inventory
        public void LoadInventoryNotification()
        {
            var ingredients = _dao.Ingredients.GetAll();
            var today = DateTime.Today;

            foreach (var item in ingredients)
            {
                if ((item.Unit == "kg" && item.Stock <= 2) ||
                    (item.Unit == "g" && item.Stock <= 300) ||
                    (item.Unit == "L" && item.Stock <= 2) ||
                    (item.Unit == "ml" && item.Stock <= 300) ||
                    (item.Unit == "pcs" && item.Stock <= 10))
                {
                    Notifications.Add(new Notification
                    {
                        Message = $"\"{item.IngredientName}\" is running low. Check it out.",
                        Target = "Inventory",
                        ItemName = item.IngredientName
                    });
                }
                if(item.ExpiryDate <  DateTime.Today) {
                    Notifications.Add(new Notification
                    {
                        Message = $"\"{item.IngredientName}\" is expired. Check it out.",
                        Target = "Inventory",
                        ItemName = item.IngredientName
                    });
                }
                if (item.ExpiryDate == DateTime.Today)
                {
                    Notifications.Add(new Notification
                    {
                        Message = $"\"{item.IngredientName}\" will be expired today. Check it out.",
                        Target = "Inventory",
                        ItemName = item.IngredientName
                    });
                }
            }

        }

        public void GoToTarget(Notification notification)
        {
            Type? targetPage = notification.Target switch
            {
                "Promotion" => typeof(PromotionManagementPage),
                "Inventory" => typeof(InventoryPage),
                _ => null
            };

            if (targetPage != null)
            {
                _frame.Navigate(targetPage, notification.ItemName);
            }
        }
    }
}
