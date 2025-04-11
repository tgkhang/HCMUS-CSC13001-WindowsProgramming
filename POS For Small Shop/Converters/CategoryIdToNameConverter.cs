using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Data;
using POS_For_Small_Shop.Services;
namespace POS_For_Small_Shop.Converters
{
    public class CategoryIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int categoryId)
            {
                var dao = Service.GetKeyedSingleton<IDao>();
                var categories = dao.Categories.GetAll();
                var category = categories.FirstOrDefault(c => c.CategoryID == categoryId);
                return category?.Name ?? "Unknown";
            }
            return "Unknown";
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string categoryName)
            {
                if (categoryName == "Unknown")
                    return 0; 
                var dao = Service.GetKeyedSingleton<IDao>();
                var categories = dao.Categories.GetAll();
                var category = categories.FirstOrDefault(c => c.Name == categoryName);
                if (category != null)
                    return category.CategoryID;

                return 0; // or another default value
            }
            return 0;
        }
    }
}